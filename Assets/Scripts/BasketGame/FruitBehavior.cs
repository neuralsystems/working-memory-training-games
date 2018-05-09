using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBehavior : MonoBehaviour {

	// Use this for initialization
	int speed = 2;
	public float lowerLimit = -10f;
	public string fruitName;
	public string fruitPath;
	public Transform extrabubble, pivot;
	public SpriteRenderer sprite;
	public Vector3 original_position, original_size;
	public Color spriteColor = Color.white;
	Vector3 velocity = Vector3.zero; 
	float smoothTime = .5f, minDistance = 0.01f;

	Coroutine fall_from_tree;
	public static float fadeInTime = 1.5f;
	public static float fadeOutTime = 1.5f;
	public static float delayToFadeOut = 2f;
	public static float delayToFadeIn = 2f;
	public static float x_max = 20f;
	public static float x_min= -20f;
	public string ps = "BubbleBurst";
	public GameObject girl;
	public AudioClip FRUIT_FALLING;
	public bool hasCollide = false;
	string original_layer;

	void Start () {
		
		StartCoroutine (FadeCycle());
		fruitPath = Camera.main.GetComponent<BasketGame_SceneVariables>().GetPath ();
	}
	
	// Update is called once per frame
	void Update () {
		DestroyWhenOutofLimit ();
	}

	public void SetUp(){
		original_position = transform.position;
		original_size = transform.localScale;
		original_layer = GetComponent<SpriteRenderer> ().sortingLayerName;
	}

	// change this function it is causing abnormal behavior
	void DestroyWhenOutofLimit(){
		var lower_limit = Shared_ScriptForGeneralFunctions.GetPointOnScreen (-1, -1);
		var upper_limit = Shared_ScriptForGeneralFunctions.GetPointOnScreen (2, 2);
		if (transform.position.y <= lower_limit.y || transform.position.x >= upper_limit.x || transform.position.x <= lower_limit.x) {
			if (tag == BasketGame_SceneVariables.fruitTag) {
				Projectile ();
			}
		}
	}

	public void OnComplete(){
		

		StartCoroutine(Camera.main.GetComponent<BasketGame_GameManager> ().MakeFruit ());
//		GetComponent<FruitBehavior> ().enabled = false;
//		Destroy (gameObject);
	}


	public void StartFall(){
//		GetComponent<Rigidbody2D> ().isKinematic = false;

		fall_from_tree = StartCoroutine(Fall());
	}

	public IEnumerator Fall(){
//		transform.position = move
		GetComponent<AudioSource>().PlayOneShot(FRUIT_FALLING);
		GameObject bubble_gameobject = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.bubbleTag)[0];
		while (Mathf.Abs (transform.position.y - bubble_gameobject.transform.position.y) > 0.1f) {
			transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
			yield return null;

		}
		bubble_gameobject.GetComponent<BasketGame_BubbleBehavior> ().MoveBubble (transform);
		GetComponent<AudioSource> ().Stop ();
	}

	IEnumerator FadeCycle()
	{
		//		Debug.Log ("Called");

		float fade = 0f;
		float startTime = Time.time;
		sprite = GetComponent<SpriteRenderer> ();
		while(fade < 1f )
		{
			fade = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeInTime);
			spriteColor.a = fade;
			sprite.color = spriteColor;
			yield return null;
		}

		//Make sure it's set to exactly 1f
//		fade = 1f;
//		spriteColor.a = fade;
//		sprite.color = spriteColor;
//		yield return new WaitForSeconds(delayToFadeOut);
//
//		startTime = Time.time;
//		while(fade > 0f )
//		{
//			fade = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeOutTime);
//			spriteColor.a = fade;
//			sprite.color = spriteColor;
//			yield return null;
//		}

//		iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath(fruitPath),"speed",speed,"easetype","linear","oncomplete","OnComplete"));
//		GetComponent<BasketGame_DetectTouch>().enabled = true;
//		GetComponent<CircleCollider2D> ().enabled = true;
//		SetTouch(true);
	}

	public void Projectile(){
//			GetComponent<Rigidbody2D>().velocity = new Vector3(10,10,0);
		Camera.main.GetComponent<BasketGame_GameManager>().IncreaseErrorCount();
		StartCoroutine(MovetoOriginalPosition());
//		OnComplete();
	}

	public IEnumerator AfterTrappedinBubble(){
		
		var path_speed = 20;
		GetComponent<BasketGame_DetectTouch>().SetBoxCollider();
		StartCoroutine(Shared_ScriptForGeneralFunctions.ScaleUp (gameObject, 1f, .3f));
		iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath(fruitPath),"speed",path_speed,"easetype","linear","oncomplete","Projectile"));
		yield return new WaitForSeconds (1f);
		SetTouch (true);
	}

	public IEnumerator MovetoOriginalPosition(){
		
		ResetProperties ();
		StartCoroutine(MoveToTarget (original_position));
		yield return StartCoroutine(Shared_ScriptForGeneralFunctions.ScaleDown (this.gameObject, original_size.x, 0.3f));


	}


	public void ResetProperties(){
		if (hasCollide == true) {
			hasCollide = false;
		}
//		transform.parent = null;
//		transform.localScale = original_size;
		if (transform.childCount > 0) {
			foreach (Transform child in transform) {
				Destroy (child.gameObject);
			}
		}
		GetComponent<SpriteRenderer> ().sortingLayerName = original_layer;
		Destroy (GetComponent<Rigidbody2D>());
		SetTouch (false);
	}

	public IEnumerator MoveToTarget ( Vector3 target)
	{
		while(Vector3.Distance (transform.position, target) > Mathf.Min(minDistance,0.0001f)) {
			//			Debug.Log ("first if");
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			//			yield return new WaitForSeconds(1f);
//			StartCoroutine (MoveToTarget (target));
		} 
//		else {
		transform.position = target;
		if (transform.position == original_position) {
			tag = BasketGame_SceneVariables.hangingFruitTag;

		}
		OnComplete ();
//		}


	}


	void SetTouch(bool value){
		GetComponent<BasketGame_DetectTouch>().enabled = value;
		GetComponent<CircleCollider2D> ().enabled = value;
	}




	public bool HasCollided(){
		return hasCollide;
	}

	public IEnumerator EatFruit(){
		Debug.Log ("Eating fruits");
		transform.parent = null;
		girl = GameObject.Find ("Girl");
		var target = girl.transform.position;
		while(Vector3.Distance (transform.position, target) > Mathf.Min(minDistance,0.0001f)) {
			//			Debug.Log ("first if");
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
		}
		Destroy (gameObject);
	}
}
