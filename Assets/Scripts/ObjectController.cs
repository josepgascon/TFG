using UnityEngine;
public class ObjectController : MonoBehaviour
{
    private Animator anim;
    public AudioSource picked_star;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (this.gameObject.tag == "Chest") anim.Play("OpenedAnim");
            if (this.gameObject.tag == "Mine")
            { 
                anim.Play("ExplosionAnim");
                GetComponent<CircleCollider2D>().enabled = false;
                CinemachineShake.Instance.Shake(5f, 1f);
            }
            if (this.gameObject.tag == "Star")
            {
                picked_star.Play();
                this.gameObject.transform.Translate(new Vector3(0, 100, 0), Space.World);// = new Vector3(0, 100, 0);
            }

            //GetComponent<CircleCollider2D>().enabled = false;
            //CinemachineShake.Instance.Shake(5f, 1f);
        }
    }

}
