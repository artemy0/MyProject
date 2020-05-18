using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMenuUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        PlayerAnchor.OnCoinsUpdated += UpdateCoin;
    }

    private void OnDestroy()
    {
        PlayerAnchor.OnCoinsUpdated -= UpdateCoin;
    }

    private void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
}
