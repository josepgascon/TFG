using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{

    public bool Explosion;
    // Start is called before the first frame update
    void Start()
    {
        //haig de fer getcomponent animator
        Explosion = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Has xocat!3");

        if (other.gameObject.tag == "Player"){
            Debug.Log("Has xocat!4");

            Explosion = true;
            Destroy(this.gameObject);
        };
    }

}
