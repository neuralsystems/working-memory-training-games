using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeOptionCue : MonoBehaviour {

	Vector3 target,target1;
	public float waitTime =.2f;
	public float stepSize = -3f;
	public bool stopMoving = true;
	private Vector3 velocity = Vector3.zero;
	private string afterReachingTarget = "AfterReachingTarget";
	float step;
	// Use this for initialization
	void Start () {
		step = CarGame_SceneVariables.speed * Time.deltaTime;
		target = GameObject.FindGameObjectWithTag (CarGame_SceneVariables.cueTag).transform.position;
//		target1 = GameObject.Find (CarGame_SceneVariables.targetTile).transform.position;
		target1 = GetComponent<ImageEffect>().position_in_parking;
		GetComponent<Scalling> ().SetScale(false);
		GetComponent<CarGame_DetectTouch> ().SetTouch (false);
		StartCoroutine (MoveToCue (target,afterReachingTarget ));
	}
	
	// Update is called once per frame
	void Update () {
		if (!stopMoving) {
			if (transform.position == target1) {
				stepSize = GetComponent<SpriteRenderer> ().bounds.size.x;
				GameObject.Find (CarGame_SceneVariables.targetTile).transform.position = new Vector3 (target1.x - stepSize, target1.y, target1.z);
				stopMoving = true;
				this.gameObject.tag = CarGame_SceneVariables.trophyTag; 
				CarGame_SceneVariables sc = Camera.main.GetComponent<CarGame_SceneVariables> ();
				sc.ResetorRestart ();
			
			} else {
				transform.position = Vector3.SmoothDamp (transform.position, target1,ref velocity ,CarGame_SceneVariables.speed * Time.deltaTime);
			}
		}
	}



//	IEnumerator MergeOptionAndCueSmooth(Vector3 target){
//
//
//		transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity,step);
//		if (transform.position != target) {
//
//			yield return null;
//			StartCoroutine (MergeOptionAndCueSmooth (target));
//		} else if (transform.position == target) {
//
//
//		}
//
//	}
	public IEnumerator MoveToCue(Vector3 target, string afterCompletionCall){

		if (Vector3.Distance(transform.position ,target) > CarGame_SceneVariables.MIN_DISTANCE) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, step);
			yield return null;
			StartCoroutine (MoveToCue (target, afterCompletionCall));
		} else { 
			transform.position = target;
			Invoke(afterCompletionCall,GetComponent<ParticleSystem> ().main.duration);
		}

	}

	void AfterReachingTarget(){
		GameObject cueTile = GameObject.FindGameObjectWithTag(CarGame_SceneVariables.cueTag);
		Destroy (cueTile);
		GetComponent<ParticleSystem> ().Play ();
		StartCoroutine (WaitBeforeMovingToTarget ());

	}

	void AfterReachingSource(){
		
	}

	IEnumerator WaitBeforeMovingToTarget(){
		if (GetComponent<ParticleSystem> ().isPlaying) {
//			yield return new WaitForSeconds (.5f);
			yield return null;
			StartCoroutine (WaitBeforeMovingToTarget ());
		} else {
			GetComponent<ScallingObject> ().enabled = true;
			stopMoving = false;
		}
	}
}
