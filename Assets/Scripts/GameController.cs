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
    public CameraMove cameraMovement;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damaged = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1f;
        total_distance = Vector2.SqrMagnitude(this.transform.position - chest.transform.position);
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
        rb.velocity = new Vector2(speed, rb.velocity.y);
        rb.gravityScale = _slider.value;
        Vector3 x = this.transform.position;
        cameraMovement.transform.position = new Vector3(x.x + 2f, 0f, 0f);
        CalculatePercentage(Vector2.SqrMagnitude(this.transform.position - chest.transform.position));
        pressureText.text = "                 " + (int)(rb.velocity.y * 1) + " atm";
        percentageSlider.value = percentage;
        if (sound_radar)StartCoroutine(Wait5Second());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mine" || collision.gameObject.tag == "Patrol" || collision.gameObject.tag == "Cave")
        {
            if (collision.gameObject.tag == "Cave")
            {
                rock_sound.Play();
            }

            if (!indestructible)
            {
                if (damaged == false)
                {
                    anim.Play("IdleDamagedPlayer");
                    damaged = true;
                    StartCoroutine(Main.Instance.DBController.DeleteUserLevelAttempt(user, level));
                }
                else
                {
                    anim.Play("PlayerExplosion");
                    StartCoroutine(Wait1Second());
                    SendStats();
                }
            }
        }

        else if (collision.gameObject.tag == "Chest")
        {
            speed = -speed;
            cameraMovement.speed = speed;
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

    IEnumerator Wait1Second()
    {
        yield return new WaitForSeconds(0.6f);
        EndText2.text = (int)(percentage * 100) + "% COMPLETED";
        end_menu.SetActive(true);
        Time.timeScale = 0;
    }

    IEnumerator Wait5Second()
    {
        sound_radar = false;
        radar_sound.Play();
        yield return new WaitForSeconds(5f);
        sound_radar = true;
    }

    IEnumerator WaitLessThanASecond()
    {
        calculate_percentage = false;
        CalculatePercentage(Vector2.Distance(this.transform.position, chest.transform.position));
        yield return new WaitForSeconds(0.2f);
        calculate_percentage = true;
    }
}
