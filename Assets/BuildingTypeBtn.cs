using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeBtn : MonoBehaviour {
	
	// reference to the GlobalManager singleton class
	GlobalManager gm_Instance;
	
	public GlobalManager.BuildingType buildingType;
	public int buildingTypeCost;
	
	void Start () {
		// Get the reference to the GlobalManager singleton class
		gm_Instance = GlobalManager.gm_Instance;
		Debug.Log(gm_Instance);
	}
	
	public void SetBuildingType()
	{
		gm_Instance.SelectBuilding(buildingType);
	}
	
	public void StartCheckingBalance(){
		
		/*if (gm_Instance.GetTotalBalance() >= buildingTypeCost){
			GetComponent<Button>().interactable = true;
		}
		else{
			GetComponent<Button>().interactable = false;
		}*/
		StartCoroutine(CheckBalance());
	}
	
	IEnumerator CheckBalance(){
		yield return new WaitForSecondsRealtime(0.1f);
		if (gm_Instance.GetTotalBalance() >= buildingTypeCost){
			GetComponent<Button>().interactable = true;
		}
		else{
			GetComponent<Button>().interactable = false;
		}
		
		yield return new WaitForSecondsRealtime(1.0f);
		StartCoroutine(CheckBalance());
	}
}
