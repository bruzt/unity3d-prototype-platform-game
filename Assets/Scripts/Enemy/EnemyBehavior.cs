using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        if(collider.name.Contains("PlayerBody")) EnterPlayer(collider);
    }

    ///////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////

    void EnterPlayer(Collider playerCollider){
        PlayerLife playerLife = playerCollider.GetComponentInParent<PlayerLife>();
    
        playerLife.ApplyDamage();
    }
}
