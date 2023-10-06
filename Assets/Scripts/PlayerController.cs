using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public Slider _slider;
    public float speed;
    //public float height;
    private Vector2 dir;
    private Transform tr;
    private bool damaged; 
    private Animator anim;
    public bool indestructible;
    public CameraMove cameraMovement;
    private Vector2 velocity;
    private Rigidbody2D rbrb;




    private void Awake()
    {
        rbrb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        //cameraMovement = GetComponent<CameraMove>();
        //dir.x = 1;
        damaged = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        //_slider.onValueChanged.AddListener(UpdateValue);

        _slider.value = 0f;
    }


    //update
    void Update()
    {
        //Debug.Log("_slider.value = " + _slider.value);
        velocity = new Vector2(3f, (_slider.value * -5f));
        rbrb.MovePosition(rbrb.position + velocity * Time.fixedDeltaTime);
    }



  /*  // Update is called once per frame
    void FixedUpdate()
    {
        velocity = new Vector2(dir.x * speed, rb.velocity.y);
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }  */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Has xocat1");

        if (collision.gameObject.tag == "Mine" || collision.gameObject.tag == "Patrol" || collision.gameObject.tag == "Cave")
        {

            if (indestructible)
            {
                //this.transform.Rotate(0f, 180f, 0f, Space.Self);
                //anim.Play("IdleReverse");
            }
            Debug.Log("Has xocat2");


            if (!indestructible)
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
                    //Destroy(this.gameObject);
                }
            }
        }

     /*   else if (collision.gameObject.tag == "Patrol")
        {
            if (!indestructible)
            {

                if (damaged == false)
                {
                    //this.transform. //(0f, 180f, 0f, Space.Self);
                    anim.Play("IdleDamagedPlayer");
                    damaged = true;
                }
                else
                {
                    anim.Play("PlayerExplosion");
                    //Destroy(this.gameObject);
                }
            }
        }   */

        else if (collision.gameObject.tag == "Chest")
        {
            speed = -speed;
            cameraMovement.speed = speed;
            this.transform.Rotate(0f, 180f, 0f, Space.Self);
        }
    }
    }
