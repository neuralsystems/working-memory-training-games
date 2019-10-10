using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class TrainGame_SampleShapeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (ChangeOutline ());
	}



	// Update is called once per frame
	void Update () {
		
	}


	IEnumerator ChangeOutline(){
		while (true) {
			yield return new WaitForSeconds (1f);
			GetComponent<Outline> ().eraseRenderer = !GetComponent<Outline> ().eraseRenderer;
		}
	}


}
