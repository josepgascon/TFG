using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Slider _slider;
    private Animator anim;
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
    private float new_distance;
    public AudioSource rock_sound;
    public AudioSource radar_sound;
    private Boolean sound_radar;

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
        //gravity = rb.gravityScale;
        // percentageText.text = (int)(percentage * 100) + "GRAVITY = " + _slider.value;
        Time.timeScale = 1f;
        percentage = 0.31f;
        pressureText.text = (int)(rb.velocity.y * 100) + "%";
        percentageSlider.value = percentage;
        total_distance = Vector2.SqrMagnitude(this.transform.position - chest.transform.position);
        sound_radar = true;
        //_slider = GetComponent<Slider>();

    }

    // Update is called once per frame
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
        Debug.Log("Has xocat1");

        if (collision.gameObject.tag == "Mine" || collision.gameObject.tag == "Patrol" || collision.gameObject.tag == "Cave")
        {
            Debug.Log("Has xocat2");

            if (collision.gameObject.tag == "Cave")
            {
                rock_sound.Play();
            }

            if (indestructible)
            {
                //this.transform.Rotate(0f, 180f, 0f, Space.Self);
            }


            else if (!indestructible)
            {
                Debug.Log("Has xocat3");


                if (damaged == false)
                {
                    anim.Play("IdleDamagedPlayer");
                    damaged = true;
                }
                else
                {
                    anim.Play("PlayerExplosion");
                    StartCoroutine(Wait1Second()); 
                    //EndText1.text = "YOU WON!";

                }
            }
        }

        else if (collision.gameObject.tag == "Chest")
        {
            speed = -speed;
            cameraMovement.speed = speed;
            this.transform.Rotate(0f, 180f, 0f, Space.Self);
        }

        else if (collision.gameObject.tag == "Finish")
        {
            EndText1.text = "YOU WON!";
            EndText2.text = "100% COMPLETED";
            end_menu.SetActive(true);
            Time.timeScale = 0;
        }

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
}
