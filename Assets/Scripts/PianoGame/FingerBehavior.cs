using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerBehavior : MonoBehaviour {

	public float smoothTime = 0.3F;
	private Vector3 velocity = Vector3.zero;
	Animation anim;
	// Use this for initialization
	void Start () {
//		GetComponent<Animator> ().wrapMode = WrapMode.Once;
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneVariables.IS_USER_MODE) {
			GetComponent<SpriteRenderer> ().enabled = false;
		}else if (!SceneVariables.IS_USER_MODE){
			GetComponent<SpriteRenderer> ().enabled = true;
		}
	}

	public IEnumerator MoveAndPlayAnimation(Vector3 target){
		
		if (Vector3.Distance(transform.position, target) >  SceneVariables.MIN_DISTANCE) {
			Debug.Log ("first if");
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			StartCoroutine (MoveAndPlayAnimation (target));
		} else {
			transform.position = target;
//			Debug.Log (target);
			GetComponent<Animator> ().Play (Camera.main.GetComponent<SceneVariables>().fingerTapAnimation);
		}
	}

	public IEnumerator SetPositionAndPlay(Vector3 target, string keyName){
		transform.position = target;
		Debug.Log("came till here");
		GetComponent<Animator>().SetBool("IsTapping",true);
		AnimatorStateInfo asf = GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0);
//		yield return new WaitForSeconds(asf.normalizedTime);
		yield return null;
		GetComponent<Animator>().SetBool("IsTapping",false);
		var pianoKey = GameObject.Find (keyName);
		pianoKey.GetComponent<OnKeyPress> ().ShowKeySquare ();
	}


}
