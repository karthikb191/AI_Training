using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN {
    public int numInputs;
    public int numHidden;
    public int numOutputs;
    public int neuronsPerHiddenLayer;
    public float alpha;   //The rate of training
    public List<Layer> layers = new List<Layer>();  //ANN should keep track of all the layers in it
    public ANN(int nI, int nO, int nH, int nNH, float a)
    {
        numInputs = nI;
        numOutputs = nO;
        numHidden = nH;
        neuronsPerHiddenLayer = nNH;
        alpha = a;

        if(numHidden > 0)
        {
            //Check this area if there's a problem with output
            layers.Add(new Layer(neuronsPerHiddenLayer, numInputs));
            for (int i = 0; i < numHidden - 1; i++)
            {
                //if (i == 0)
                //    layers.Add(new Layer(neuronsPerHiddenLayer, numInputs));
                //else
                    layers.Add(new Layer(neuronsPerHiddenLayer, neuronsPerHiddenLayer));
            }
            layers.Add(new Layer(numOutputs, neuronsPerHiddenLayer));
        }
        else
        {
            //We just need a single layer in this case. In this case, it's mostly a single neuron / perceptron
            layers.Add(new Layer(numOutputs, numInputs));
        }
    }
    public List<double> Go(List<double> inputValues, List<double> desiredOutputs)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        if(inputValues.Count != numInputs)
        {
            Debug.Log("Error! Input values provided is not right");
            return null;
        }

        inputs = new List<double>(inputValues);
        for(int i = 0; i < numHidden + 1; i++)
        {
            if (i > 0)
                inputs = new List<double> (outputs);   //Inputs become the outputs of the previous layer

            outputs.Clear();
            //Looping through the neurons
            for(int j = 0; j < layers[i].numNeurons; j++)
            {
                double n = 0;
                //If a previous set of inputs exist, they must be cleared because we are obtaining the updated inputs in all the layers except input
                layers[i].neurons[j].inputs.Clear();
                //Looping through each input of neurons
                for (int k = 0; k < layers[i].neurons[j].numInputs; k++)
                {
                    layers[i].neurons[j].inputs.Add(inputs[k]); //Add input to the neutron
                    n += inputs[k] * layers[i].neurons[j].weights[k];
                }
                n -= layers[i].neurons[j].bias;
                layers[i].neurons[j].output = ActivationFunction(n);
                outputs.Add(layers[i].neurons[j].output);
            }
        }
        UpdateWeights(outputs, desiredOutputs);
        return outputs;
    }
    void UpdateWeights(List<double> outputs, List<double> desiredOutputs)
    {
        double error = 0;
        for(int i = numHidden; i >= 0; i--)
        {
            //Debug.Log("layer count: " + layers.Count + " i " + i + " neu " + layers[i].numNeurons);
            //Back propagation
            for(int j = 0; j < layers[i].numNeurons; j++)
            {
                //The error gradient is adjust for each neuron. Not for each input
                //Adjusting the error gradient
                if(i == numHidden)
                {
                    error = desiredOutputs[j] - outputs[j];
                    layers[i].neurons[j].errorGradient = outputs[j] * (1 - outputs[j]) * error; //Delta rule
                }
                else
                {
                    layers[i].neurons[j].errorGradient = layers[i].neurons[j].output * (1 - layers[i].neurons[j].output);
                    double errorGradSum = 0;
                    for(int p = 0; p < layers[i+1].numNeurons; p++)
                    {
                        errorGradSum += layers[i + 1].neurons[p].errorGradient * layers[i + 1].neurons[p].weights[j];
                    }
                    layers[i].neurons[j].errorGradient *= errorGradSum;
                }

                //Adjusting the actual weights
                for(int k = 0; k < layers[i].neurons[j].numInputs; k++)
                {
                    if(i == numHidden)
                    {
                        error = desiredOutputs[j] - outputs[j];
                        layers[i].neurons[j].weights[k] += layers[i].neurons[j].inputs[k] * error * alpha;
                    }
                    else
                    {
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * layers[i].neurons[j].errorGradient;
                    }
                }
                //Adjust the bias for the particular neuron
                layers[i].neurons[j].bias += alpha * -1 * layers[i].neurons[j].errorGradient;
            }
        }
    }

    double ActivationFunction(double num)
    {
        return Sigmoid(num);
    }
    double Sigmoid(double num)
    {
        double k = (double)System.Math.Exp(num);
        return k / (1.0f + k);
    }
}
