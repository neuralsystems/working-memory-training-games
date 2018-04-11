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

		rotationHeight = GetRotationHeight ();
		lowerBound = Camera.main.GetComponent<BasketGame_SceneVariables> ().GetPointOnScreen (0, 0).y;
		Debug.Log ("lower bound is: "+ lowerBound);
		lowerBound -= GetComponent<SpriteRenderer> ().bounds.size.y / 2;
		Debug.Log ("lower bound is: "+ lowerBound);
		foreach (Transform child in transform) {
			child.gameObject.transform.localScale = new Vector3 (.4f, .4f, 1f);
		}
		num_of_fruits = 3f;
		reduce_height_by = (transform.position.y - lowerBound) / num_of_fruits;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
		 
	float GetRotationHeight(){
		var screenWidth = Camera.main.pixelWidth;
		var screenHeight = Camera.main.pixelHeight;
		return Camera.main.ScreenToWorldPoint (new Vector3 (screenWidth, screenHeight*.5f, Camera.main.nearClipPlane)).y;	// return the 50% of the screen height as height of rotation for basket
	}

	void OnCollisionEnter2D(Collision2D collision){
//		Debug.Log ("till this point ");
//		Debug.Log (collision.gameObject.GetComponent<FruitBehavior>().fruitName);
		if (collision.gameObject.tag == BasketGame_SceneVariables.fruitTag) {
			Debug.Log (basketName);
			var fruitList = Camera.main.GetComponent<BasketGame_SceneVariables> ().objectColorMap [basketName];

			if (fruitList.Contains (collision.gameObject.GetComponent<FruitBehavior> ().fruitName)) {


				GetComponent<ParticleSystem> ().Play ();
				currentIns += 1;
//				Debug.Log (basketName + " " + currentIns);
				transform.GetChild (currentIns - 1).GetComponent<SpriteRenderer> ().sprite = collision.gameObject.GetComponent<SpriteRenderer> ().sprite;
//				

				collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				GameObject[] gos = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag);

				Destroy(collision.gameObject);
				foreach (GameObject g in gos) {
//					Debug.Log ("in loop " + g.transform.localPosition.y );
					if (Mathf.Abs(g.transform.position.y - lowerBound) > BasketGame_SceneVariables.minDistance) {
						Debug.Log ("in if");
						var x = g.transform.position;
						Debug.Log (g.transform.localPosition);
						g.transform.position = new Vector3 (x.x, x.y - reduce_height_by , x.z);
						Debug.Log (g.transform.position);

					} 
				}
				originalPosition = transform.position;
//				Debug.Log ("distance from lower bound " + Mathf.Abs (transform.position.y - lowerBound));
				if (Mathf.Abs(transform.position.y - lowerBound) < BasketGame_SceneVariables.minDistance) {
					Destroy (collision.gameObject);
					var BasketLauncher = GameObject.Find("RocketLaunch");
					BasketLauncher.transform.position = transform.position;
					BasketLauncher.transform.parent = transform;
					BasketLauncher.GetComponent<ParticleSystem>().Play();
					BaksetAnimation ();
				} else {
					collision.gameObject.GetComponent<FruitBehavior> ().OnComplete ();
				}

			} else {
				collision.gameObject.GetComponent<FruitBehavior> ().Projectile ();
			}
			if (capacity <= currentIns) {
				tag = BasketGame_SceneVariables.fullBasket;
				Camera.main.GetComponent<BasketGame_GameManager> ().BasketFilled();

			}
		}


	}
	public bool spaceLeft(){
		return capacity > currentIns;
	}

	public void BaksetAnimation(){
		StartCoroutine (MoveBasket (new Vector3 (transform.position.x, rotationHeight, transform.position.z)));
	}

	IEnumerator MoveBasket(Vector3 target){
		if ((Vector3.Distance (transform.position, target) > BasketGame_SceneVariables.minDistance) && spaceLeft()) {
			//			Debug.Log ("first if");
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			StartCoroutine (MoveBasket (target));
		} else {
			transform.position = target;
//			Debug.Log (transform.position.y + " " + rotationHeight + " "+ (transform.position.y - rotationHeight ));
			if (Mathf.Abs (transform.position.y - rotationHeight) < .001f) {
				Debug.Log ("start rotating the basket..");
				StartCoroutine (RotateMe (Vector3.forward * 355 * numOfRounds, inTime));
			} else {
				if (tag == BasketGame_SceneVariables.basketTag) {
					Debug.Log ("called make fruit from a basket");
					StartCoroutine (Camera.main.GetComponent<BasketGame_GameManager> ().MakeFruit ());
				}
			}
		}
	}

	IEnumerator RotateMe(Vector3 byAngles, float inTime) {
		var fromAngle = transform.rotation;
		var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
		for(var t = 0f; t < 1; t += Time.deltaTime/inTime) {
//			Debug.Log ("rotating the basket..");
//			transform.rotation = Quaternion.Lerp(fromAngle, Quaternion.Euler(fromAngle.eulerAngles +byAngles), t);
			transform.Rotate (0,0,180*Time.deltaTime);
			yield return null;
		}
		transform.rotation = Quaternion.identity;
		StartCoroutine (MoveBasket (originalPosition));
	}
}
