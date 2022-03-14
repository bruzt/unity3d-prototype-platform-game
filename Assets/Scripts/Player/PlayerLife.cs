using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Renderer[] playerModelRenderers;
    private Collider playerBodyCollider;
    private int currentHitPoints;
    private float currentDamageFlashTime = 0;
    private bool isAlive = true;

    [SerializeField] private int totalHitPoints = 3;
    [SerializeField] private float damageFlashTime = 1;
    [SerializeField] private float damageFlashIntervalTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerModelRenderers = transform.Find("Model").GetComponentsInChildren<Renderer>();
        playerBodyCollider = transform.Find("PlayerBodyCollider").GetComponent<Collider>();
        currentHitPoints = totalHitPoints;
        currentDamageFlashTime = damageFlashTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentDamageFlashTime += Time.deltaTime;
    }

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    public void ApplyDamage(){
        if(GetIsDamageFlashing() == false){
            currentHitPoints--;
            currentDamageFlashTime = 0;

            StartCoroutine(DamageFlashAnimationCoroutine());

            if(currentHitPoints < 1) {
                SetIsAlive(false); 
                playerRigidbody.velocity = Vector3.zero;
                StartCoroutine(GoToGameOverScene());
            }
            else playerMovement.JumpUp();
        }
    }

    private IEnumerator DamageFlashAnimationCoroutine(){

        SetPlayerColliders(false);

        while(GetIsDamageFlashing()){
            SetPlayerModelVisible(false);

            yield return new WaitForSeconds(damageFlashIntervalTime);

            SetPlayerModelVisible(true);

            yield return new WaitForSeconds(damageFlashIntervalTime);
        }

        SetPlayerColliders(true);

        yield return null;
    }

    private IEnumerator GoToGameOverScene(){

        yield return new WaitForSeconds(1);

        GameOver.LoadScene();

        yield return null;
    }

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    public bool GetIsDamageFlashing(){
        return currentDamageFlashTime <= damageFlashTime;
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
