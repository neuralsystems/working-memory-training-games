using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoGame_UserInputSquareParentBehavior : MonoBehaviour {

	public float smoothTime = 0.5F,minDistance = 0.01f;
	private Vector3 velocity = Vector3.zero, original_position;
	public bool All_Match;
	// Use this for initialization
	void Start () {
		original_position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator MoveToTarget ( Vector3 target)
	{
		if (Vector3.Distance (transform.position, target) > Mathf.Min(minDistance,0.0001f)) {
			//			Debug.Log ("first if");
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
//			yield return new WaitForSeconds(1f);
			StartCoroutine (MoveToTarget (target));
		} else {
			transform.position = target;
			if (All_Match) {
				var reward_square_parent = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().REWARD_SQUARE_PARENT);
//				var new_position = reward_square_parent.transform.position;
//				new_position.y += reward_square_parent.GetComponent<SpriteRenderer> ().bounds.size.y;
				reward_square_parent.GetComponent<PG_RewardSquareParentBehavior> ().ResetPosition();
				GetComponentInParent<ParticleSystem> ().Play ();
				Camera.main.GetComponent<SceneVariables> ().ShowSquares ();
			}
//			ResetPosition ();

		}

	}
	public void ResetPosition(){
		transform.position = original_position;
	}
}
