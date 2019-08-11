using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain_Sense : MonoBehaviour {
    
    GameObject eye;
    public LayerMask layerMask;

    public DNA_Sense dna;

    int dnaLength = 2;

    public bool alive = true;
    public bool seeGround;
    public float timeAlive;
    public float timeTravelled;

    // Use this for initialization
    void Start () {
        eye = transform.Find("Eye").gameObject;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "dead")
        {
            alive = false;
        }    
    }

    public void Init()
    {
        //0 - forward move, 1 - rotate left, 2 - rotate right
        //We need 2 genes. One for normal motion and other for the decision when a dead floor is encountered
        dna = new DNA_Sense(dnaLength, 3);
        alive = true;
        timeAlive = 0;
        timeTravelled = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (!alive) return;

        timeAlive = PopulationManager_Sense.timeElapsed;

        //Floor Detection
        seeGround = false;
        RaycastHit hit;
        Debug.DrawRay(eye.transform.position, eye.transform.forward * 10, Color.green);
        if(Physics.Raycast(eye.transform.position, eye.transform.forward, out hit, 10, layerMask))
        {
            if(hit.collider.tag == "platform")
            {
                seeGround = true;
            }
            else
            {
                seeGround = false;
            }
        }

        Move();
	}

    void Move()
    {
        float move = 0, turn = 0;
        if (seeGround)
        {
            //The value of the first gene is evaluated
            if (dna.GetGene(0) == 0) { move = 1; timeTravelled += PopulationManager_Sense.timeElapsed; }
            else if (dna.GetGene(0) == 1) turn = -90;
            else if (dna.GetGene(0) == 2) turn = 90;
        }
        else
        {
            //If the ground is not seen, then the other gene is evaluated for its value
            //The value of the first gene is evaluated
            if (dna.GetGene(1) == 0) move = 1;
            else if (dna.GetGene(1) == 1) turn = -90;
            else if (dna.GetGene(1) == 2) turn = 90;
        }

        //Move the capsule
        transform.Translate(new Vector3(0, 0, move * 0.1f));
        transform.Rotate(new Vector3(0, turn, 0));
    }
}
