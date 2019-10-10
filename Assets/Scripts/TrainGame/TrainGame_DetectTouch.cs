using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGame_DetectTouch : MonoBehaviour {

	public AudioClip TouchSound;
	public bool shouldTouch = false;
	// Use this for initialization
	void Start () {
//		shouldTouch = GetComponent<BoxCollider2D> ().enabled;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1 )
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPos = new Vector2(wp.x, wp.y);
			if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
			{
				if (Camera.main.GetComponent<AudioSource> ()) {
					Camera.main.GetComponent<AudioSource> ().PlayOneShot (TouchSound);
				}
				if (gameObject.tag == TrainGame_SceneVariables.BOGIE_TAG ) {
					if (shouldTouch) {
						GetComponent<TrainGame_BogieBehavior> ().OnMouseDown ();
					} else {
						GetComponentInChildren<TrainGame_KeyLockScript> ().ZoomInOut();
					}
				} else if (gameObject.tag == TrainGame_SceneVariables.KEYLOCK_TAG) {
					GetComponentInParent<TrainGame_BogieBehavior> ().OnMouseDown ();
				} else if (gameObject.tag == TrainGame_SceneVariables.COUNTER_SHAPE_OPTION_TAG) {
					GetComponent<TrainGame_CounterShapeScript> ().OnMouseDown ();
				}
			}
		}
	}

	public void SetTouch( bool value){
		Debug.Log ("Set Values with "+ value);
		shouldTouch = value;
//		GetComponent<TrainGame_DetectTouch> ().enabled = value;
		GetComponent<BoxCollider2D> ().enabled = value;
		GetComponent<Shared_AdjustColliderProperties> ().AdjustCollidersize ();
	}


			
}
