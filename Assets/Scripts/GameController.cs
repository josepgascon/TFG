using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Slider _slider;
    private Animator anim;
    public bool indestructible;
    public ObjectMove cameraMovement;
    private float speed;
    private bool damaged;
    public Slider percentageSlider;
    public float percentage = 0.31f;
    public GameObject end_menu;
    public TMP_Text EndText1;
    public TMP_Text EndText2;
    public GameObject chest;
    private float total_distance;
    public AudioSource rock_sound;
    public AudioSource radar_sound;
    private Boolean sound_radar;
    private Boolean calculate_percentage;
    string user;
    string level;
    int dir;
    public GameObject heart;
    private int star_count = 0;
    public Image star1;
    public Image star2;
    public Image star3;
    public Sprite star;
    private bool finish = false;

    private bool isAccelerating;
    private bool isDecelerating;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damaged = false;
        isAccelerating = false;
        isDecelerating = false; 
        dir = 1;
    }

    void Start()
    {
        Debug.Log("_slider.transform.position = " + _slider.transform.position);
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1f;
        total_distance = Vector2.Distance(this.transform.position, chest.transform.position);
        sound_radar = true;
        calculate_percentage = true;
        user = Main.currentUser.ToString();
        level = SceneManager.GetActiveScene().buildIndex.ToString();
        Debug.Log("user1 = " + user);
        Debug.Log("level = " + level);
        Debug.Log("(Main.attempts = " + Main.attempts);
        if (Main.currentUser != -1)
        {
            StartCoroutine(Main.Instance.DBController.GetUserLevelAttempt(user, level));
            Debug.Log("estic dins1:C");
        }


        Debug.Log(" Main.currentUser =" + Main.currentUser);
        AdsManager.Instance.bannerAds.HideBannerAd();
        //StartCoroutine(DisplayBannerWithDelay());
    }


    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * dir, - (_slider.value * 5.3f));
        //rb.velocity = new Vector2(speed * dir, rb.velocity.y);
        //rb.gravityScale = _slider.value;

        Vector3 x = this.transform.position;
        cameraMovement.rb.velocity = new Vector2(3f*dir, 0);

        if (calculate_percentage) StartCoroutine(Percentage()); //cada 0.2 segons calcula el percentatge de nivell que s'ha recorregut

        percentageSlider.value = percentage;
        if (sound_radar) StartCoroutine(PlayRadar5Second());

        if (dir == 1)
        {
            if (isAccelerating)
            {
                if (this.transform.position.x >= cameraMovement.transform.position.x + 8f) isAccelerating = false;
                speed = 6f;
            }
            else if (isDecelerating)
            {
                if (this.transform.position.x <= cameraMovement.transform.position.x - 8f) isDecelerating = false;
                speed = 1f;
            }
            else speed = 3f;
        }
        else
        {
            if (isDecelerating)
            {
                if (this.transform.position.x <= cameraMovement.transform.position.x - 8f) isDecelerating = false;
                speed = 6f;
            }
            else if (isAccelerating)
            {
                if (this.transform.position.x >= cameraMovement.transform.position.x + 8f) isAccelerating = false;
                speed = 1f;
            }
            else speed = 3f;
        }

    }

    public void SliderPointerUp()
    {
        _slider.value = 0f;
        //_slider.transform.Translate(0f, -15f, 0f, Space.Self);
    }
    public void SliderPointerDown()
    {
        //_slider.transform.Translate(0f, 15f, 0f, Space.Self);
    }


    // Event handler for the Accelerate button
    public void PointerUpAccelerate()
    {
        isAccelerating = false;
    }
    public void PointerDownAccelerate()
    {
        //if (this.transform.position.x < cameraMovement.transform.position.x + 5f)
        isAccelerating = true;
    }

    // Event handler for the Decelerate button
    public void PointerUpDecelerate()
    {
        isDecelerating = false;
    }
    public void PointerDownDecelerate()
    {
        //if (this.transform.position.x > cameraMovement.transform.position.x - 5f)
        isDecelerating = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mine" || collision.gameObject.tag == "Patrol" || collision.gameObject.tag == "Cave")
        {
            if (collision.gameObject.tag == "Cave") rock_sound.Play();
            if (!indestructible)
            {
                if (damaged == false)
                {
                    heart.SetActive(false);
                    anim.Play("IdleDamagedPlayer");
                    damaged = true;

                }
                else
                {
                    if (Main.currentUser != -1) StartCoroutine(Main.Instance.DBController.DeleteUserLevelAttempt(user, level));
                    anim.Play("PlayerExplosion");
                    StartCoroutine(Endgame());
                }
            }
        }

        else if (collision.gameObject.tag == "Star")
        {
            star_count++;
            if (star_count == 1) star1.sprite = star;
            else if (star_count == 2) star2.sprite = star;
            else if (star_count == 3) star3.sprite = star;
        }


        else if (collision.gameObject.tag == "Chest")
        {
            dir = -1;
            this.transform.Rotate(0f, 180f, 0f, Space.Self);
        }

        else if (collision.gameObject.tag == "Finish")
        {
            if (Main.currentUser != -1) StartCoroutine(Main.Instance.DBController.DeleteUserLevelAttempt(user, level));
            EndText1.text = "YOU WON!";
            finish = true;
            StartCoroutine(Endgame());
        }

    }

    private void SendStats()
    {
        Debug.Log("user = " + user);
        Debug.Log("level = " + level);

        Debug.Log(Main.user_level_id);
        Debug.Log("main attempts =" + Main.attempts);
        Debug.Log(Main.average_score);
        Debug.Log(Main.max_score);
        Debug.Log(Main.perfectly_completed);

        //first attempt
        if (Main.attempts == 0 && Main.currentUser != -1)
        {
            int attempts = 1;
            int average_score = (int)(percentage * 100) / attempts;
            float new_score = percentage * 100;
            Main.max_score = (int)new_score;
            if (percentage == 1f && damaged == false && star_count == 3) Main.perfectly_completed = 1;
            Debug.Log("estic molt dins");

            StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(user, level, attempts.ToString(), average_score.ToString(), Main.max_score.ToString(), Main.perfectly_completed.ToString()));
        }
        //update stats
        else if (Main.currentUser != -1)
        {
            int attempts = Main.attempts + 1;
            int average_score = Main.average_score;
            average_score = (int)((Main.attempts * average_score) + (percentage * 100)) / attempts;
            float new_score = percentage * 100;
            if ((int)new_score > Main.max_score) Main.max_score = (int)new_score;
            if (percentage == 1f && damaged == false && star_count == 3) Main.perfectly_completed = 1;

            StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(user, level, attempts.ToString(), average_score.ToString(), Main.max_score.ToString(), Main.perfectly_completed.ToString()));

        }
    }
       

    void CalculatePercentage(float new_distance)
    {
        if (dir > 0)
        {
            percentage = (total_distance - new_distance) / (2 * total_distance);
        }
        else
        {
            percentage = (total_distance + new_distance) / (2 * total_distance);
        }
    }

    IEnumerator Endgame()
    {
        yield return new WaitForSeconds(0.7f);
        int games = PlayerPrefs.GetInt("games_played", 0);
        PlayerPrefs.SetInt("games_played", ++games);
        if (games % 4 == 0) AdsManager.Instance.interstitialAds.ShowInterstitialAd();

        if (finish) percentage = 1f;
        EndText2.text = (int)(percentage * 100) + "% COMPLETED";
        end_menu.SetActive(true);
        Time.timeScale = 0;
        SendStats();
    }

    IEnumerator PlayRadar5Second()
    {
        sound_radar = false;
        radar_sound.Play();
        yield return new WaitForSeconds(5f);
        sound_radar = true;
    }

    IEnumerator Percentage()
    {
        calculate_percentage = false;
        CalculatePercentage(Vector2.Distance(this.transform.position, chest.transform.position));

        if((Vector2.Distance(this.transform.position, cameraMovement.transform.position) > 19)){
            if (Main.currentUser != -1) StartCoroutine(Main.Instance.DBController.DeleteUserLevelAttempt(user, level));
            anim.Play("PlayerExplosion");
            StartCoroutine(Endgame());
        } 

        yield return new WaitForSeconds(0.2f);
        calculate_percentage = true;
    }
}