using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeBubble : MonoBehaviour {

	Vector3 target = new Vector3(-7.6f, 6.0f,0f), velocity = Vector3.zero;
	float smoothTime = 2f, minSize = 0, maxSize =.3f ;
	// Use this for initialization
	void Start () {
		float x = Random.Range (minSize, maxSize);
//		transform.position = Scenevariables.initVector;
		transform.localScale = new Vector3(x,x,0);
//		StartCoroutine (MoveBubble ());
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
		if (transform.position == target) {
			Destroy (gameObject);
		}
	}

}
