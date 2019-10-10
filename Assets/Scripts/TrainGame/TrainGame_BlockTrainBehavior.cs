using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGame_BlockTrainBehavior : MonoBehaviour {

	Vector3 original_position;
	Vector3 velocity = Vector3.zero; 
	float smoothTime = .5f, minDistance = 0.1f;
	// Use this for initialization
	void Start () {
		original_position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator MoveToTarget (Vector3 target)
	{

		while (Vector3.Distance (transform.position, target) > Mathf.Min(minDistance,0.1f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		} 
		transform.position = target;
	}

	public IEnumerator BlockView(){
		var block_position = Shared_ScriptForGeneralFunctions.GetPointOnScreen (.5f, 1f);
		var target = original_position;
		target.x = block_position.x;
		yield return StartCoroutine (MoveToTarget (target));
		var go = GameObject.FindGameObjectsWithTag (TrainGame_SceneVariables.BOGIE_TAG);
		foreach (var g in go) {
			StartCoroutine (g.GetComponent<TrainGame_BogieBehavior> ().SetTouch (true));
		}
	}

	public IEnumerator UnBlockView(){
		var block_position = Shared_ScriptForGeneralFunctions.GetPointOnScreen (2.5f, 1f);
		var target = original_position;
		target.x = block_position.x;
		yield return StartCoroutine (MoveToTarget (target));
	}

}
