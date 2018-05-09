using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BasketBehavior : MonoBehaviour {

	public int capacity, currentIns = 0;
	public string basketName ;
	public float lowerBound ;
	public float rotationHeight ;
	public Vector3 velocity = Vector3.zero;
	public float smoothTime = 0.2f;
	public float  numOfRounds =  2f, inTime = 2f;
	public Vector3 originalPosition;
	public float num_of_fruits;
	public float reduce_height_by;

	// Use this for initialization
	void Start () {
//		capacity = Camera.main.GetComponent<Scenevariables>().basketCapacity;
//		originalPosition = transform.position;


	}

	public string GetBasketName(){
		return basketName;
	}

	public void SetUpBasketProperties(string name, Vector3 scale, Vector3 basket_pos, string basket_tag, int bas_capacity) {
		
		foreach (Transform child in transform) {
			child.gameObject.transform.localScale = new Vector3 (.4f, .4f, 1f);
		}
		num_of_fruits = 3f;
		tag = basket_tag;
		GetComponent<BasketBehavior> ().basketName = name;
		transform.localScale = scale;
		transform.position = basket_pos;
		rotationHeight = GetRotationHeight ();
		lowerBound = Camera.main.GetComponent<BasketGame_SceneVariables> ().GetPointOnScreen (0, 0).y;
		lowerBound -= GetComponent<SpriteRenderer> ().bounds.size.y / 2;
		reduce_height_by = (transform.position.y - lowerBound) / num_of_fruits;
		capacity = bas_capacity;
		Debug.Log(capacity);
	}
	// Update is called once per frame
	void Update () {
		
	}
		 
	float GetRotationHeight(){
		var screenWidth = Camera.main.pixelWidth;
		var screenHeight = Camera.main.pixelHeight;
		return transform.position.y;
//		return Camera.main.ScreenToWorldPoint (new Vector3 (screenWidth, screenHeight*.5f, Camera.main.nearClipPlane)).y;	// return the 50% of the screen height as height of rotation for basket
	}

	void OnCollisionEnter2D(Collision2D collision){
		StartCoroutine(AfterCollision(collision));
	}
	IEnumerator AfterCollision(Collision2D collision){
		yield return null;
		var has = collision.gameObject.GetComponent<FruitBehavior> ().hasCollide;
		collision.gameObject.GetComponent<FruitBehavior> ().hasCollide = true;
		if (collision.gameObject.tag == BasketGame_SceneVariables.fruitTag && (!has)) {
			Debug.Log("Collision with: "+ basketName);
//			if (collision.gameObject.GetComponent<FruitBehavior> ().hasCollide == false) {

			var fruitList = Camera.main.GetComponent<BasketGame_SceneVariables> ().objectColorMap [basketName];

			if (fruitList.Contains (collision.gameObject.GetComponent<FruitBehavior> ().fruitName)) {

				GetComponent<ParticleSystem> ().Play ();
				currentIns += 1;
				//				Debug.Log (basketName + " " + currentIns);
//				transform.GetChild (currentIns - 1).GetComponent<SpriteRenderer> ().sprite = collision.gameObject.GetComponent<SpriteRenderer> ().sprite;
				//				

				collision.gameObject.GetComponent<CircleCollider2D> ().enabled = false;
				GameObject[] gos = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag);

				collision.gameObject.transform.position = transform.GetChild (currentIns - 1).transform.position;
				collision.gameObject.transform.parent = transform.GetChild (currentIns - 1).transform;
				collision.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
				collision.gameObject.tag = BasketGame_SceneVariables.inBasketFruitTag;
				collision.gameObject.GetComponent<FruitBehavior> ().ResetProperties ();
				yield return new WaitForSeconds (2f);
				foreach (GameObject g in gos) {
					//					Debug.Log ("in loop " + g.transform.localPosition.y );
					if (Mathf.Abs (g.transform.position.y - lowerBound) > BasketGame_SceneVariables.minDistance) {
						Debug.Log ("in if");
						var x = g.transform.position;
						Debug.Log (g.transform.localPosition);
						g.transform.position = new Vector3 (x.x, x.y - reduce_height_by, x.z);
						Debug.Log (g.transform.position);

					} 
				}
				originalPosition = transform.position;
				//				Debug.Log ("distance from lower bound " + Mathf.Abs (transform.position.y - lowerBound));
				if (Mathf.Abs (transform.position.y - lowerBound) < BasketGame_SceneVariables.minDistance) {
					//					Destroy (collision.gameObject);
					StartCoroutine (BaksetAnimation ());
				}
				StartCoroutine (Camera.main.GetComponent<BasketGame_GameManager> ().PlayGirlAnimation ());
				if (capacity <= currentIns) {
					tag = BasketGame_SceneVariables.fullBasket;
					//					Camera.main.GetComponent<BasketGame_GameManager> ().BasketFilled();
					if (GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.hangingFruitTag).Length == 0) {
						Camera.main.GetComponent<BasketGame_GameManager> ().FinalAnimation ();
					}
				}

			} else {
				collision.gameObject.GetComponent<FruitBehavior> ().Projectile ();
			} 

//			}
		}


	}
	public bool spaceLeft(){
		return capacity > currentIns;
	}

	public IEnumerator BaksetAnimation(){
//		yield return 
		yield return StartCoroutine (MoveBasket (new Vector3 (transform.position.x, rotationHeight, transform.position.z)));
//		foreach (Transform fruit in transform) {
//			
//		}
	}

	IEnumerator MoveBasket(Vector3 target){
		while ((Vector3.Distance (transform.position, target) > BasketGame_SceneVariables.minDistance) ) {
			//			Debug.Log ("first if");
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
//			StartCoroutine (MoveBasket (target));
		}
//		else {
		transform.position = target;
//			Debug.Log (transform.position.y + " " + rotationHeight + " "+ (transform.position.y - rotationHeight ));
		if (Mathf.Abs (transform.position.y - rotationHeight) < .001f) {
//				Debug.Log ("start rotating the basket..");
			StartCoroutine (RotateMe (Vector3.forward * 355 * numOfRounds, inTime));
		} else {
//			if (tag == BasketGame_SceneVariables.basketTag) {
////					Debug.Log ("called make fruit from a basket");
//
//			}
		}
//		}
	}

	IEnumerator RotateMe(Vector3 byAngles, float inTime) {
		var fromAngle = transform.rotation;
		var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
		for(var t = 0f; t < 1; t += Time.deltaTime/inTime) {
			yield return null;
		} 
		if (GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag).Length != 0) {
			transform.rotation = Quaternion.identity;
			StartCoroutine (MoveBasket (originalPosition));
		} else {
			StartCoroutine (Camera.main.GetComponent<BasketGame_GameManager>().EmptyBasket ());
		}
	}


}
