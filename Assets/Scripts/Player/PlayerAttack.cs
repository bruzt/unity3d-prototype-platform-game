using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*static class AttackTypes {
    public const string Dash = "Dash";
}*/

public enum AttackTypes {
    Dash,
}

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private PlayerLife playerLife;
    private TrailRenderer trailRenderer;
    private Collider playerDashCollider;
    private Collider playerFootCollider;
    private float currentAttackRate;
    private float currentAttackDuration;
    private string activeAttack;

    [SerializeField] private float attackRate = 1;
    [SerializeField] private float attackDuration = 0.1f;
    [SerializeField] private float dashForce = 30000;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerLife = GetComponent<PlayerLife>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        playerDashCollider = transform.Find("PlayerDashCollider").GetComponent<Collider>();
        playerFootCollider = transform.Find("PlayerFootCollider").GetComponent<Collider>();
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
            SetPlayerAttackColliders(true);
        }
        else {
            trailRenderer.emitting = false;
            SetPlayerAttackColliders(false);
        }
    }

    ///////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////

    public void Attack(float direction){
        if(currentAttackRate > attackRate && direction != 0){
            if(activeAttack == AttackTypes.Dash.ToString()) DashAttack(direction);
        }

        //print("attack " + direction);
    }

    private void DashAttack(float direction){
        currentAttackRate = 0;
        currentAttackDuration = 0;

        direction = (direction > 0) ? 1 : -1;
        playerRigidbody.AddForce(direction * dashForce, 0, 0);
    }

    ///////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////

    public bool GetIsAttacking(){
        return currentAttackDuration < attackDuration;
    }

    public void SetPlayerDashCollider(bool value){
        playerDashCollider.enabled = value;
    }

    public void SetPlayerFootCollider(bool value){
        playerFootCollider.enabled = value;
    }

    void SetPlayerAttackColliders(bool value){
        SetPlayerDashCollider(value);
        SetPlayerFootCollider(!value);
        playerLife.SetPlayerBodyCollider(!value);
    }

    public void SetActiveAttack(AttackTypes attack){
        activeAttack = attack.ToString();
    }
}
