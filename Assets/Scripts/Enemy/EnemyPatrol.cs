using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : EnemyBehavior
{
    private EnemyMovement enemyMovement;
    private int targetIndex = 0;
    private Vector3 initPosition;
    private float currentRestTime;

    [SerializeField] private List<Vector3> patrolPositions;
    [SerializeField] private float distanceChangePosition = 0;
    [SerializeField] private float restTime = 0;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        enemyMovement = GetComponent<EnemyMovement>();
        initPosition = transform.position;
        currentRestTime = restTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentRestTime += Time.deltaTime;
        Patrol();
    }

    void OnDrawGizmos()
    {
        foreach(Vector3 position in patrolPositions){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + position, 0.25f);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////

    private void Patrol(){
        Vector3 newPosition = patrolPositions[targetIndex];

        if(Vector3.Distance(newPosition, transform.position - initPosition) < distanceChangePosition){
            currentRestTime = 0;

            if(targetIndex == patrolPositions.Count - 1){
                targetIndex = 0;
            } else {
                targetIndex++;
            }
        }

        if(currentRestTime > restTime) {
            if(newPosition.x > 0){
                enemyMovement.Move(1);
            } else {
                enemyMovement.Move(-1);
            }
        }
    }
}
