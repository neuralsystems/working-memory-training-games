using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {


	public float time = 0f;
	public bool start = false;
	// Use this for initialization
	void Start () {
		
	}
		
	// Update is called once per frame
	void Update () {
		if (start) {
			time += Time.deltaTime;
		}
	}

	public void  startTimer(){
		start = true;
	}

	public void StopTimer(){
		start = false; 
	}

	public void ResetTimer(){
		start = false;
		time = 0f;
	}
}
