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

        int sectionAmount =  (int)System.Math.Round((float)amount/((float)positions.Count-1));

        for(int i=0; i < positions.Count; i++){
            for(int j=0; j < sectionAmount; j++){

                Vector3 initPosition = positions[i];
                Vector3 nextPosition;
                
                if(i <= positions.Count - 2) {
                    nextPosition = positions[i+1];

                    Vector3 position = transform.position;

                    float percent = (float)j/(float)sectionAmount;
                    
                    position = Vector3.Lerp(position + initPosition, position + nextPosition, percent);

                    GameObject prop = Instantiate(prefab, position, transform.rotation);

                    prop.transform.SetParent(transform);
                }
            }
        }
    }
}
