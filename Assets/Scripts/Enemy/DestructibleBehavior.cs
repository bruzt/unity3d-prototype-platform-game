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
    [SerializeField] private int ForceUpOnHit = 10000;
    [SerializeField] private bool shrinkYWhenDestroyed = false;
    [SerializeField] private float shrinkYFactor = 0.5f;
    [SerializeField] private GameObject spawnAfterDestroyed;

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
        Rigidbody otherRigidbody = other.GetComponentInParent<Rigidbody>();
        
        foreach(string colliderName in takesDamageOf){
            if(other.name.Contains(colliderName) && currentHitPoints > 0) {
                if(colliderName.Contains("Foot")) {
                    otherRigidbody.velocity = new Vector3(otherRigidbody.velocity.x, 0, 0);
                    otherRigidbody.AddForce(0, ForceUpOnHit,0);
                }

                //if(other.name.Contains("Player")) player = other.gameObject;
                ApplyDamage(1);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////

    void ApplyDamage(int damage){
        currentHitPoints -= damage;

        if(shrinkYWhenDestroyed) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * shrinkYFactor, transform.localScale.z);

        if(currentHitPoints <= 0) {
            Destroy();

            if(spawnAfterDestroyed != null){
                spawnedObject = Instantiate(spawnAfterDestroyed, transform.position, transform.rotation);

                if(spawnedObject.TryGetComponent(out FlyTowardsTag flyTowardsTag)){
                    flyTowardsTag.SetTag("Player");
                    flyTowardsTag.SetFlySpeed(8);
                    flyTowardsTag.SetShouldFlyTowardsTag(true);
                }
            }
        }
    }

    void Destroy(){
        GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezePosition;
        GetComponentInChildren<Collider>().enabled = false;
        Destroy(gameObject, timeToDestroy);
    }
}
