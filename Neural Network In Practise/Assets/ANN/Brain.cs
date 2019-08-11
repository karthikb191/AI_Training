using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    ANN ann;
    double sumSquareError = 0;
	// Use this for initialization
	void Start () {
        List<double> result = new List<double>();
        ann = new ANN(2, 1, 1, 2, 0.8f);
        for(int i = 0; i < 10000; i++)
        {
            sumSquareError = 0;
            result = ann.Go(new List<double> { 0, 0 }, new List<double> { 0 });
            sumSquareError += Mathf.Pow((float)result[0] - 0, 2);

            result = ann.Go(new List<double> { 0, 1 }, new List<double> { 1 });
            sumSquareError += Mathf.Pow((float)result[0] - 1, 2);

            result = ann.Go(new List<double> { 1, 0 }, new List<double> { 1 });
            sumSquareError += Mathf.Pow((float)result[0] - 1, 2);

            result = ann.Go(new List<double> { 1, 1 }, new List<double> { 0 });
            sumSquareError += Mathf.Pow((float)result[0] - 0, 2);
        }
        Debug.Log("SSE: " + sumSquareError);
	}
	
}
