using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomeEffect : MonoBehaviour {
	// Script that handles the effect when a constructed building generates income

	Vector2 originalPosition, targetPosition;
	
	RectTransform effectRect;

	// Reference to the image component
	Image parentImage;
	Color32 originalImageColor;
	
	// Reference to the text child 
	Text incomeText;
	Color32 originalTextColor;
	
	float effectDuration = 0.5f;
	float timeInterval = 0.0f;
	
	void Start () {
		// Get the image's original color;
		parentImage = GetComponentInChildren<Image>();
		originalImageColor = parentImage.color;

		//Get the text's reference
		incomeText = GetComponentInChildren<Text>();
		originalTextColor = incomeText.color;
		
		effectRect = GetComponent<RectTransform>();
		originalPosition = effectRect.anchoredPosition;
		
		Invoke("DestroySelf", effectDuration);
	}
	
	void Update () {
		effectRect.anchoredPosition = new Vector2(originalPosition.x, Mathf.Lerp(originalPosition.y, originalPosition.y + 30, timeInterval));
		
		parentImage.color = new Color32(originalImageColor.r, originalImageColor.g, originalImageColor.b, (byte)Mathf.Lerp(originalImageColor.a, 0, timeInterval));
		incomeText.color = new Color32(originalTextColor.r, originalTextColor.g, originalTextColor.b, (byte)Mathf.Lerp(originalTextColor.a, 0, timeInterval));
		
		if (timeInterval <= effectDuration){
			timeInterval += Time.deltaTime;
		}
	}
	
	void DestroySelf(){
		Destroy(gameObject);
	}
}
