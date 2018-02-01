using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour {
	
	// Class that holds global settings configuration
	
	// Singleton object
	public static GlobalManager gm_Instance;
	
	// The exact reference date, to calculate timestamps
	private System.DateTime referenceDate;
	public System.DateTime ReferenceDate
	{
		get {return referenceDate;}
		set {referenceDate = value;}
	}
	
	// Use this for initialization
	void Awake () {
		// Set the 2nd of Febryary 2018 as the exact ref date, to calculate timestamps. (Keeps the variables' values small)
		referenceDate = new System.DateTime(2018, 2, 2, 0, 0, 0, System.DateTimeKind.Utc);
		
		// A usage example of the reference date. Get current timestamps in seconds
		int cur_time = (int)(System.DateTime.UtcNow - referenceDate).TotalSeconds;
		Debug.Log(cur_time);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
