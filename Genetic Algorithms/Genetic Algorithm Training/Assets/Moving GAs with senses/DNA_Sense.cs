using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA_Sense{

    public int dnaLength;
    public int maxValues;
    List<int> Genes = new List<int>();

    public DNA_Sense(int length, int values)
    {
        dnaLength = length;
        maxValues = values;
        RandomInitialize();
    }

    void RandomInitialize()
    {
        //Genes.Clear();
        for(int i = 0; i < dnaLength; i++)
        {
            Genes.Add(Random.Range(0, maxValues));
        }
    }

    public int GetGene(int pos)
    {
        return Genes[pos];
    }
    public void SetGene(int pos, int value)
    {
        Genes[pos] = value;
    }

    public void Combine(DNA_Sense parent1, DNA_Sense parent2)
    {
        for(int i = 0; i < dnaLength; i++)
        {
            if(i < (dnaLength / 2.0f))
            {
                Genes[i] = parent1.Genes[i];
            }
            else
            {
                Genes[i] = parent2.Genes[i];
            }
        }
    }

    public void Mutate()
    {
        Genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
    }

}
