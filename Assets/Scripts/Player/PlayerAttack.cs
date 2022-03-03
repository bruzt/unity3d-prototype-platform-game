using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private TrailRenderer trailRenderer;
    private Collider playerDashCollider;
    private float currentAttackRate;
    private float currentAttackDuration;

    public int damage = 1;
    public float attackRate = 1;
    public float attackDuration = 0.1f;
    public float dashForce = 30000;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        playerDashCollider = transform.Find("PlayerDashCollider").GetComponent<Collider>();
        currentAttackRate = attackRate;
        currentAttackDuration = attackDuration;
        trailRenderer.time = attackDuration;
        trailRenderer.widthMultiplier = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        currentAttackRate += Time.deltaTime;
        currentAttackDuration += Time.deltaTime;

        if(GetIsAttacking()) {
            trailRenderer.emitting = true;
            playerDashCollider.enabled = true;
        }
        else {
            trailRenderer.emitting = false;
            playerDashCollider.enabled = false;
        }
    }

    ///////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////

    public void Attack(float direction){
        if(currentAttackRate > attackRate && direction != 0){
            currentAttackRate = 0;
            currentAttackDuration = 0;

            direction = (direction > 0) ? 1 : -1;
            playerRigidbody.AddForce(direction * dashForce, 0, 0);

            //print("attack " + direction);
        }
    }

    ///////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////

    public bool GetIsAttacking(){
        return currentAttackDuration < attackDuration;
    }
}
