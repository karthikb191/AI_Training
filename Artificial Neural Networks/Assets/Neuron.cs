using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Neuron is the same as the perceptron.
/// This class just holds the same data that the perceptron holds
/// </summary>

public class Neuron{
    public int numInputs;   //Number of inputs coming to the neuron
    public double bias;     
    public double errorGradient;
    public double output;
    public List<double> inputs = new List<double>();
    public List<double> weights = new List<double>();

    public Neuron(int nInputs)
    {
        numInputs = nInputs;
        bias = Random.Range(-1.0f, 1.0f);
        for(int i = 0; i < nInputs; i++)
        {
            weights.Add(Random.Range(-1.0f, 1.0f));
        }
    }
}
