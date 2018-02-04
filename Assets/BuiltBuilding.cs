using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuiltBuilding : MonoBehaviour {
		
	Material[] materials;
	
	float highlighter = 1.4f;
	
	// Use this for initialization
	void Start () {
		
		materials = GetComponent<Renderer>().materials;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Show generated income on click
	void OnMouseDown()
	{
		transform.parent.GetComponent<GroundCell>().InstantiateIncomeUI();
	}
	// Create a mouse hover effect when the mouse is over a built building
	void OnMouseEnter() {
        // Enlight the surface of the area
		if (true){
			foreach(Material mat in materials){
				mat.color = new Color (mat.color.r * highlighter, mat.color.g * highlighter, mat.color.b * highlighter, 1);
			}
		}
    }	
    void OnMouseExit() {
		// Set back the original color
        if (true){
			foreach(Material mat in materials){
				mat.color = new Color (mat.color.r / highlighter, mat.color.g / highlighter, mat.color.b / highlighter, 1);
			}
			
			StopCoroutine(transform.parent.GetComponent<GroundCell>().UpdateUIIncome());
			GameObject incomeUI = transform.parent.GetComponent<GroundCell>().IncomeUI;
			if (incomeUI){
				Destroy(incomeUI);
			}
		}
    }
}
