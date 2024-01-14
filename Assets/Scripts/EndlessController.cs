using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndlessController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Slider _slider;
    private Animator anim;
    public bool indestructible;
    public ObjectMove cameraMovement;
    public float speed;
    private bool damaged;
    public TMP_Text pressureText;
    public TMP_Text distanceText;
    public TMP_Text speedText;
    public int gravity = 0;
    //public float percentage = 0.31f;
    public GameObject end_menu;
    public TMP_Text EndText1;
    public TMP_Text EndText2;
    private int total_distance = 0;
    public AudioSource rock_sound;
    public AudioSource radar_sound;
    private Boolean sound_radar;
    private Boolean calculate_distance;
    string user;
    string level;
    int dir = 1;
    public GameObject heart;


    private bool increase_speed = true;

    private bool isAccelerating;
    private bool isDecelerating;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damaged = false;
        isAccelerating = false;
        isDecelerating = false;
        Main.speed = 2.5f;
        speed = Main.speed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1f;
        sound_radar = true;
        calculate_distance = true;
        user = Main.currentUser.ToString();
        level = SceneManager.GetActiveScene().buildIndex.ToString();
        Debug.Log("user1 = " + user);
        Debug.Log("level = " + level);
        if (Main.currentUser != -1) StartCoroutine(Main.Instance.DBController.GetUserLevelAttempt(user, level));
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * dir, rb.velocity.y);
        rb.gravityScale = _slider.value;
        cameraMovement.rb.velocity = new Vector2(Main.speed * dir, 0);

        pressureText.text = "                 " + (int)(rb.velocity.y * 1) + " atm";
        distanceText.text = "                 " + total_distance + "m";
        int aux1 = (int)(rb.velocity.x * 10);
        speedText.text = aux1/10 + "." + aux1%10 + "m/s";

        if (increase_speed) StartCoroutine(IncreaseSpeed());
        if (calculate_distance) StartCoroutine(CalculateDistance());
        if (sound_radar) StartCoroutine(PlayRadar5Second());

        // accelerate
        if (isAccelerating)
        {
            Debug.Log("isAccelerating =  perro");

            if (this.transform.position.x >= cameraMovement.transform.position.x + 8f) isAccelerating = false;

            speed = Main.speed*2f;
            Debug.Log("isAccelerating = true perro");

        }
        // decelerate
        else if (isDecelerating)
        {
            if (this.transform.position.x <= cameraMovement.transform.position.x - 8f) isDecelerating = false;
            speed = Main.speed*0.33f;
        }
        else speed = Main.speed;

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
        if (collision.gameObject.tag == "Mine")
        {
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
        average_score = (int)((Main.attempts * average_score) + total_distance) / attempts;
        float new_score = total_distance;
        if ((int)new_score > Main.max_score) Main.max_score = (int)new_score;
        //if (percentage == 1f && damaged == false) Main.perfectly_completed = 1;

        if (Main.currentUser != -1) StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(user, level, attempts.ToString(), average_score.ToString(), Main.max_score.ToString(), Main.perfectly_completed.ToString()));
    }


    IEnumerator Endgame()
    {
        yield return new WaitForSeconds(1f);
        EndText1.text = "GAME OVER!";
        EndText2.text = "SCORE = "+total_distance +"m";
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

    IEnumerator CalculateDistance()
    {
        calculate_distance = false;
        // CalculatePercentage(Vector2.Distance(this.transform.position, chest.transform.position));
        total_distance = (int)this.transform.position.x;
        yield return new WaitForSeconds(0.2f);
        calculate_distance = true;
    }

    IEnumerator IncreaseSpeed()
    {
        increase_speed = false;
        // CalculatePercentage(Vector2.Distance(this.transform.position, chest.transform.position));
        Main.speed = Main.speed * 1.0014f;
        speed = Main.speed;
        yield return new WaitForSeconds(0.3f);
        increase_speed = true;
    }
}