using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager_2 : MonoBehaviour {

    public GameObject personPrefab;
    public GameObject spawnPoint;
    public int populationSize = 20;
    public int trialTime = 10;
    public int generation = 1;

    public static float timeElapsed = 0;

    List<GameObject> population = new List<GameObject>();
    // Use this for initialization
    void Start () {
        //Create the initial generation of bots
		for(int i = 0; i< populationSize; i++)
        {
            Vector3 randomPosition = new Vector3(
                    spawnPoint.transform.position.x + Random.Range(-2, 2),
                    spawnPoint.transform.position.y,
                    spawnPoint.transform.position.z + Random.Range(-2, 2));

            GameObject g = Instantiate(personPrefab, randomPosition, Quaternion.identity);
            g.GetComponent<Brain>().Init();

            population.Add(g);
        }
	}

    private void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(30, 30, 100, 50), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(30, 90, 100, 50), "Time Elapsed: " + timeElapsed, guiStyle);
    }

    // Update is called once per frame
    void Update () {
        timeElapsed += Time.deltaTime;
		if(timeElapsed >= trialTime)
        {
            
            BreedNewPopulation();
            timeElapsed = 0;
            generation++;
        }
	}

    void BreedNewPopulation()
    {
        //Change the sorted list here for a different fitness test
        List<GameObject> sortedPopulation = population.OrderBy(p => p.GetComponent<Brain>().distanceTravelled).ToList();

        //After the population is sorted, clear the population list
        population.Clear();

        for(int i = (int)(sortedPopulation.Count/2) - 1; i < sortedPopulation.Count - 1; i++)
        {
            Breed(sortedPopulation[i].GetComponent<Brain>().dna, sortedPopulation[i+1].GetComponent<Brain>().dna);
            Breed(sortedPopulation[i+1].GetComponent<Brain>().dna, sortedPopulation[i].GetComponent<Brain>().dna);
        }

        //Destroy the previous generation after breeding
        for(int i = 0; i < sortedPopulation.Count; i++)
        {
            Destroy(sortedPopulation[i]);
        }
    }

    void Breed(DNA_2 parent1, DNA_2 parent2)
    {
        Vector3 randomPosition = new Vector3(
                    spawnPoint.transform.position.x + Random.Range(-2, 2),
                    spawnPoint.transform.position.y,
                    spawnPoint.transform.position.z + Random.Range(-2, 2));

        GameObject offspring = Instantiate(personPrefab, randomPosition, Quaternion.identity);

        offspring.GetComponent<Brain>().Init();

        if (Random.Range(0, 100) < 2)
        {
            offspring.GetComponent<Brain>().dna.Mutate();
        }
        else
        {
            offspring.GetComponent<Brain>().dna.Combine(parent1, parent2);
        }

        population.Add(offspring);
    }

}
