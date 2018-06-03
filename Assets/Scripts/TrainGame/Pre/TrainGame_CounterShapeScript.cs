using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TrainGame_CounterShapeScript : MonoBehaviour {

//	bool detectTouch = false;
	Vector3 velocity = Vector3.zero;
	public Vector3 original_position; 
	float minDistance = 0.1f;
	static bool Repeat = false;
    string original_tag;
    string hexForWrong = "#F80C1AFF";
	string hexForNeutral = "#330CFF13";
	string  hexForRight = "#72FF20FB";
	public AudioClip connect_sound;
	void Awake(){
		original_position = transform.position;
        original_tag = tag;
	}
	// Use this for initialization
	void Start () {
		
	}

	public void SetUp(Sprite shape){
		
		GetComponent<SpriteRenderer> ().sprite = shape;
		transform.position = original_position;	
		SetTouchAndScale (true, true, true,tag);
		Repeat = false;
		foreach (Transform chi in transform) {
			chi.gameObject.GetComponent<TrainGame_CircleMaskScript> ().SetColorAndVisibility (hexForNeutral,false);
		}
	}
	
	public void OnMouseDown(){
        tag = TrainGame_SceneVariables.SELECTED_SHAPE_TAG;
		StartCoroutine(MoveToCounter ());
	}

	IEnumerator MoveToCounter(){
		var target_go = GameObject.Find ("SampleShape");
		var target= target_go.transform.position;
		SetTouchAndScale(false,false,false ,original_tag);
		var is_correct = GetComponent<SpriteRenderer> ().sprite.name == "Correct";
        var smoothTime = .1f;
        if (is_correct)
        {
            smoothTime *= 5;
        }
		yield return StartCoroutine (MoveToTarget (target, is_correct, smoothTime));
		var sound_manager_go = GameObject.Find ("SoundManager");
		float extra_wait	= .2f;
		if (!is_correct) {
			yield return new WaitForSeconds (sound_manager_go.GetComponent<SoundManager_Script> ().PlaySadSound ());
			yield return new WaitForSeconds (1f);
			ResetPosition ();
			Camera.main.GetComponent<TrainGame_PreGameManager> ().ResetCorrect ();
			Repeat = true;
		} else {
			yield return new WaitForSeconds(sound_manager_go.GetComponent<SoundManager_Script>().PlaySound(connect_sound) + extra_wait);
			yield return new WaitForSeconds (sound_manager_go.GetComponent<SoundManager_Script> ().PlayHappySound () + extra_wait);
			if (!Repeat) {
				Camera.main.GetComponent<TrainGame_PreGameManager> ().IncrementOnCorrectMatch ();
			}
            tag = TrainGame_SceneVariables.COUNTER_SHAPE_OPTION_TAG;
            Camera.main.GetComponent<TrainGame_PreGameManager> ().LoadNext ();
		}
	}


	IEnumerator MoveToTarget(Vector3 target, bool is_correct, float smoothTime){
		while (Vector3.Distance (transform.position, target) > Mathf.Min (minDistance, 0.1f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		}
		transform.position = target;
		if((Vector3.Distance(transform.position,original_position) < 0.01f)){
            tag = original_tag;
			foreach (Transform chi in transform) {
				chi.gameObject.GetComponent<TrainGame_CircleMaskScript> ().SetColorAndVisibility (hexForNeutral,false);
			}
			SetTouchAndScale (true, true,true ,tag);
		} else{
			if (!is_correct) {
				foreach (Transform chi in transform) {
					chi.gameObject.GetComponent<TrainGame_CircleMaskScript> ().SetColorAndVisibility (hexForNeutral, false);
				}
			} else {
				foreach (Transform chi in transform) {
				chi.gameObject.GetComponent<TrainGame_CircleMaskScript> ().SetColorAndVisibility (hexForRight, true);
				}
			}
		}

	}


	void ResetPosition(){
		StartCoroutine(MoveToTarget (original_position, true, .1f));
	}

	void SetTouchAndScale(bool touch, bool scale, bool visible, string object_tag)
    {
		var all_obj = GameObject.FindGameObjectsWithTag (object_tag);
		foreach (var obj in all_obj) {
            obj.GetComponent<SpriteRenderer>().enabled = visible;
			obj.GetComponent<TrainGame_DetectTouch> ().SetTouch (touch); 
			obj.GetComponent<Scalling> ().SetScale (scale);
		}
	}
}
