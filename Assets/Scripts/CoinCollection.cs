using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private float coins = 0;
    public TMP_Text coinsText;
    public AudioSource audio;   
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            coins++;
            coinsText.text = coins.ToString();
            AudioSource.PlayClipAtPoint(audio.clip, gameObject.transform.position, audio.volume);
            Destroy(collision.gameObject);
        }
    }
    
}
