using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float changeColorTime = 0.5f;
    [SerializeField] private Color changedColor; //цвет после прохождения игрока через объект

    private void OnCollisionEnter2D(Collision2D collision) //колизия для определения сталкновения игрока с препятствием
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerAnchor playerAnchor = collision.gameObject.GetComponentInParent<PlayerAnchor>(); //нет смысла кэшировать данные так как игрок сталкнётся с препятствием только 1 раз
            playerAnchor.Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //трегер для определения прохождения игроком препятствия
    {
        if (collision.gameObject.CompareTag("PlayerAnchor"))
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); //получение SpriteRenderer препятствия для изменения его цвета
            StartCoroutine(SmoothColor(spriteRenderer, changedColor, changeColorTime));
        }
    }

    private IEnumerator SmoothColor(SpriteRenderer renderer, Color endColor, float time)
    {
        Color startColor = renderer.color;
        float currentTime = 0.0f;

        do
        {
            renderer.color = Color.Lerp(startColor, endColor, currentTime / time);
            currentTime += Time.deltaTime;

            yield return null;

        } while (currentTime <= time);
    }
}
