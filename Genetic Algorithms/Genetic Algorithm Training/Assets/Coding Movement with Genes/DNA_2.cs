using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA_2 {
    public int dnaLength;
    List<int> Genes = new List<int>();
    public int maxValues;

    public DNA_2(int length, int values)
    {
        dnaLength = length;
        maxValues = values;
        RandomInitialize();
    }

    void RandomInitialize()
    {
        for(int i = 0; i < dnaLength; i++)
        {
            Genes.Add(Random.Range(0, maxValues));
        }
    }

    public void Combine(DNA_2 parent1, DNA_2 parent2)
    {
        //Split the DNA into two parts. We get one part from the first parent and the other from the second parent
        //Since this example only uses DNA of length = 1, we always get the DNA from the first parent only
        for(int i = 0; i < dnaLength; i++)
        {
            if(i < dnaLength / 2.0f)
            {
                //Get the reespective gene from the parent 1
                Genes[i] = parent1.Genes[i];
            }
            else
            {
                //Get the respective gene from the parent 2
                Genes[i] = parent2.Genes[i];
            }
        }

    }
    
    public void Mutate()
    {
        //A random value is assigned for a random gene
        Genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
    }

    public int GetGene(int pos)
    {
        return Genes[pos];
    }
    public void SetGene(int pos, int value)
    {
        Genes[pos] = value;
    }
}
