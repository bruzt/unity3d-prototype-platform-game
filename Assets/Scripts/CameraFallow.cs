using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{
    private Vector3 initPosition;

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField, Range(1, 20)] private float movementFactor = 1;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target == null) return;

        Vector3 newPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, initPosition.z);

        transform.position = Vector3.Lerp(transform.position, newPosition, movementFactor * Time.deltaTime);
    }
}
