using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed;
    [SerializeField]
    public Rigidbody2D rb;
    private Vector2 dir;

    void Start()
    {
        dir.x = 1;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }
}
