using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TrainGame_BogieBehavior : MonoBehaviour {
	Vector3 velocity = Vector3.zero;
	public Vector3 original_position; 
	float smoothTime = .5f, minDistance = 0.1f;
	string original_tag ;
	public GameObject BLOCK_TRAIN;
	// Use this for initialization
	void Start () {
		// initial tag to be set
		original_tag = TrainGame_SceneVariables.ATTACHED_BOGIE_TAG;
		original_position = transform.position;
		Debug.Log (original_position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public IEnumerator MoveToTarget ( Vector3 target)
	{
		
		while(Vector3.Distance (transform.position, target) > Mathf.Min(minDistance,0.1f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		}
		transform.position = target;
		if (target == original_position) {
			tag = original_tag;
			var unattached_bogies = GameObject.FindGameObjectsWithTag (TrainGame_SceneVariables.BOGIE_TAG);
			if (unattached_bogies.Length == 0) {
				StartCoroutine(Camera.main.GetComponent<TrainGame_GameManager> ().BlockAndRandomize ());

			}
		}

	}

	public void AttachBack(){
		StartCoroutine (MoveToTarget (original_position));
	}


	public void OnMouseDown(){
		var target = Shared_ScriptForGeneralFunctions.GetPointOnScreen (1.2f, 0f);
		target.y = transform.position.y;
		StartCoroutine (ReAttachBogie ());
	}


	public IEnumerator SetTouch(bool value){
		GetComponent<BoxCollider2D> ().enabled = value;
		GetComponent<TrainGame_DetectTouch> ().enabled = value;
		foreach (Transform child in transform) {
			child.GetComponent<TrainGame_DetectTouch> ().enabled = value;
			child.GetComponent<BoxCollider2D> ().enabled = value;
		}
		yield return null;
	}

	public IEnumerator ReAttachBogie(){
		foreach (Transform bogie in transform.parent) {
			if (bogie.tag == TrainGame_SceneVariables.BOGIE_TAG) {
				StartCoroutine (bogie.GetComponent<TrainGame_BogieBehavior> ().SetTouch (false));
			}
		}
		var target = Shared_ScriptForGeneralFunctions.GetPointOnScreen (1.2f, 0f);
		target.y = transform.position.y;
//		StartCoroutine (MoveToTarget (target));
		while (Vector3.Distance (transform.position, target) > Mathf.Min (minDistance, 0.1f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		}
		transform.position = new Vector3 (transform.position.x, original_position.y, transform.position.z);
		target = original_position;
		while (Vector3.Distance (transform.position, target) > Mathf.Min (minDistance, 0.1f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		}
		// determine whether it is the correct match or not;
		if(!correctMatch()){
//			 code for wrong match
//			iTween.ShakePosition(gameObject, Vector3(1f,1f,1f), 1f);

		} else{
			// code if it is a correct match
			tag = original_tag;
			yield return null;
			var unattached_bogies = GameObject.FindGameObjectsWithTag (TrainGame_SceneVariables.BOGIE_TAG);
			if (unattached_bogies.Length == 0) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			} else{
				foreach (var bogie in unattached_bogies) {
//				if (bogie.tag == TrainGame_SceneVariables.BOGIE_TAG) {
					StartCoroutine (bogie.GetComponent<TrainGame_BogieBehavior> ().SetTouch (true));
//				}
				}
			}
		}
	}
	public IEnumerator MoveToTargetAndSet(Vector3 target, bool TouchValue, string tag_value){
		tag = tag_value;
		yield return StartCoroutine(MoveToTarget (target));
		yield return new WaitForSeconds (2f);
		StartCoroutine(SetTouch (TouchValue));

	}

	bool correctMatch(){
		return 1 == 1;
	}

	public bool OnOriginalPosition(){
		return transform.position == original_position;
	}
}
