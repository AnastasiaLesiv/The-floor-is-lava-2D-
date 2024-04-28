using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 2f;
    public float frequency = 1f;
    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude ;
        transform.position = startPosition + new Vector3(0f, yOffset, 0f);
    }
    
}
