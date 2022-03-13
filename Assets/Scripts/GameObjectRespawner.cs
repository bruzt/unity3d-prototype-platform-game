using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRespawner : MonoBehaviour
{
    private GameObject gameObjectCopy;

    [SerializeField] private GameObject gameObjectToRespawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObjectCopy == null) {
            gameObjectCopy = Instantiate(
                gameObjectToRespawn, 
                gameObjectToRespawn.transform.position, 
                gameObjectToRespawn.transform.rotation
            );
            gameObjectCopy.SetActive(false);
        }
    }

    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////

    void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Player")){
            try {
                if(gameObjectToRespawn.activeInHierarchy) return;
            } catch {
                gameObjectToRespawn = gameObjectCopy;
                gameObjectCopy = null;
                gameObjectToRespawn.SetActive(true);
            }
        }
    }
}
