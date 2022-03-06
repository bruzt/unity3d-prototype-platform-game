using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    [SerializeField] private int size = 1;
    [SerializeField] private float offsetNode = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Transform node = transform.GetChild(0);

        for(int i=1; i < size; i++){
            Vector3 position = node.transform.position;
            position.y -= offsetNode;

            GameObject newNode = Instantiate(node.gameObject, position, transform.rotation);
            
            newNode.transform.SetParent(transform);

            newNode.GetComponent<FixedJoint>().connectedBody = node.GetComponent<Rigidbody>();

            //Physics.IgnoreCollision(node.GetComponent<Collider>(), newNode.GetComponent<Collider>());
            
            //newNode.GetComponent<Rigidbody>().mass *= 1.01f; 
            //newNode.GetComponent<RopeSegmentController>().DecreaseMaxZRotationDegree(size, i);

            node = newNode.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
