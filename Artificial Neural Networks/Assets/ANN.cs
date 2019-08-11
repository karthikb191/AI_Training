using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN{

    public int numInputs;   //Number of inputs coming to each neuron at the start
    public int numOutputs;  //number of outputs neurons that constitute the output layer
    public int numHiddenLayers;
    public int numNeuronsPerHiddenLayer;
    public double alpha;    //This value determies how fast your neural network is going to learn

    public List<Layer> layers = new List<Layer>();

    public ANN(int nI, int nO, int nH, int nPH, double a)
    {
        numInputs = nI; numOutputs = nO; numHiddenLayers = nH; numNeuronsPerHiddenLayer = nPH; alpha = a;

        if(numHiddenLayers > 0)
        {
            //If the ANN has hidden layers
            layers.Add(new Layer(numNeuronsPerHiddenLayer, numInputs)); //This becomes out input layer
            for(int i = 0; i < numHiddenLayers - 1; i++)
            {
                //These form our hidden layer. Every neuron gets the input from every other neuron from the previous layer
                layers.Add(new Layer(numNeuronsPerHiddenLayer, numNeuronsPerHiddenLayer));  
            }
            layers.Add(new Layer(numOutputs, numNeuronsPerHiddenLayer));    //This forms our output layer

        }
        else
        {
            //If there are no hidden layers in the neural network, then there's just a single layer for input and output
            layers.Add(new Layer(numOutputs, numInputs));
        }
    }

    //This function trains the layer. With perceptron we trained multiple inputs and one output.
    //With neural network, we can train multiple inputs and multiple outputs
    public List<double> Go(List<double> inputValues, List<double> desiredOutputs)
    {
        //Inputs and outputs to keep track of each neuron
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();

        //We are going to loop through each input of every neuron, in each layer in the neural network 
        //We do this in order to multiply the inputs with the weights
        if(inputValues.Count != numInputs)
        {
            Debug.Log("Input value Discrepency");
            return null;
        }

        //Initially, assign the inputs to the given input value set
        inputs = new List<double>(inputValues);
        //Looping through the input layer and the hidden layers
        //Debug.Log("num layers:" + (numHiddenLayers + 1));
        for(int i = 0; i < numHiddenLayers + 1; i++)
        {
            //If i > 0, assign inputs to the list of outputs of previous layer
            if( i > 0)
            {
                inputs = new List<double>(outputs);
            }
            outputs.Clear();
            //Debug.Log("neuron count: " + layers[i].numNeurons);
            //Looping through the neurons of each layer
            for(int j = 0; j < layers[i].numNeurons; j++)
            {
                double res = 0;
                layers[i].neurons[j].inputs.Clear();
                for (int k = 0; k < layers[i].neurons[j].numInputs; k++)
                {
                    //Add to the neuron's list of inputs
                    //Each neuron has the same number of weights as the inputs
                    layers[i].neurons[j].inputs.Add(inputs[k]);
                    res += inputs[k] * layers[i].neurons[j].weights[k];
                }
                //Debug.Log("sdfsfadfas");
                res -= layers[i].neurons[j].bias;
                //Add the output to the output of the respective neuron
                layers[i].neurons[j].output = ActivationFunction(res);
                //Output is prodced for every neuron in the layer.
                outputs.Add(layers[i].neurons[j].output);
            }
        }
        //After all the inputs of all the neurons are evaluated, update the weights
        UpdateWeights(outputs, desiredOutputs);

        return outputs;
    }

    double ActivationFunction(double num)
    {
        return Sigmoid(num);
    }
    double Sigmoid(double value)
    {
        double k = (double)System.Math.Exp(value);
        return k / (1.0f + k);
    }
	
    void UpdateWeights(List<double> outputs, List<double> desiredOutputs)
    {
        double error = 0;
        //Loop through the layers in reverse
        for(int i = numHiddenLayers; i >= 0; i--)
        {
            for(int j = 0; j < layers[i].numNeurons; j++)
            {
                if(i == numHiddenLayers)
                {
                    //error is the desired op - obtained op. This can only be calculated in the output later
                    error = desiredOutputs[j] - outputs[j];
                    //Error Gradient distributes the error over the entire neural network
                    layers[i].neurons[j].errorGradient = outputs[j] * (1 - outputs[j]) * error; //Delta Rule
                }
                else
                {
                    layers[i].neurons[j].errorGradient = (layers[i].neurons[j].output) * (1 - layers[i].neurons[j].output);
                    double errorGradSum = 0;
                    //If it's not the output layer, update the error gradient
                    //Loop through the neurons of the next layer
                    for(int p = 0; p < layers[i + 1].numNeurons; p++)
                    {
                        errorGradSum += layers[i + 1].neurons[p].errorGradient * layers[i + 1].neurons[p].weights[j];
                    }
                    layers[i].neurons[j].errorGradient *= errorGradSum;
                }
                //loop through each input, adjusting weights
                for(int k = 0; k < layers[i].neurons[j].numInputs; k++)
                {
                    if(i == numHiddenLayers)
                    {
                        error = desiredOutputs[j] - outputs[j];
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * error;
                    }
                    else
                    {
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * layers[i].neurons[j].errorGradient;
                    }
                }
                layers[i].neurons[j].bias += alpha * -1 * layers[i].neurons[j].errorGradient;
            }
        }
    }
    
}
