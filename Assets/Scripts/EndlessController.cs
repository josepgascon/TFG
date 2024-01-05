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
    public int gravity = 0;
    public float percentage = 0.31f;
    public GameObject end_menu;
    public TMP_Text EndText1;
    public TMP_Text EndText2;
    private float total_distance;
    public AudioSource rock_sound;
    public AudioSource radar_sound;
    private Boolean sound_radar;
    private Boolean calculate_percentage;
    string user;
    string level;
    int dir;

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
        dir = 1;
        Main.speed = 3f;
        speed = Main.speed;

    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1f;
        total_distance = 0f;// Vector2.SqrMagnitude(this.transform.position - chest.transform.position);
        sound_radar = true;
        calculate_percentage = true;
        user = Main.currentUser.ToString();
        level = SceneManager.GetActiveScene().buildIndex.ToString();
        Debug.Log("user1 = " + user);
        Debug.Log("level = " + level);
        StartCoroutine(Main.Instance.DBController.GetUserLevelAttempt(user, level));

        Debug.Log("game_controller");

        Debug.Log("audio_enabled = " + Main.AudioEnabled);


    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * dir, rb.velocity.y);
        rb.gravityScale = _slider.value;
      //  Vector3 x = this.transform.position;
        cameraMovement.rb.velocity = new Vector2(Main.speed * dir, 0);

        //CalculatePercentage(Vector2.SqrMagnitude(this.transform.position - chest.transform.position));
        //  if (calculate_percentage) StartCoroutine(Percentage());
          if (increase_speed) StartCoroutine(IncreaseSpeed());

        pressureText.text = "                 " + (int)(rb.velocity.y * 1) + " atm";
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
        if (collision.gameObject.tag == "Mine" || collision.gameObject.tag == "Patrol" || collision.gameObject.tag == "Cave")
        {

            if (!indestructible)
            {

                if (damaged == false)
                {
                    if (collision.gameObject.tag == "Cave")
                    {
                        rock_sound.Play();
                    }

                    anim.Play("IdleDamagedPlayer");
                    damaged = true;
                    StartCoroutine(Main.Instance.DBController.DeleteUserLevelAttempt(user, level));

                }
                else
                {
                    anim.Play("PlayerExplosion");
                    StartCoroutine(Endgame());
                    SendStats();
                }
            }
        }

        else if (collision.gameObject.tag == "Chest")
        {
            dir = -1;
            //cameraMovement.speed = speed;
            this.transform.Rotate(0f, 180f, 0f, Space.Self);
            if (!damaged) StartCoroutine(Main.Instance.DBController.DeleteUserLevelAttempt(user, level));
        }

        else if (collision.gameObject.tag == "Finish")
        {
            EndText1.text = "YOU WON!";
            EndText2.text = "100% COMPLETED";
            percentage = 1f;
            end_menu.SetActive(true);
            Time.timeScale = 0;
            SendStats();
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
        if (percentage == 1f && damaged == false) Main.perfectly_completed = 1;

        StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(user, level, attempts.ToString(), average_score.ToString(), Main.max_score.ToString(), Main.perfectly_completed.ToString()));
    }

    void CalculatePercentage(float new_distance)
    {
        if (speed > 0)
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
        yield return new WaitForSeconds(0.6f);
        EndText2.text = (int)(percentage * 100) + "% COMPLETED";
        end_menu.SetActive(true);
        Time.timeScale = 0;
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
       // CalculatePercentage(Vector2.Distance(this.transform.position, chest.transform.position));
        yield return new WaitForSeconds(0.2f);
        calculate_percentage = true;
    }

    IEnumerator IncreaseSpeed()
    {
        increase_speed = false;
        // CalculatePercentage(Vector2.Distance(this.transform.position, chest.transform.position));
        Main.speed = Main.speed * 1.02f;
        speed = Main.speed;
        yield return new WaitForSeconds(0.3f);
        increase_speed = true;
    }
}