using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Slider _slider;
    private Animator anim;
    // public new Camera mainCamera;
    //public SoundController soundController;
    public bool indestructible;
    public ObjectMove cameraMovement;
    public float speed;
    private bool damaged;
    public TMP_Text pressureText;
    public Slider percentageSlider;
    public int gravity = 0;
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
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1f;
        total_distance = Vector2.Distance(this.transform.position, chest.transform.position);
        sound_radar = true;
        calculate_percentage = true;
        user = Main.currentUser.ToString();
        level = SceneManager.GetActiveScene().buildIndex.ToString();
        Debug.Log("user1 = " + user);
        Debug.Log("level = " + level);
        if(Main.currentUser != -1) StartCoroutine(Main.Instance.DBController.GetUserLevelAttempt(user, level));

        Debug.Log(" Main.currentUser =" + Main.currentUser);

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * dir, rb.velocity.y);
        rb.gravityScale = _slider.value;
        Vector3 x = this.transform.position;
        cameraMovement.rb.velocity = new Vector2(3f*dir, 0);

        if (calculate_percentage) StartCoroutine(Percentage()); //cada 0.2 segons calcula el percentatge de nivell que s'ha recorregut

        pressureText.text = "                 " + (int)(rb.velocity.y * 1) + " atm";
        percentageSlider.value = percentage;
        if (sound_radar) StartCoroutine(PlayRadar5Second());

        if (dir == 1)
        {
            if (isAccelerating)
            {
                if (this.transform.position.x >= cameraMovement.transform.position.x + 8f) isAccelerating = false;
                speed = 6f;
                Debug.Log("isAccelerating = true perro");

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
            if (isAccelerating)
            {
                if (this.transform.position.x <= cameraMovement.transform.position.x - 8f) isAccelerating = false;
                speed = 1f;
                Debug.Log("isAccelerating = true perro");

            }
            else if (isDecelerating)
            {
                if (this.transform.position.x >= cameraMovement.transform.position.x + 8f) isDecelerating = false;
                speed = 6f;
            }
            else speed = 3f;
        }

    }


    // Event handler for the Accelerate button
    public void PointerUpAccelerate()
    {
        isAccelerating = false;
        Debug.Log("isAccelerating = false perro");

    }
    public void PointerDownAccelerate()
    {
        //if (this.transform.position.x < cameraMovement.transform.position.x + 5f)
        isAccelerating = true;

            Debug.Log("isAccelerating = true");
    }

    // Event handler for the Decelerate button
    public void PointerUpDecelerate()
    {
        isDecelerating = false;
        Debug.Log("isDecelerating = false");

    }
    public void PointerDownDecelerate()
    {
        //if (this.transform.position.x > cameraMovement.transform.position.x - 5f)
        isDecelerating = true;
        Debug.Log("isDecelerating = true");

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mine" || collision.gameObject.tag == "Patrol" || collision.gameObject.tag == "Cave")
        {
            if (!indestructible)
            {
                if (damaged == false)
                {
                    heart.SetActive(false);
                    if (collision.gameObject.tag == "Cave")
                    {
                        rock_sound.Play();
                    }

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
            //cameraMovement.speed = speed;
            //this.transform.Rotate(0f, 180f, 0f, Space.Self);
            //if (!damaged) StartCoroutine(Main.Instance.DBController.DeleteUserLevelAttempt(user, level));
        }


        else if (collision.gameObject.tag == "Chest")
        {
            dir = -1;
            //cameraMovement.speed = speed;
            this.transform.Rotate(0f, 180f, 0f, Space.Self);
            //if (!damaged) StartCoroutine(Main.Instance.DBController.DeleteUserLevelAttempt(user, level));
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
        Debug.Log(Main.attempts);
        Debug.Log(Main.average_score);
        Debug.Log(Main.max_score);
        Debug.Log(Main.perfectly_completed);

        //update stats
        int attempts = Main.attempts + 1;
        int average_score = Main.average_score;
        average_score = (int)((Main.attempts * average_score) + (percentage * 100)) / attempts;
        float new_score = percentage * 100;
        if ((int)new_score > Main.max_score) Main.max_score = (int)new_score;
        if (percentage == 1f && damaged == false && star_count == 3) Main.perfectly_completed = 1;

        if (Main.currentUser != -1) StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(user, level, attempts.ToString(), average_score.ToString(), Main.max_score.ToString(), Main.perfectly_completed.ToString()));
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
        yield return new WaitForSeconds(1f);
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
        yield return new WaitForSeconds(0.2f);
        calculate_percentage = true;
    }
}