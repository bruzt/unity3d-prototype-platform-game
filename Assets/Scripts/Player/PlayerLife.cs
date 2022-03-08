using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Renderer[] playerModelRenderers;
    private Collider playerBodyCollider;
    private int currentHitPoints;
    private float currentTimeInvencible = 0;
    private bool isBlinking = false;
    private bool isAlive = true;

    [SerializeField] private int totalHitPoints = 3;
    [SerializeField] private float timeInvencible = 1;
    [SerializeField] private float blinkIntervalTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerModelRenderers = transform.Find("Model").GetComponentsInChildren<Renderer>();
        playerBodyCollider = transform.Find("PlayerBodyCollider").GetComponent<Collider>();
        currentHitPoints = totalHitPoints;
        currentTimeInvencible = timeInvencible;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeInvencible += Time.deltaTime;
    }

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    public void ApplyDamage(){
        if(GetIsInvencible() == false && playerAttack.GetIsAttacking() == false){
            SetPlayerColliders(false);

            currentHitPoints--;
            currentTimeInvencible = 0;

            StartCoroutine(DamageBlinkCoroutine());

            if(currentHitPoints < 1) {
                SetIsAlive(false); 
                playerRigidbody.velocity = Vector3.zero;
            }
            else playerMovement.JumpUp();
        }
    }

    private IEnumerator DamageBlinkCoroutine(){

        isBlinking = true;

        while(GetIsInvencible()){
            SetPlayerModelVisible(false);

            yield return new WaitForSeconds(blinkIntervalTime);

            SetPlayerModelVisible(true);

            yield return new WaitForSeconds(blinkIntervalTime);
        }

        isBlinking = false;
        SetPlayerColliders(true);

        yield return null;
    }

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    public bool GetIsInvencible(){
        return currentTimeInvencible < timeInvencible;
    }

    public bool GetIsAlive(){
        return isAlive;
    }
    public void SetIsAlive(bool value){
        isAlive = value;
    }

    void SetPlayerModelVisible(bool value){
        foreach(Renderer playerModelRenderer in playerModelRenderers){
            playerModelRenderer.enabled = value;
        }
    }

    public void SetPlayerBodyCollider(bool value){
        playerBodyCollider.enabled = value;
    }

    void SetPlayerColliders(bool value){
        SetPlayerBodyCollider(value);
        playerAttack.SetPlayerFootCollider(value);
    }
}
