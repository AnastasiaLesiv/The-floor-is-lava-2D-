using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressPickup : MonoBehaviour
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
