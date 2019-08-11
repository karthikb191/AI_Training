using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Brain : MonoBehaviour {

    public int dnaLength = 1;


    public float distanceTravelled;
    public float timeAlive;
    public bool dead;
    public ThirdPersonCharacter character;
    public DNA_2 dna;

    Vector3 startPosition;
    

    public void Init()
    {
        dna = new DNA_2(dnaLength, 6);
        character = GetComponent<ThirdPersonCharacter>();
        //The reset properties of the characters.
        timeAlive = 0.0f;
        dead = false;
        startPosition = gameObject.transform.position;
    }

    bool jump = false;

    private void Update()
    {
        // 0-forward 1-back 2-left 3-right 4-jump 5-crouch
        int forward = 0, side = 0; bool crouch = false;

        if (dna.GetGene(0) == 0) forward = 1;
        else if (dna.GetGene(0) == 1) forward = -1;
        else if (dna.GetGene(0) == 2) side = -1;
        else if (dna.GetGene(0) == 3) side = 1;
        else if (dna.GetGene(0) == 4) jump = true;
        else if (dna.GetGene(0) == 5) crouch = true;

        Vector3 moveVector = forward * Vector3.forward + side * Vector3.right;
        character.Move(moveVector, crouch, jump);
        jump = false;

        if(!dead)
            RegisterFitnessValues();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "dead")
        {
            dead = true;
            RegisterFitnessValues();
        }
    }

    public void RegisterFitnessValues()
    {
        //Once the character dies, register the time character was alive and the distance travelled
        timeAlive = PopulationManager_2.timeElapsed;
        distanceTravelled = Vector3.Distance(startPosition, gameObject.transform.position);
    }
}
