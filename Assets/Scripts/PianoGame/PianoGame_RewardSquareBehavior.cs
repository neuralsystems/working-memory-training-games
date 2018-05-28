using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
public class PianoGame_RewardSquareBehavior : MonoBehaviour {

	public float smoothTime = .0050F;
	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public IEnumerator MoveToTarget ( Vector3 target)
	{
		
		if (Vector3.Distance (transform.position, target) > 0.01f) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			StartCoroutine (MoveToTarget (target));
		} else {
			transform.position = target;
			Debug.Log (smoothTime);
			if (GetComponent<Animator> ()) {
				Stand ();
			}
		}


	}

	public void SetVisibility(bool value){
		GetComponent<SpriteRenderer> ().enabled = value;
//		GetComponent<Outline> ().eraseRenderer = value;
	}

	public void Stand(){
		GetComponent<Animator> ().SetInteger ("WalkorStand", 1);
	}
}
