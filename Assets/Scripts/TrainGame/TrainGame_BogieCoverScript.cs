using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGame_BogieCoverScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	IEnumerator MoveToTarget (Vector3 target)
	{
		Vector3 velocity = Vector3.zero; 
		float smoothTime = .5f, minDistance = 0.01f;
		while (Vector3.Distance (transform.position, target) > Mathf.Min(minDistance,0.1f)) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		} 
		transform.position = target;
	}

	public IEnumerator BlockTarget(GameObject target){
		GetComponent<SpriteRenderer> ().enabled = true;
		yield return MoveToTarget (target.transform.position);
		transform.parent = target.transform;
		transform.position = target.transform.position;
		target.GetComponent<TrainGame_DetectTouch> ().SetTouch (true);
	}
}
