using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FishController : MonoBehaviour
{
    public AudioSource fish_sound;
    public float moveSpeed = 2f; 
    public float patrolDistance = 5f; 

    private bool moveRight = true;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Move on the patrol direction
        if (moveRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }

        // Check if has reached the patrol distance
        if (Mathf.Abs(transform.position.x - initialPosition.x) >= patrolDistance)
        {
            moveRight = !moveRight;
            FlipSprite();
        }
    }

    private void FlipSprite()
    {
        // Flip the enemy sprite by reversing its scale on the X-axis
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<CircleCollider2D>().enabled = false;
            CinemachineShake.Instance.Shake(2f, 1.5f);
            fish_sound.Play();
        }
    }
}
