using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D rb;
    public float speed;
    public float height;
    private Vector2 dir;
    public Transform tr;
    private bool damaged; 
    private Animator anim;
    public bool indestructible;
    public CameraMove cameraMovement;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        //cameraMovement = GetComponent<CameraMove>();
        dir.x = 1;
        damaged = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Has xocat1");

        if (collision.gameObject.tag == "Mine" || collision.gameObject.tag == "Patrol")
        {

            if (indestructible)
            {
                //this.transform.Rotate(0f, 180f, 0f, Space.Self);
                //anim.Play("IdleReverse");
            }

            if (!indestructible)
            {

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

        else if (collision.gameObject.tag == "Patrol")
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
        }

        if (collision.gameObject.tag == "Chest")
        {
            speed = -speed;
            cameraMovement.speed = speed;
            this.transform.Rotate(0f, 180f, 0f, Space.Self);
        }
    }
    }
