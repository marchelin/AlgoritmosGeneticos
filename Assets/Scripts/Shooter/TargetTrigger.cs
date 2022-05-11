using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public Transform Target;

    private bool _toDestroy = false;

    public static float currentDistanceToTarget;
    //public static float lastDistanceToTarget;

    public delegate void SetResult(float result);

    public event SetResult OnHitCollider;

    public static bool obstacleCollision;

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "obstacle")
        {
            obstacleCollision = true;

            Debug.DrawRay(transform.position, Target.transform.position - transform.position, Color.red, 10f);
            currentDistanceToTarget = Vector3.Distance(transform.position, Target.transform.position);
            OnHitCollider(currentDistanceToTarget);
            _toDestroy = true;
        }
        else
        {
            obstacleCollision = false;

            Debug.DrawRay(transform.position, Target.transform.position - transform.position, Color.yellow, 10f);
            currentDistanceToTarget = Vector3.Distance(transform.position, Target.transform.position);
            OnHitCollider(currentDistanceToTarget);
            _toDestroy = true;

            //lastDistanceToTarget = currentDistanceToTarget;
        }
    }

    public void Update()
    {
        if (_toDestroy) DestroyImmediate(this.gameObject);
    }
}
