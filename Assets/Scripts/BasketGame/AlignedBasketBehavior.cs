using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignedBasketBehavior : MonoBehaviour {

	Vector3 target, velocity = Vector3.zero;
	float smoothTime = 3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MoveToList(){
		target = GameObject.Find (BasketGame_SceneVariables.targetObject).transform.position; 
		StartCoroutine (MoveBasket ());
	}

	public IEnumerator MoveBasket(){
		transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
		yield return new WaitForSeconds (Time.deltaTime);
		if (transform.position != target) {
			StartCoroutine (MoveBasket ());
		}
	}
}
