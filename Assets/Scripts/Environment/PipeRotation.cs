using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeRotation : MonoBehaviour
{
    [SerializeField] private Transform startPoint, controlPoint, endPoint;

    private static PlayerAnchor playerAnchor; //кэширование данных для всех экземпляров данного класса (игрок долден быть 1)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAnchor"))
        {
            playerAnchor = playerAnchor == null ? collision.gameObject.GetComponentInParent<PlayerAnchor>() : playerAnchor;
            playerAnchor.StartMoveCurve(startPoint, controlPoint, endPoint);
        }
    }

    //collision.GetComponent<Player>().ResetPointsForBezier();
}
