using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGame_Engine_Behavior : MonoBehaviour {

	public string TRAIN_TYPE;
	Vector3 velocity = Vector3.zero; 
	float smoothTime = .5f, minDistance = 0.01f;
	// Use this for initialization
	void Start () {
		
		StartCoroutine (InitialAnimation ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// only to be used if the train is of type Key and Lock
	IEnumerator DetachBogie(){
		var detacher = new GameObject();
		var num_of_bogies = transform.childCount ;
		int x = num_of_bogies;
		var N_points = Shared_ScriptForGeneralFunctions.GetNPointsAtHeight (.5f, num_of_bogies-1, true, 0.05f,0.05f);
		for (int i = num_of_bogies-1; i > 0; i--) {
			Debug.Log ("child number "+ i);
			var bogie_object = transform.GetChild (i).gameObject;
			var original_position = bogie_object.transform.position;
			bogie_object.GetComponent<TrainGame_BogieBehavior> ().enabled = true;
			bogie_object.GetComponent<TrainGame_BogieBehavior> ().original_position = original_position;
			var move_back_to = original_position;
			move_back_to.x += x;
			x-=1;
			Debug.Log ("move_back_to " + move_back_to);
			Debug.Log("to this point1");
			yield return StartCoroutine(bogie_object.GetComponent<TrainGame_BogieBehavior> ().MoveToTargetAndSet (move_back_to, false, TrainGame_SceneVariables.BOGIE_TAG));
			yield return new WaitForSeconds (2f);
		}
		yield return new WaitForSeconds (2f);
		for (int i = 1; i < num_of_bogies; i++) {
			var bogie_object = transform.GetChild (i).gameObject;
			bogie_object.GetComponent<TrainGame_BogieBehavior> ().AttachBack();
			yield return new WaitForSeconds (3f);
		}
		yield return new WaitForSeconds (1f);
		for (int i = 1; i < num_of_bogies; i++) {
			var bogie_object = transform.GetChild (i).gameObject;
			Debug.Log ("position for " + i + N_points[i-1] );
			StartCoroutine(bogie_object.GetComponent<TrainGame_BogieBehavior> ().MoveToTargetAndSet (N_points[i-1], true, TrainGame_SceneVariables.BOGIE_TAG));
		}
	}


	IEnumerator InitialAnimation(){
		transform.position = Shared_ScriptForGeneralFunctions.GetPointOnScreen (1.1f, .85f);
		var target = Shared_ScriptForGeneralFunctions.GetPointOnScreen (.1f, .85f);
		yield return StartCoroutine(MoveToTarget( target));
		yield return new WaitForSeconds (2f);
		StartCoroutine (DetachBogie ());
	}

	public IEnumerator MoveToTarget ( Vector3 target)
	{

		if (Vector3.Distance (transform.position, target) > Mathf.Min(minDistance,0.0001f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			//			yield return new WaitForSeconds(1f);
			StartCoroutine (MoveToTarget (target));
		} else {
			transform.position = target;

		}


	}
}
