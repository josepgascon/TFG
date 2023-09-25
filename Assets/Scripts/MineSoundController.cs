using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSoundController : MonoBehaviour
{
    public AudioSource explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ExplosionSound()
    {
        explosion.Play();
    }

}
