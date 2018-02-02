using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCell : MonoBehaviour {

	// The original color of the ground cube's material
	Color32 originalColor;
	
	void Start () {
		//Get the color from the 'Top' child object
		originalColor = GetComponentsInChildren<Renderer>()[1].material.color; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown()
	{
		Debug.Log("Received click on: " + gameObject.name);
	}
	
	// Create a mouse hover effect when the mouse is over a buildable area
	void OnMouseEnter() {
        // Enlight the surface of the area
		GetComponentsInChildren<Renderer>()[1].material.color = Color.white;
    }	
    void OnMouseExit() {
		// Set back the original color
        GetComponentsInChildren<Renderer>()[1].material.color = originalColor;
    }
}
