using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShotgunConfiguration : MonoBehaviour
{
    [Header("Other Scripts")]
    [SerializeField] private GeneticAlgorithm Genetic;
    [SerializeField] private Individual CurrentIndividual;

    [Header("References")]
    [SerializeField] private Rigidbody ShotSpherePrefab;
    [SerializeField] private Transform ShotPosition;
    [SerializeField] private Transform Target;

    [Header("Shooting")]
    [SerializeField] private float Strength;
    [SerializeField] private float Degrees;
                     public static float currentBestStrength;
                     public static float currentBestDegree = 90;
    [SerializeField] private float CB_Strength;
    [SerializeField] private float CB_Degrees;

    private bool getAngle;
    private Vector3 targetRelative;
    private float angle;

    private GameObject compassReference;

    private float extraImpulse = 0.1f;

    [Header("Gameplay")]
    [SerializeField] private int poblacion;
    [SerializeField] private float gameSpeed;
                     private bool isReady;
    [SerializeField] private bool done;

    [Header("Case Selection")]
    [Range(1, 3)] public int caseNumber = 1;

    void Start()
    {
        compassReference = GameObject.Find("compassReference");

        Genetic = new GeneticAlgorithm(poblacion, poblacion);
        isReady = true;
        done = false;

        getAngle = false;
        LootAtTarget();
    }

    public void ShooterConfigure(float xDegrees, float strength)
    {
        Degrees = xDegrees;
        Strength = strength;
    }

    public void GetResult(float data)
    {
        Debug.Log($"Result { data}");
        CurrentIndividual.fitness = data;
        isReady = true;
    }

    public void Shot()
    {
        isReady = false;

        float angleY = compassReference.transform.rotation.eulerAngles.y;
        transform.eulerAngles = new Vector3(Degrees, angleY, 0);

        var shot = Instantiate(ShotSpherePrefab, ShotPosition.position, Quaternion.identity);

        shot.gameObject.GetComponent<TargetTrigger>().Target = Target;
        shot.gameObject.GetComponent<TargetTrigger>().OnHitCollider += GetResult;
        shot.isKinematic = false;

        var force = (transform.up * Strength);
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
                CheckObstacleCollision();
            }
            else
            {
                CurrentIndividual = Genetic.GetFittest();
                isReady = false;
                done = true;
            }
        }
    }

    // 1. - Rotate Y Axis:
    private void LootAtTarget()
    {
        if (getAngle == false)
        {
            getAngle = true;

            targetRelative = transform.InverseTransformPoint(Target.position);
            angle = Mathf.Atan2(targetRelative.x, targetRelative.z) * Mathf.Rad2Deg;
        }
    }

    // 2. - Calculate the most optimal throw:
    private void CheckObstacleCollision()
    {
        if (TargetTrigger.obstacleCollision == true)
        {
            if (currentBestDegree > Degrees)
            {
                currentBestDegree = Degrees;
                CB_Degrees = currentBestDegree;
            }

            if (currentBestStrength < Strength)
            {
                currentBestStrength = Strength;
                CB_Strength = currentBestStrength;
            }
        }
    }
}
