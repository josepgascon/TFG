using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class MineController : MonoBehaviour
{

    public bool Explosion;
    private Animator anim;
    public CinemachineVirtualCameraBase cam;
    // Start is called before the first frame update
    void Start()
    {
        //haig de fer getcomponent animator
        Explosion = false;
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Has xocat!3");

        if (other.gameObject.tag == "Player"){
            Debug.Log("Has xocat!5");

            Explosion = true;
            anim.Play("ExplosionAnim");
            GetComponent<CircleCollider2D>().enabled = false;
            CinemachineShake.Instance.Shake(5f, 1f);
            //Destroy(this.gameObject);
        };
    }

}
