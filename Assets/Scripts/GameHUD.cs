using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    public TextMeshProUGUI bulletsCountText;
    public Color noBulletsColor;
    public Color hasBulletsColor;

    public static GameHUD Instance;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateHUD(Player player)
    {
        bulletsCountText.text = player.bulletsCount.ToString();

        if (player.bulletsCount > 0)
        {
            bulletsCountText.color = hasBulletsColor;
        } else {
            bulletsCountText.color = noBulletsColor;
        }
    }
}