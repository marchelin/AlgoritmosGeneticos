using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GeneticAlgorithm
{
    public int caseNumber = 3;   

    public List<Individual> population;
    private int _currentIndex;

    public int CurrentGeneration;
    public int MaxGenerations;

    public string Summary;

    public GeneticAlgorithm(int numberOfGenerations, int populationSize, int caseType)
    {
        caseNumber = caseType;
        CurrentGeneration = 0;
        MaxGenerations = numberOfGenerations;
        GenerateRandomPopulation(populationSize);
        Summary = "";
    }
    public void GenerateRandomPopulation(int size)
    {
        population = new List<Individual>();        

        for (int i = 0; i < size; i++)
        {
            population.Add(new Individual(Random.Range(0, ShotgunConfiguration.currentBestDegree), Random.Range(ShotgunConfiguration.currentBestStrength, 15f)));
        }

        StartGeneration();
    }

    public Individual GetFittest()
    {
        population.Sort();
        return population[0];
    }


    public void StartGeneration()
    {
        _currentIndex = 0;
        CurrentGeneration ++;
    }
    public Individual GetNext()
    {
        if (_currentIndex == population.Count)
        {
            EndGeneration();

            if (CurrentGeneration >= MaxGenerations)
            {
                Debug.Log(Summary);
                return null;
            }

            StartGeneration();
        }

        return population[_currentIndex++];
    }

    public void EndGeneration()
    {
        population.Sort();
        Summary += $"{GetFittest().fitness};";

        if (CurrentGeneration < MaxGenerations)
        {
            if (caseNumber == 1) // Case 1
            {
                Crossover();
                //Debug.LogWarning("1case");
            }
            else if (caseNumber == 2) // Case 2
            {
                Mutation();
                //Debug.LogWarning("2case");
            }
            else if (caseNumber == 3) // Case 3
            {
                Crossover();
                Mutation();
                //Debug.LogWarning("3case");
            }
        }
    }

    public void Crossover()
    {
        //SELECCION
        var ind1 = population[0];
        var ind2 = population[1];
        //

        //Cruce Plano Mono Punto//
        var new1 = new Individual(ind1.degree, ind2.strength);
        var new2 = new Individual(ind2.degree, ind1.strength);

        //REEMPLAZO

        /*
        if (TargetTrigger.obstacleCollision == true)
        {
            for (int i = 0; i < population.Count; i++)
            {
                if (population[i].degree < ShotgunConfiguration.currentBestDegree)
                {
                    population.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }
        */

        population.RemoveAt(population.Count - 1);
        population.RemoveAt(population.Count - 1);
        population.Add(new1);
        population.Add(new2);
    }

    public void Mutation()
    {
        foreach (var individual in population)
        {
            if (Random.Range(0f, 1f) < 0.02f)
            {
                individual.degree = Random.Range(0f, 90f);
            }
            if (Random.Range(0f, 1f) < 0.02f)
            {
                individual.strength = Random.Range(0f, 15f);
            }
        }
    }
}
