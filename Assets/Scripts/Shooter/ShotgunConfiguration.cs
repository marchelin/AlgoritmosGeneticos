using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShotgunConfiguration : MonoBehaviour
{
    [SerializeField] private GeneticAlgorithm Genetic;
    [SerializeField] private Individual CurrentIndividual;

    [SerializeField] private Rigidbody ShotSpherePrefab;
    [SerializeField] private Transform ShotPosition;
    [SerializeField] private Transform Target;

    [SerializeField] private float gameSpeed = 1f;

    [SerializeField] private float Degrees;
    [SerializeField] private float Strength;

    private bool isReady;

    private bool getAngle;
    private Vector3 targetRelative;
    private float angle;

    void Start()
    {
        Genetic = new GeneticAlgorithm(10, 10);
        isReady = true;

        getAngle = false;
        LootAtTarget();
    }

    public void ShooterConfigure(float degrees, float strength)
    {
        Degrees = degrees;
        Strength = strength;
    }

    public void GetResult(float data)
    {
        Debug.Log($"Result {data}");
        CurrentIndividual.fitness = data;
        isReady = true;
    }

    public void Shot()
    {
        isReady = false;

        transform.eulerAngles = new Vector3(-Degrees, angle, 0);

        var shot = Instantiate(ShotSpherePrefab, ShotPosition.position, Quaternion.identity);

        shot.gameObject.GetComponent<TargetTrigger>().Target = Target;
        shot.gameObject.GetComponent<TargetTrigger>().OnHitCollider += GetResult;
        shot.isKinematic = false;

        var force = transform.forward * Strength;
        shot.AddForce(force, ForceMode.Impulse);
    }

    void Update()
    {
        Time.timeScale = gameSpeed;

        if (isReady)
        {
            CurrentIndividual = Genetic.GetNext();

            if (CurrentIndividual != null)
            {
                ShooterConfigure(CurrentIndividual.degree, CurrentIndividual.strength);
                Shot();
            }
            else
            {
                CurrentIndividual = Genetic.GetFittest();
                isReady = false;
            }
        }
    }

    private void LootAtTarget()
    {
        if (getAngle == false)
        {
            getAngle = true;

            targetRelative = transform.InverseTransformPoint(Target.position);
            angle = Mathf.Atan2(targetRelative.x, targetRelative.z) * Mathf.Rad2Deg;
        }
    }
}