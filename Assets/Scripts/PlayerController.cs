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



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
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

        if(damaged == false)
        {
            anim.Play("IdleDamagedPlayer");
            damaged = true;
        }
        else
        {
            anim.Play("PlayerExplosion");
            //Destroy(this.gameObject);
        }


        if (collision.gameObject.tag == "Mine")
        {
            Debug.Log("Has xocat!2");

            //Destroy(collision.gameObject);
            //cam.Screenshake(10, 2);
        }
    }
}
