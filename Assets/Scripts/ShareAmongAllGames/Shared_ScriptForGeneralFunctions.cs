using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shared_ScriptForGeneralFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 GetPointOnScreen(float width_percentage, float height_percentage){
		var screen_height = Camera.main.pixelHeight;
		var screen_width = Camera.main.pixelWidth;
		return Camera.main.ScreenToWorldPoint (new Vector3 (screen_width * width_percentage, screen_height * height_percentage, Camera.main.nearClipPlane));

	}
}
