using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EndlessGameController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public Slider _slider;
    private Animator anim;
    // public new Camera mainCamera;
    //public SoundController soundController;
    public bool indestructible;
    public ObjectMove cameraMovement;
    public ObjectMove top_border;
    public ObjectMove bottom_border;
    public float speed;
    private bool damaged;
    public TMP_Text pressureText;
    public int gravity = 0;
    public float score = 0.31f;
    public GameObject end_menu;
    public TMP_Text EndText1;
    public TMP_Text EndText2;
    public AudioSource rock_sound;
    public AudioSource radar_sound;
    public GameObject background;
    public GameObject top_background;
    public GameObject bottom_background;
    public GameObject mine;
    private bool create_background;
    private bool spawn_mines;


   // public GameObject groundPrefab; // Reference to the ground prefab that you want to spawn
    public float spawnDistance = 10f; // Distance at which the next ground object will be spawned

    // public GameObject borders;
    private Boolean sound_radar;
    private Boolean calculate_score;
    string user;
    string level;
    int dir;

    private const float accelerationRate = 1.5f; // Adjust the acceleration rate as needed
    private bool isAccelerating;
    private bool isDecelerating;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damaged = false;
        isAccelerating = false;
        isDecelerating = false;
        create_background = true;
        spawn_mines = false;
        dir = 1;
        //borders.rb
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        sound_radar = true;
        calculate_score = true;
        user = Main.currentUser.ToString();
        level = SceneManager.GetActiveScene().buildIndex.ToString();
        Debug.Log("user1 = " + user);
        Debug.Log("level = " + level);
        StartCoroutine(Main.Instance.DBController.GetUserLevelAttempt(user, level));

        Debug.Log("game_controller");

        Debug.Log("audio_enabled = " + Main.AudioEnabled);

     //   cameraMovement.rb.velocity = new Vector2(3f * dir, 0);
       // top_border.rb.velocity = new Vector2(3f * dir, 0);
       // bottom_border.rb.velocity = new Vector2(3f * dir, 0);
    }

    void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(speed * dir, rigidBody.velocity.y);
        rigidBody.gravityScale = _slider.value;
        Vector3 x = this.transform.position;


        // CalculatePercentage(Vector2.SqrMagnitude(this.transform.position - chest.transform.position));
        pressureText.text = "                 " + (int)(rigidBody.velocity.y * 1) + " atm";
        if (sound_radar) StartCoroutine(PlayRadar5Second());
       // if (create_background) StartCoroutine(InstantiateBackground());


        // accelerate
        if (isAccelerating)
        {

            if (this.transform.position.x >= cameraMovement.transform.position.x + 8f) isAccelerating = false;

            speed = 6f;
            Debug.Log("isAccelerating = true perro");

        }
        // decelerate
        else if (isDecelerating)
        {
            if (this.transform.position.x <= cameraMovement.transform.position.x - 8f) isDecelerating = false;
            speed = 1f;
        }
        else speed = 3f;



        // Move the ground object to the left
        transform.Translate(Vector2.left * 3f * Time.deltaTime);

        // Check if we need to spawn a new ground object
        if (transform.position.x < -spawnDistance)
        {
            SpawnGround();
        }


    }

    private void SpawnGround()
    {
        // Spawn a new ground object at the specified distance
        GameObject newGround = Instantiate(bottom_background, new Vector3(spawnDistance * 2, 0, 0), Quaternion.identity);
        // Make the new ground object a child of the current ground object for organization
        newGround.transform.parent = transform;
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
        average_score = (int)((Main.attempts * average_score) + (score * 100)) / attempts;
        float new_score = score * 100;
        if ((int)new_score > Main.max_score) Main.max_score = (int)new_score;
        if (score == 1f && damaged == false) Main.perfectly_completed = 1;

        //StartCoroutine(Main.Instance.DBController.RegisterUserLevelAttempt(user, level, attempts.ToString(), average_score.ToString(), Main.max_score.ToString(), Main.perfectly_completed.ToString()));
    }


    IEnumerator Endgame()
    {
        yield return new WaitForSeconds(0.6f);
        EndText2.text = (int)(score) + " COMPLETED";
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

    IEnumerator CalculateScore()
    {
        calculate_score = false;
        score = this.transform.position.x;
        yield return new WaitForSeconds(0.2f);
        calculate_score = true;
    }

   /* IEnumerator InstantiateBackground()
    {
        create_background = false;
        UnityEngine.Transform top_translation = top_background.transform;
        UnityEngine.Transform bottom_translation = bottom_background.transform;
       /* top_translation.position += new Vector3(50.0f, 6.8f, -8.5f);
        top_translation.RotateAround(top_translation.position, Vector3.left, 180);

        UnityEngine.Transform bottom_translation = cameraMovement.transform;
        top_translation.position += new Vector3(50.0f, -6.88f, -8.5f); 

        Instantiate(background, top_translation);
        Instantiate(background, bottom_translation);
        yield return new WaitForSeconds(5f);
        create_background = true;
    }

    */
   

  /*  IEnumerator SpawnMines()
    {
        spawn_mines = false;
        Instantiate(mine, transform.position + new Vector3(randomX, randomY, 0), transform.rotation);
        yield return new WaitForSeconds(1f);
        spawn_mines = true;
    } */
}