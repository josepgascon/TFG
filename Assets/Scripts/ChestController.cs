using UnityEngine;
public class ChestController : MonoBehaviour
{
    public bool Opened;
    private Animator anim;
    void Start()
    {
        Opened = false;
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Opened = true;
            anim.Play("OpenedAnim");
            //GetComponent<CircleCollider2D>().enabled = false;
            //CinemachineShake.Instance.Shake(5f, 1f);
        }
    }

}
