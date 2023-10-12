using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

public class PatrolController : MonoBehaviour
{

    public AudioSource jellyfish_sound;
    public float y;
    float speed = 2f;
    float range = 1f;
    float yStart;
    // Start is called before the first frame update
    void Start()
    {
        //haig de fer getcomponent animator
        yStart = this.transform.position.y;
    }

    public void Update()
    {
        loop();
    }

    void loop()
    {
        float yPos = Mathf.PingPong(Time.time * speed, 5) * range;
        transform.position = new Vector3(transform.position.x, yPos + yStart, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Has xocat!3");

        if (other.gameObject.tag == "Player")
        {
            GetComponent<CircleCollider2D>().enabled = false;
            CinemachineShake.Instance.Shake(1f, 1f);
            jellyfish_sound.Play();

        }
    }

}
