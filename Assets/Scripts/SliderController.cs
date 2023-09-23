using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D rb;
    private Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        _slider.onValueChanged.AddListener(UpdateValue);
    }

    private void Update()
    {
        //rb.gravityScale = _slider.value;
    }

    private void UpdateValue(float arg0)
    {
        //Debug.Log("You have updated the value!");
        if (rb != null)
        {
            rb.gravityScale = _slider.value;
        }
    }
}
