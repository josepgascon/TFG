using UnityEngine;
public class MineController : MonoBehaviour
{
    public bool Explosion;
    private Animator anim;
    void Start()
    {
        Explosion = false;
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Explosion = true;
            anim.Play("ExplosionAnim");
            GetComponent<CircleCollider2D>().enabled = false;
            CinemachineShake.Instance.Shake(5f, 1f);
        }
    }

}
