using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrainingSet
{
    public double[] inputs;
    public double output;
}

public class Perceptron_Xor : MonoBehaviour {

    //We need a list of training sets to train the perceptron
    public TrainingSet[] trainingSet;
    double[] weights = { 0, 0 };
    double bias = 0;
    double totalError = 0;  //Total error for a single epoch

    void InitializePerceptron()
    {
        for(int i = 0; i < weights.Length; i++)
        {
            weights[i] = Random.Range(-1.0f, 1.0f);
        }

        bias = Random.Range(-1.0f, 1.0f);
    }

    //Epoch is a single iteration of perceptron training over entire training set
    void Train(int epochs)
    {
        InitializePerceptron();

        for(int i = 0; i < epochs; i++)
        {
            totalError = 0;
            for(int j = 0; j < trainingSet.Length; j++)
            {
                UpdateWeights(j);
                Debug.Log("weight 1: " + weights[0] + " Weight 2: " + weights[1] + " Bias: " + bias);
            }
            if (totalError == 0)
                Debug.Log("Error is zero. Training finished at " + i+1 + " epoch");
            else
                Debug.Log("Total Error: " + totalError);
        }
    }

    void UpdateWeights(int index)
    {
        double error = trainingSet[index].output - CalculateOutput(index);
        totalError += Mathf.Abs((float)error);

        for(int i = 0; i < trainingSet[index].inputs.Length; i++)
        {
            weights[i] = trainingSet[index].inputs[i] * error + weights[i];
        }
        bias += error;
    }

    double CalculateOutput(int index)
    {
        if (ActivationFunction(trainingSet[index].inputs, weights) > 0) return 1;
        else
            return 0;
    }

    //Activation function: summation of inputs and weights + bias
    double ActivationFunction(double[] inputs, double[] weights)
    {
        if (inputs == null || weights == null)
            return -1;
        if (inputs.Length != weights.Length)
            return -1;

        double result = 0;
        for(int i = 0; i < inputs.Length; i++)
        {
            result += inputs[i] * weights[i];
        }
        result += bias;
        return result;
    }

    //Function that uses the learned perceptron to calculate results
    double CalculateOutput(double input1, double input2)
    {
        double[] inputs = { input1, input2 };
        double dp = ActivationFunction(inputs, weights);
        if (dp > 0) return 1;
        return 0;
    }

	// Use this for initialization
	void Start () {
        Train(10);
        Debug.Log("0    0:  " + CalculateOutput(0, 0));
        Debug.Log("1    0:  " + CalculateOutput(1, 0));
        Debug.Log("0    1:  " + CalculateOutput(0, 1));
        Debug.Log("1    1:  " + CalculateOutput(1, 1));
	}
	
}
