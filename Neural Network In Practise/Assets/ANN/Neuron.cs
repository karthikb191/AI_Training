using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {
    //A neuron can have input from multiple sources. Each input is associated with some weight. It has single output
    public int numInputs;
    public double output;
    public List<double> weights = new List<double>();
    public List<double> inputs = new List<double>();
    public double bias;
    public double errorGradient;
    public Neuron(int nInputs)
    {
        numInputs = nInputs;
        for(int i = 0; i < numInputs; i++)
        {
            weights.Add(Random.Range(-1.0f, 1.0f));
        }
        bias = Random.Range(-1.0f, 1.0f);
    }
}
