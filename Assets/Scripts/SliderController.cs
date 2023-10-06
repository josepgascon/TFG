using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;


public class SliderController : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D rb;
    private Slider _slider;
    private Vector2 velocity;


    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        _slider.onValueChanged.AddListener(UpdateValue);
        _slider.value = 0f;
    }

    private void Update()
    {
        //rb.gravityScale = _slider.value;
        velocity = new Vector2(3f, (_slider.value * -5f));
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void UpdateValue(float arg0)
    {
        //Debug.Log("You have updated the value!");
        if (rb != null)
        {
           // _slider.value *= 25;
           // rb.gravityScale = _slider.value;
        }
    }
}
