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
    private PlayerMovement playerMovement;
    private TrailRenderer trailRenderer;
    private Collider playerDashCollider;
    private Collider playerHeadCollider;
    private Collider playerFootCollider;
    private float currentAttackRate;
    private bool isDashing = false;
    private string activeAttack = "Dash";

    [SerializeField] private float attackRate = 1;
    [SerializeField] private float attackDuration = 0.1f;
    [SerializeField] private float dashForce = 30000;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerLife = GetComponent<PlayerLife>();
        playerMovement = GetComponent<PlayerMovement>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        playerDashCollider = transform.Find("PlayerDashCollider").GetComponent<Collider>();
        playerHeadCollider = transform.Find("PlayerHeadCollider").GetComponent<Collider>();
        playerFootCollider = transform.Find("PlayerFootCollider").GetComponent<Collider>();
        currentAttackRate = attackRate;
        trailRenderer.time = attackDuration;
        trailRenderer.widthMultiplier = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        currentAttackRate += Time.deltaTime;
    }

    ///////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////

    public void Attack(float direction){
        if(
            currentAttackRate > attackRate && 
            activeAttack != null && 
            playerLife.GetIsDamageFlashing() == false
        ){
            currentAttackRate = 0;

            if(activeAttack == AttackTypes.Dash.ToString()) StartCoroutine(DashAttackCoroutine());
        }
    }

    private IEnumerator DashAttackCoroutine(){
        isDashing = true;
        SetDashAttackColliders(true);
        trailRenderer.emitting = true;

        float direction = (playerMovement.GetIsLookingRight()) ? 1 : -1;
        playerRigidbody.AddForce(direction * dashForce, 0, 0);

        yield return new WaitForSeconds(attackDuration);
        
        trailRenderer.emitting = false;
        SetDashAttackColliders(false);
        isDashing = false;

        yield return null;
    }

    ///////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////

    public bool GetIsDashing(){
        return isDashing;
    }

    public void SetPlayerDashCollider(bool value){
        playerDashCollider.enabled = value;
    }

    public void SetPlayerHeadCollider(bool value){
        playerHeadCollider.enabled = value;
    }

    public void SetPlayerFootCollider(bool value){
        playerFootCollider.enabled = value;
    }

    void SetDashAttackColliders(bool value){
        SetPlayerDashCollider(value);
        SetPlayerHeadCollider(!value);
        SetPlayerFootCollider(!value);
        playerLife.SetPlayerBodyCollider(!value);
    }

    public void SetActiveAttack(AttackTypes attack){
        activeAttack = attack.ToString();
    }
}
