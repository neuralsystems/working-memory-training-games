using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PG_RewardSquareParentBehavior : MonoBehaviour {

	public float smoothTime = .0050F;
	private Vector3 velocity = Vector3.zero;
	Vector3 original_position;
	Coroutine scroll_movement;
	// Use this for initialization
	void Start () {
		original_position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator MoveToTarget ( Vector3 target)
	{

		if (Vector3.Distance (transform.position, target) > 0.00001f) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
//			yield return new WaitForSeconds(0.5f);
			yield return null;
			StartCoroutine (MoveToTarget (target));
		} else {
			transform.position = target;
//			if (target == original_position) {
//				
//			}
		}


	}

	public void ResetPosition(){
		StartCoroutine(MoveToTarget(original_position));
	}

	public void ScrollRewardSquares(bool direction){
		var target = original_position;
		// right swipe
		if (direction) {
			var last_square_gameobject = transform.GetChild(transform.childCount);
//			if(last_square_gameobject.x > Shared_ScriptForGeneralFunctions.GetPointOnScreen(

		} else {
			
		}
		scroll_movement = StartCoroutine(MoveToTarget(target));
	}

	public void OnMouseDown(){
		StopCoroutine (scroll_movement);
	}
}
