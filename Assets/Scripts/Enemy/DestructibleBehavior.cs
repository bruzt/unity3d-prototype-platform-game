using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehavior : MonoBehaviour
{
    private int currentHitPoints;
    private GameObject spawnedObject;

    [SerializeField] private List<string> takesDamageOf;
    [SerializeField] private int totalHitPoints = 1;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private int ForceOnHit = 10000;
    [SerializeField] private bool shrinkYWhenDestroyed = false;
    [SerializeField] private float shrinkYFactor = 0.5f;
    [SerializeField] private GameObject spawnAfterDestroyed;
    [SerializeField] private int quantityToSpawn = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentHitPoints = totalHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        
        foreach(string colliderName in takesDamageOf){

            if(other.name.Contains(colliderName) && currentHitPoints > 0) {

                PlayerInteraction playerInteraction = other.GetComponentInParent<PlayerInteraction>();
                
                if(colliderName.Contains("Foot")) {
                    if(playerInteraction.GetIsInGround() == false){
                        ApplyForce(other, ForceOnHit);
                        ApplyDamage(1);
                    }  
                } else if(colliderName.Contains("Head")) {
                    if(playerInteraction.GetIsInGround() == false){
                        ApplyForce(other, -ForceOnHit);
                        ApplyDamage(1);
                    } 
                } else {
                    ApplyDamage(1);
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////

    void ApplyDamage(int damage){
        currentHitPoints -= damage;

        if(currentHitPoints <= 0) {
            if(shrinkYWhenDestroyed) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * shrinkYFactor, transform.localScale.z);

            if(spawnAfterDestroyed != null) StartCoroutine(SpawnAfterDestroyedCoroutine());
            else DestroyThis();
        }
    }

    void DestroyThis(){
        Destroy(gameObject, timeToDestroy);
    }

    IEnumerator SpawnAfterDestroyedCoroutine(){

        StartCoroutine(DisableRendererCoroutine());

        for(int i=0; i < quantityToSpawn; i++){
            spawnedObject = Instantiate(spawnAfterDestroyed, transform.position, transform.rotation);
            FlyTowardsName flyTowardsName = spawnedObject.AddComponent<FlyTowardsName>();
                
            flyTowardsName.SetNameToFlyTowards("Player");
            
            int speed = (quantityToSpawn * 3 > 15) ? quantityToSpawn * 3 : 15;
            flyTowardsName.SetFlySpeed(speed);

            yield return new WaitForSeconds(0.1f);

            flyTowardsName.SetShouldFlyTowardsName(true);
        }
        
        DestroyThis();

        yield return null;
    }

    IEnumerator DisableRendererCoroutine(){
        GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezePosition;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach(Collider collider in colliders){
            collider.enabled = false;
        }
        
        yield return new WaitForSeconds(timeToDestroy);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers){
            renderer.enabled = false;
        }

        yield return null;
    }

    void ApplyForce(Collider other, int force){
        Rigidbody otherRigidbody = other.GetComponentInParent<Rigidbody>();
        if(force > 0) otherRigidbody.velocity = new Vector3(otherRigidbody.velocity.x, 0, 0);
        otherRigidbody.AddForce(0, force,0);
    }
}
