using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketGame_DetectTouch : MonoBehaviour {
	// Use this for initialization
	void Start () {
//		Debug.Log ("in start");
	}
	
	// Update is called once per frame
	void Update () {
		// when the object is Touched


			if (Input.touchCount == 1)
			{
				Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				Vector2 touchPos = new Vector2(wp.x, wp.y);
				if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
				{
					// add the code for execuation  on tap 
//					Destroy(this.gameObject);
					OnMouseDown();

				}
			}
		}
		


	public void OnMouseDown(){
//		Debug.Log ("touched on the bubble");
//		Destroy (this.gameObject);
//		GetComponent<Rigidbody2D>().isKinematic = true;
		gameObject.AddComponent<Rigidbody2D>();
		GetComponent<SpriteRenderer>().sortingLayerName = "Game";
		foreach (Transform child in gameObject.transform) {
			
			Destroy (child.gameObject);
		}
		GetComponent<ParticleSystem> ().Play();
		iTween.Stop (gameObject);
//		StartCoroutine(GetComponent<FruitBehavior> ().Fall ());
	}
}
