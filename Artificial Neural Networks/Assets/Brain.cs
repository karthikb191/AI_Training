using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    ANN ann;
    double sunSquareError = 0;

    List<double> result = new List<double>();
	// Use this for initialization
	void Start () {
        ann = new ANN(2, 1, 1, 2, 0.8);
        for(int i = 0; i < 1000; i++)
        {
            sunSquareError = 0;
            result = Train(0, 0, 0);
            sunSquareError += Mathf.Pow((float)result[0] - 0, 2);
            result = Train(1, 0, 1);
            sunSquareError += Mathf.Pow((float)result[0] - 1, 2);
            result = Train(0, 1, 1);
            sunSquareError += Mathf.Pow((float)result[0] - 1, 2);
            result = Train(1, 1, 0);
            sunSquareError += Mathf.Pow((float)result[0] - 0, 2);
        }
        Debug.Log("sun squared error: " + sunSquareError);

        result = Train(0, 0, 0);
        Debug.Log("0 0: " + result[0]);

        result = Train(0, 1, 1);
        Debug.Log("0 1: " + result[0]);

        result = Train(1, 0, 1);
        Debug.Log("1 0: " + result[0]);

        result = Train(1, 1, 0);
        Debug.Log("1 1: " + result[0]);
    }

	List<double> Train(double x, double y, double r)
    {
        return ann.Go(new List<double> { x, y }, new List<double> { r });
    }
}
