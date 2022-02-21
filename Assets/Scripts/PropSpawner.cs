using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int amount;
    public List<Vector3> positions; 

    // Start is called before the first frame update
    void Start()
    {
        InstantiatePrefabs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        foreach(Vector3 position in positions){
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + position, 0.25f);
        }
    }

    void InstantiatePrefabs(){
        for(int i=0; i < amount; i++){
            Vector3 position = transform.position;

            float percent = (float)i/(float)amount;
            
            position = Vector3.Lerp(position + positions[0], position + positions[positions.Count-1], percent);

            Instantiate(prefab, position, transform.rotation);
        }
    }
}
