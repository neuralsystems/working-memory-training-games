using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FragmentScript : MonoBehaviour {

	string targetRoot = "EndofRoad", pathRoot = "Fragment";
	string targetName, pathName ;
	static bool[] reached = new bool[]{false,false,false,false};
	Transform destination;
	Vector3 velocity = Vector3.zero, target;
	float smoothTime = 0.3f;
	// Use this for initialization
	void Start () {
		targetName = targetRoot + this.gameObject.name;
		destination = GameObject.Find (targetName).transform;
		pathName = pathRoot + this.gameObject.name;
//		iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath(pathName),"speed",speed, "oncomplete","AfterComplete"));
		target = Camera.main.GetComponent<CarGame_SceneVariables>().screenCorners[System.Convert.ToInt32(name) -1 ];
		StartCoroutine (MergeOnMatch (target));
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void AfterComplete(){
		GameObject go = transform.parent.gameObject;
		int index = Convert.ToInt32(name);
		reached[index-1] = true;
		bool allReached = true;
		for (int i = 0; i < reached.Length; i++) {
			allReached = allReached && reached [i];

		}
		if (allReached) {
			StartCoroutine(Camera.main.GetComponent<CarGame_GameManager> ().MoveBack ());
//			Destroy (transform.parent.gameObject);
		}
	}

	public IEnumerator MergeOnMatch(Vector3 target){
		if (Vector3.Distance (transform.position, target) > CarGame_SceneVariables.minDistance) {
			//			Debug.Log ("first if");
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			StartCoroutine (MergeOnMatch (target));
		} else {
			transform.position = target;
			AfterComplete ();
		}

	}
}
