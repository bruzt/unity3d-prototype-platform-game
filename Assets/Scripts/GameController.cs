using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private static int maxHitpoints = 3;

    private static int coins = 0;
    private static int hitpoints;
    private static int lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        hitpoints = maxHitpoints;
    }

    // Update is called once per frame
    void Update()
    {
        CalcCoins();
    }

    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////

    private void CalcCoins(){
        if(coins >= 100) {
            coins -= 100;
            
            if(hitpoints >= 3) lives++;
            else hitpoints++;
        }
    }

    public static void SetLayerRecursively(GameObject obj, int newLayer){

        if (obj == null) return;
    
        obj.layer = newLayer;
       
        foreach (Transform child in obj.transform){
            
            if (child == null) continue;
            
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    ///////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////

    public static int GetCoins(){
        return coins;
    }

    public static void AddCoin(int coin){
        coins += coin;
    }

    public static int GetHitpoints(){
        return hitpoints;
    }

    public static void SubtractHitpoints(int value){
        hitpoints -= value;
    }

    public static int GetLives(){
        return lives;
    }

    public static void SubtractLives(int value){
        lives -= value;
    }
}
