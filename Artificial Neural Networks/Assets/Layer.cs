using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Every neural network consists of a bunch of neurons. 
/// They have bunch of inputs and outputs connecting to othher layers
/// </summary>

public class Layer : MonoBehaviour {

    public int numNeurons; //This tells how many neurons this layer has
    public List<Neuron> neurons = new List<Neuron>();

    public Layer(int num, int neuronInputs)
    {
        numNeurons = num;
        //Create neurons with respective inputs
        for(int i = 0; i < numNeurons; i++)
        {
            neurons.Add(new Neuron(neuronInputs));
        }
    }

}
