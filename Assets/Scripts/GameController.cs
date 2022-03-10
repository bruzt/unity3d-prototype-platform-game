using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private static int coins = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(coins > 10) {
            LevelSelector.AddAvaliableLevel("Level2");
            WinLevel.LoadScene();
        }*/
    }

    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////

    public static int GetCoins(){
        return coins;
    }

    public static void AddCoin(int coin){
        coins += coin;
    }
}
