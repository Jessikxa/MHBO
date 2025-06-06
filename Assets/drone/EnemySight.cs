using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public float viewRadius = 40f;
    [Range(0, 360)] public float viewAngle = 90f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector] public Transform CurrentTarget;
    [HideInInspector] public bool HasTarget = false;

    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.5f));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            DetectTarget();
        }
    }

    void DetectTarget()
    {
        HasTarget = false;
        CurrentTarget = null;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(Vector3.up, dirToTarget); // looking upward cone

            if (angleToTarget < viewAngle / 2f)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
                {
                    CurrentTarget = target.transform;
                    HasTarget = true;

                    Debug.Log($"{gameObject.name} spotted {target.name}");

                    break;
                }
            }
        }
    }

    // Optional: for visualizing the view radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
