using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Text coins;

    // Start is called before the first frame update
    void Start()
    {
        coins = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowCoins();
    }

    void ShowCoins(){
        coins.text = GameController.coins.ToString();
    }
}
