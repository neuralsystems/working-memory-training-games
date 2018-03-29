using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGame_OptionTileCoverBehavior : MonoBehaviour {

	Vector3 velocity = Vector3.zero;	
	float smoothTime = .5f;
	// Use this for initialization
	void Start () {
//		StartCoroutine (AdjustSizeToParent());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator AdjustSizeToParent(){
		transform.localScale = transform.parent.localScale;
		yield return new WaitForSeconds(1f);
		StartCoroutine (AdjustSizeToParent());
	}

	public IEnumerator MoveToCoveTheObject(GameObject parent_gameobject){
		
		if (Vector3.Distance (transform.position, parent_gameobject.transform.position) > CarGame_SceneVariables.MIN_DISTANCE) {
			transform.position = Vector3.SmoothDamp (transform.position, parent_gameobject.transform.position, ref velocity, smoothTime);
			yield return null;
			StartCoroutine (MoveToCoveTheObject (parent_gameobject));
		} else {
			transform.position = parent_gameobject.transform.position;
			transform.parent = parent_gameobject.transform;
			CarGame_SceneVariables.presentCue = true;
//			StartCoroutine(AdjustSizeToParent());
		}
	}
}
