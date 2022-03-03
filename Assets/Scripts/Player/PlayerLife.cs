using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Renderer[] playerModelRenderers;
    private Collider[] playerColliders;
    [SerializeField] private int currentHitPoints;
    private float currentTimeInvencible = 0;
    private bool isBlinking = false;
    private bool isAlive = true;

    public int totalHitPoints = 3;
    public float timeInvencible = 1;
    public float blinkIntervalTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerModelRenderers = transform.Find("Model").GetComponentsInChildren<Renderer>();
        playerColliders = GetComponentsInChildren<Collider>();
        currentHitPoints = totalHitPoints;
        currentTimeInvencible = timeInvencible;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeInvencible += Time.deltaTime;

        if(GetIsInvencible()) {
            if(isBlinking == false) StartCoroutine(BlinkDamagedInvencibility());   
        } else {
            SetPlayerModelVisible(true);
            isBlinking = false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    public void ApplyDamage(){
        if(GetIsInvencible() == false && playerAttack.GetIsAttacking() == false){
            currentHitPoints--;
            currentTimeInvencible = 0;

            if(currentHitPoints < 1) {
                SetIsAlive(false); 
                playerRigidbody.velocity = Vector3.zero;
            }
            else playerMovement.JumpUp();
        }
    }

    IEnumerator BlinkDamagedInvencibility(){

        isBlinking = true;

        SetPlayerCollidersEnabled(false);

        while(GetIsInvencible()){
            SetPlayerModelVisible(false);

            yield return new WaitForSeconds(blinkIntervalTime);

            SetPlayerModelVisible(true);

            yield return new WaitForSeconds(blinkIntervalTime);
        }

        SetPlayerCollidersEnabled(true);

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

    void SetPlayerCollidersEnabled(bool value){
        foreach(Collider playerCollider in playerColliders){
            if(playerCollider.isTrigger){
                playerCollider.enabled = value;
            }
        }
    }
}
