using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Text coins;
    private Text hitpoints;
    private Text lives;

    // Start is called before the first frame update
    void Start()
    {
        coins = transform.Find("Coins").GetComponent<Text>();
        hitpoints = transform.Find("Hitpoints").GetComponent<Text>();
        lives = transform.Find("Lives").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowCoins();
        ShowHitpoints();
        ShowLives();
    }

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    void ShowCoins(){
        coins.text = "Coins: " + GameController.GetCoins();
    }

    void ShowHitpoints(){
        hitpoints.text = "Hitpoints: " + GameController.GetHitpoints();
    }

    void ShowLives(){
        lives.text = "Lives: " + GameController.GetLives();
    }
}
