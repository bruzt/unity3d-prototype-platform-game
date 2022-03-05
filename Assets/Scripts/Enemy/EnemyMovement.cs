using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody enemyRigidbody;

    [SerializeField] private int moveSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////

    public void Move(float xDirection){
        enemyRigidbody.velocity = new Vector3(xDirection * moveSpeed, 0, 0);
        RotateEnemyModel(xDirection);
    }

    private void RotateEnemyModel(float xDirection){
        if(xDirection > 0){
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if(xDirection < 0) {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }
}
