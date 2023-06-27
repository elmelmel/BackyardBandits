using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class FieldOfViewDown : MonoBehaviour
{public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;
    [SerializeField] private float delay = 0.2f;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(delay);
        
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
    
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Calculate the angle between the forward direction and the direction to the target,
            // taking into account the rotation of the object
            Vector3 forward = transform.forward;
            Vector3 forwardNoY = new Vector3(forward.x, 0f, forward.z).normalized;
            Vector3 directionToTargetNoY = new Vector3(directionToTarget.x, 0f, directionToTarget.z).normalized;
            float angleToTarget = Vector3.Angle(forwardNoY, directionToTargetNoY);

            if (angleToTarget < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
            
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;

        if (canSeePlayer)
        {
            GameEventManager.Instance.Raise(new GameOver(playerRef));
        }
    }
}
