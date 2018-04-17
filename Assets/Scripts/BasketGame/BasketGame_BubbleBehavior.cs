using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketGame_BubbleBehavior : MonoBehaviour {

	float smoothTime = 0.1f;
	Vector3 velocity = Vector3.zero;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public IEnumerator MoveTowardsFruit(Transform fruitTransform){
		if (Vector3.Distance (transform.position, fruitTransform.position) > 0.001f) {
			transform.position = Vector3.SmoothDamp (transform.position, fruitTransform.position, ref velocity, smoothTime);
//			yield return new WaitForSeconds(1f);
			yield return null;
			StartCoroutine ( MoveTowardsFruit(fruitTransform));
		} else {
			transform.position = fruitTransform.position;
			transform.parent = fruitTransform;
			StartCoroutine(fruitTransform.gameObject.GetComponent<FruitBehavior> ().AfterTrappedinBubble ());
		}
	}


	public void MoveBubble (Transform target){
		
		StartCoroutine ( MoveTowardsFruit(target));
	}
}
