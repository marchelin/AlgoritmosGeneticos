using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public Transform Target;
    public float DistanceToTarget;

    public delegate void SetResult(float result);
    public event SetResult OnHitCollider;

    public void OnCollisionEnter(Collision col)
    {
        Debug.DrawRay(transform.position, Target.transform.position - transform.position, Color.red, 100f);
        DistanceToTarget = Vector3.Distance(transform.position, Target.transform.position);
        OnHitCollider(DistanceToTarget);

        Destroy(gameObject);
    }

    void Update()
    {
        //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}