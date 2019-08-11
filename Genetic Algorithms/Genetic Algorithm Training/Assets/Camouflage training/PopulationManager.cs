using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public struct CameraBounds
{
    public Vector3 bottomLeft;
    public Vector3 topRight;
}

public class PopulationManager : MonoBehaviour {
    CameraBounds cameraBounds;

    public GameObject personPrefab;
    public int populationSize = 10;
    public int trialTime = 10;
    public int generation = 1;

    public static float timeElapsed = 0;

    List<GameObject> population = new List<GameObject>();

	// Use this for initialization
	void Start () {
        InitializeCamera();

        if(personPrefab != null)
        {
            for(int i = 0; i < populationSize; i++)
            {
                Vector3 randomPosition = new Vector3(Random.Range(cameraBounds.bottomLeft.x, cameraBounds.topRight.x),
                                                        Random.Range(cameraBounds.bottomLeft.y, cameraBounds.topRight.y), 0);
                GameObject g = Instantiate(personPrefab, randomPosition, Quaternion.identity);

                g.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
                g.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
                g.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);

                population.Add(g);
            }
        }
	}

    void InitializeCamera()
    {
        cameraBounds = new CameraBounds();
        //Intialize the camera bounds so that the object instantiation is possible
        //Orthographic size is half the height of the camera's view
        cameraBounds.bottomLeft = new Vector3(
            Camera.main.transform.position.x - Camera.main.aspect * Camera.main.orthographicSize,   //This gives half width
            Camera.main.transform.position.y - Camera.main.orthographicSize,                        //Similarly, this gives half height
            0
            );
        cameraBounds.topRight = new Vector3(
            Camera.main.transform.position.x + Camera.main.aspect * Camera.main.orthographicSize, 
            Camera.main.transform.position.y + Camera.main.orthographicSize,
            0
            );
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
            //Breed new population, reset time elapsed for this generation and increment the generation
            BreedNewPopulation();
            timeElapsed = 0;
            generation++;
        }
	}

    void BreedNewPopulation()
    {
        //Sort the population by the amount of time survived in ascending order
        List<GameObject> sortedPopulation = population.OrderBy(p => p.GetComponent<DNA>().durationOfLife).ToList();

        population.Clear();
        //Take the upper half of the population and breed them
        for(int i = (int)(sortedPopulation.Count / 2.0f) - 1; i < sortedPopulation.Count - 1; i++)
        {
            population.Add(Breed(sortedPopulation[i].GetComponent<DNA>(), sortedPopulation[i + 1].GetComponent<DNA>()));
            population.Add(Breed(sortedPopulation[i + 1].GetComponent<DNA>(), sortedPopulation[i].GetComponent<DNA>()));
        }

        //Destroy the previous generation
        for(int i = 0; i < sortedPopulation.Count; i++)
        {
            Destroy(sortedPopulation[i]);
        }
    }

    GameObject Breed(DNA parent1, DNA parent2)
    {
        //we breed the child here. The resulting child is either mutated or has properties of one of the parents
        Vector3 randomPosition = new Vector3(Random.Range(cameraBounds.bottomLeft.x, cameraBounds.topRight.x),
                                                        Random.Range(cameraBounds.bottomLeft.y, cameraBounds.topRight.y), 0);

        GameObject offspring = Instantiate(personPrefab, randomPosition, Quaternion.identity);

        if(Random.Range(0, 100) > 5)
        {
            offspring.GetComponent<DNA>().r = Random.Range(0, 10) < 5 ? parent1.r : parent2.r;
            offspring.GetComponent<DNA>().g = Random.Range(0, 10) < 5 ? parent1.g : parent2.g;
            offspring.GetComponent<DNA>().b = Random.Range(0, 10) < 5 ? parent1.b : parent2.b;
        }
        else
        {
            //mutation
            offspring.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
        }

        return offspring;
    }
}
