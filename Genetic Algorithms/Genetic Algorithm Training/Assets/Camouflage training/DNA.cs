using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour {

    public float r;
    public float g;
    public float b;

    public bool dead;   //Flag that checks if the object is actually dead
    public float durationOfLife;    //This variable is needed to keep track of the game object that survived the longest
    
    Collider2D collider;
    SpriteRenderer renderer;
	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(r, g, b);
	}

    private void OnMouseDown()
    {
        dead = true;
        renderer.enabled = false;
        collider.enabled = false;
        durationOfLife = PopulationManager.timeElapsed;
    }

}
