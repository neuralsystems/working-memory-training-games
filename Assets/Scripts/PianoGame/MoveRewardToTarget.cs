using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRewardToTarget : MonoBehaviour {

	Transform  target ,source;

	// Use this for initialization
	void Start () {
		this.transform.rotation = Quaternion.AngleAxis(SceneVariables.INITIAL_ANGLE, Vector3.forward);
		if (source == null) {
			source = this.gameObject.transform;
		}
		if (target == null) {
			target = GameObject.Find ("Target").transform;
		}
		StartCoroutine(DelayBeforeStart());
	}
		
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator DelayBeforeStart(){
		yield return new WaitForSeconds (SceneVariables.DELAY_TO_START);
		MoveObjectToTarget ();
	}
	public void MoveObjectToTarget(){
		source.position = Vector3.MoveTowards (source.position, target.position, SceneVariables.SPEED * Time.deltaTime);
		if (source.position == target.position) {
			target.position = new Vector3 (target.position.x + SceneVariables.stepSize, target.position.y, target.position.z);
			SceneVariables.IS_READY = true;
		} else {
			StartCoroutine(DelayBeforeMove ());

		}

	}
	IEnumerator DelayBeforeMove(){
		yield return new WaitForSeconds (SceneVariables.DELAY_TO_MOVE);
		MoveObjectToTarget ();
	}
}
