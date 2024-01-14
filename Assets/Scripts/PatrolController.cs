using UnityEngine;

public class PatrolController : MonoBehaviour
{
    public AudioSource jellyfish_sound;
    public float y;
    float speed = 2f;
    float range = 1f;
    float yStart;

    void Start()
    {
        yStart = this.transform.position.y;
    }

    public void Update()
    {
        float yPos = Mathf.PingPong(Time.time * speed, 5) * range;
        transform.position = new Vector3(transform.position.x, yPos + yStart, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<CircleCollider2D>().enabled = false;
            CinemachineShake.Instance.Shake(1f, 1f);
            jellyfish_sound.Play();

        }
    }

}
