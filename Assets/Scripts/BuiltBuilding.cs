using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuiltBuilding : MonoBehaviour {
	// Controls the input mouse events over a constructed building on the grid
	
	Renderer[] renderers;
	Material[] materials;
	
	float highlighter = 1.4f;
	
	bool built = false;
	public bool Built{
		get {return built;}
		set {built = value;}
	}
	
	bool canShowUI = false;
	
	// Use this for initialization
	void Start () {
		
		renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers){
			materials = renderer.materials;
		}
		
		canShowUI = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Show generated income on click on a built building
	void OnMouseDown()
	{
		if (built && canShowUI){
			transform.parent.GetComponent<GroundCell>().InstantiateIncomeUI();
			canShowUI = false;
		}
	}
	// Create a mouse hover effect when the mouse is over a built building
	void OnMouseEnter() {
        // Enlight the surface of the area
		if (built){
			foreach(Material mat in materials){
				mat.color = new Color (mat.color.r * highlighter, mat.color.g * highlighter, mat.color.b * highlighter, 1);
			}
		}
    }	
    void OnMouseExit() {
		// Set back the original color
        if (built){
			foreach(Material mat in materials){
				mat.color = new Color (mat.color.r / highlighter, mat.color.g / highlighter, mat.color.b / highlighter, 1);
			}
			
			StopCoroutine(transform.parent.GetComponent<GroundCell>().UpdateUIIncome());
			GameObject incomeUI = transform.parent.GetComponent<GroundCell>().IncomeUI;
			if (incomeUI){
				Destroy(incomeUI);
			}
			canShowUI = true;
		}
    }
}
