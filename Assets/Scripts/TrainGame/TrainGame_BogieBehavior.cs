using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TrainGame_BogieBehavior : MonoBehaviour {
	Vector3 velocity = Vector3.zero;
	public Vector3 original_position; 
	float smoothTime = 0.3f, minDistance = 0.1f;
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


	public IEnumerator MoveToTarget (Vector3 target, bool randmoize = false)
	{
		
		while(Vector3.Distance (transform.position, target) > Mathf.Min(minDistance,0.1f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		}
		transform.position = target;
		if (target == original_position) {
			tag = original_tag;
			GetComponentInChildren<ParticleSystem> ().Play ();
			yield return new WaitForSeconds (GetComponentInChildren<ParticleSystem> ().main.duration);
		}
		if (randmoize) {
			var attached_bogies = GameObject.FindGameObjectsWithTag (TrainGame_SceneVariables.ATTACHED_BOGIE_TAG);
			if (attached_bogies.Length == 0) {
				StartCoroutine(Camera.main.GetComponent<TrainGame_GameManager> ().BlockAndRandomize ());

			}
		}

	}

	public void AttachBack(bool randomize = false){
		StartCoroutine (MoveToTarget (original_position, randomize));
	}


	public void OnMouseDown(){
		var target = Shared_ScriptForGeneralFunctions.GetPointOnScreen (1.2f, 0f);
		target.y = transform.position.y;
		var top = GetComponentInParent<TrainGame_Engine_Behavior> ().GetTopPosition ();
		Debug.Log ("Matched? = "+ (Vector3.Distance(top, original_position) <= 0.1f) + " "+top + " "+ original_position+ " " + Vector3.Distance(top, original_position));
		StartCoroutine (ReAttachBogie ((Vector3.Distance(top, original_position) <= 0.1f), top));

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

	public IEnumerator ReAttachBogie(bool matched, Vector3 new_position){
		var unattached_bogies = GameObject.FindGameObjectsWithTag (TrainGame_SceneVariables.BOGIE_TAG);
		foreach (var bogie in unattached_bogies) {
			//				if (bogie.tag == TrainGame_SceneVariables.BOGIE_TAG) {
			StartCoroutine (bogie.GetComponent<TrainGame_BogieBehavior> ().SetTouch (true));
			//				}
		}
		var _random_position = transform.position;
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
		target = new_position;
		while (Vector3.Distance (transform.position, target) > Mathf.Min (minDistance, 0.1f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		}
		transform.position = target;
		// determine whether it is the correct match or not;
		if(!matched){
			Camera.main.GetComponent<TrainGame_GameManager> ().ErrorDetected ();
			var shaketime = .1f;
			iTween.ShakePosition (this.gameObject, iTween.Hash("x", 1f,"islocal", false, "time",shaketime ));
			yield return new WaitForSeconds(shaketime);
			yield return StartCoroutine(MoveToTargetAndSet(_random_position, true, TrainGame_SceneVariables.BOGIE_TAG));
		} else{
			// code if it is a correct match
			tag = original_tag;
			yield return null;
			GetComponent<AudioSource> ().Play ();
			GetComponentInChildren<ParticleSystem> ().Emit (500);
			GetComponentInParent<TrainGame_Engine_Behavior> ().RemoveFromTop ();
			yield return new WaitForSeconds(2.0f * GetComponentInChildren<ParticleSystem> ().main.duration);
		}
		Next ();
	}

	void Next(){
		var unattached_bogies = GameObject.FindGameObjectsWithTag (TrainGame_SceneVariables.BOGIE_TAG);
		if (unattached_bogies.Length == 0) {
			Camera.main.GetComponent<TrainGame_GameManager>().FinalAnimation();
		} else{
			foreach (var bogie in unattached_bogies) {
				//				if (bogie.tag == TrainGame_SceneVariables.BOGIE_TAG) {
				StartCoroutine (bogie.GetComponent<TrainGame_BogieBehavior> ().SetTouch (true));
				//				}
			}
		}
	}
	public IEnumerator MoveToTargetAndSet(Vector3 target, bool TouchValue, string tag_value){
		transform.position = new Vector3(transform.position.x, target.y, transform.position.z);
		tag = tag_value;
		yield return StartCoroutine(MoveToTarget (target));
		yield return new WaitForSeconds (2f);
		StartCoroutine(SetTouch (TouchValue));

	}

	public bool OnOriginalPosition(){
		return transform.position == original_position;
	}
}
