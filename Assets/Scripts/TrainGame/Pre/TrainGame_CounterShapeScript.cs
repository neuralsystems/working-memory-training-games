using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TrainGame_CounterShapeScript : MonoBehaviour {

//	bool detectTouch = false;
	Vector3 velocity = Vector3.zero;
	public Vector3 original_position; 
	float smoothTime = 0.5f, minDistance = 0.1f;
	static bool Repeat = false;
	// Use this for initialization
	void Start () {
		original_position = transform.position;	
	}

	public void SetUp(Sprite shape){
		
		GetComponent<SpriteRenderer> ().sprite = shape;
		transform.position = original_position;	
		GetComponent<Scalling> ().SetScale (true);
		GetComponent<TrainGame_DetectTouch> ().SetTouch (true);
	}
	
	public void OnMouseDown(){
		StartCoroutine(MoveToCounter ());
	}

	IEnumerator MoveToCounter(){
		var target_go = GameObject.Find ("SampleShape");
		var target= target_go.transform.position;
//		target.x += GetComponent<SpriteRenderer> ().bounds.size.x/2; 
//		GetComponent<Scalling> ().SetScale (false);
		SetTouchAndScale(false,false, tag);
		var is_correct = GetComponent<SpriteRenderer> ().sprite.name == "Correct";
		yield return StartCoroutine (MoveToTarget (target, is_correct));
		var sound_manager_go = GameObject.Find ("SoundManager");
		float extra_wait	= 1f;
		if (!is_correct) {
//			yield return new WaitForSeconds (1f);
			foreach (Transform comp in target_go.transform) {
				comp.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			}
//			comp.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (sound_manager_go.GetComponent<SoundManager_Script> ().PlaySadSound () + extra_wait);
			yield return new WaitForSeconds (1f);
			ResetPosition ();
			foreach (Transform comp in target_go.transform) {
				comp.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
			Camera.main.GetComponent<TrainGame_PreGameManager> ().ResetCorrect ();
			Repeat = true;
		} else {
			
			yield return new WaitForSeconds (sound_manager_go.GetComponent<SoundManager_Script> ().PlayHappySound () + extra_wait);
//			yield return new WaitForSeconds (2f);
			if (!Repeat) {
				Camera.main.GetComponent<TrainGame_PreGameManager> ().IncrementOnCorrectMatch ();
			}
			Camera.main.GetComponent<TrainGame_PreGameManager> ().LoadNext ();
		}
	}


	IEnumerator MoveToTarget(Vector3 target, bool is_correct){
//		var target = target_go.transform.position;
		while (Vector3.Distance (transform.position, target) > Mathf.Min (minDistance, 0.1f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		}
		transform.position = target;
		if (!is_correct) {
			foreach (Transform chi in transform) {
				chi.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			}
		} else if ((Vector3.Distance(transform.position,original_position) < 0.01f) ) {
			foreach (Transform chi in transform) {
				chi.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
			SetTouchAndScale(true,true, tag);
		}
	}


	void ResetPosition(){
		StartCoroutine(MoveToTarget (original_position, true));
	}

	void SetTouchAndScale(bool touch, bool scale, string object_tag){
		var all_obj = GameObject.FindGameObjectsWithTag (object_tag);
		foreach (var obj in all_obj) {
			obj.GetComponent<TrainGame_DetectTouch> ().SetTouch (touch); 
			obj.GetComponent<Scalling> ().SetScale (scale);
		}
	}
}
