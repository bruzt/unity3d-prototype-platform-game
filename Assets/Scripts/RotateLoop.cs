using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLoop : MonoBehaviour
{
    private Vector3 axis;

    [SerializeField] bool rotateX = false;
    [SerializeField] bool rotateY = true;
    [SerializeField] bool rotateZ = false;
    [SerializeField, Range(0, 1000)] private float speed = 500;

    // Start is called before the first frame update
    void Start()
    {
        axis.x = (rotateX) ? 5 : 0;
        axis.y = (rotateY) ? 5 : 0;
        axis.z = (rotateZ) ? 5 : 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newAlgle = new Vector3(transform.eulerAngles.x + axis.x, transform.eulerAngles.y + axis.y, transform.eulerAngles.z + axis.z);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, newAlgle, speed * Time.deltaTime);
    }
}
