using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isTriggered = false;

    private static PlayerAnchor playerAnchor; //кэширование данных для всех экземпляров данного класса (игрок долден быть 1)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;

            playerAnchor = playerAnchor == null ? collision.gameObject.GetComponentInParent<PlayerAnchor>() : playerAnchor;
            playerAnchor.AddCoin();

            Destroy(gameObject);
        }
    }
}
