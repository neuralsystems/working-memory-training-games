﻿using System.Collections;
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


	public static float fadeInTime = 1.5f;
	public static float fadeOutTime = 1.5f;
	public static float delayToFadeOut = 2f;
	public static float delayToFadeIn = 2f;
	public static float x_max = 20f;
	public static float x_min= -20f;
	public string ps = "BubbleBurst";

	void Start () {
		original_position = transform.position;
		original_size = transform.localScale;
//		StartCoroutine (FadeCycle());
		fruitPath = Camera.main.GetComponent<BasketGame_SceneVariables>().GetPath ();
	}
	
	// Update is called once per frame
	void Update () {
		DestroyWhenOutofLimit ();
	}



	// change this function it is causing abnormal behavior
	void DestroyWhenOutofLimit(){
		if (transform.position.y <= lowerLimit || transform.position.x >= x_max || transform.position.x <= x_min) {
			OnComplete ();
		}
	}

	public void OnComplete(){
		
		StartCoroutine(Camera.main.GetComponent<BasketGame_GameManager> ().MakeFruit ());
//		Destroy (gameObject);
	}


	public void StartFall(){
//		GetComponent<Rigidbody2D> ().isKinematic = false;
		StartCoroutine(Fall());
	}

	public IEnumerator Fall(){
//		transform.position = move
	  	transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
		yield return null;
		GameObject bubble_gameobject = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.bubbleTag)[0];
		if (Mathf.Abs (transform.position.y - bubble_gameobject.transform.position.y) < 0.1f) {
			bubble_gameobject.GetComponent<BasketGame_BubbleBehavior> ().MoveBubble (transform);
		}else{
			StartCoroutine(Fall ());
		}
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
		GetComponent<BasketGame_DetectTouch>().enabled = true;
		GetComponent<CircleCollider2D> ().enabled = true;
	}

	public void Projectile(){
//			GetComponent<Rigidbody2D>().velocity = new Vector3(10,10,0);
		StartCoroutine(MovetoOriginalPosition());
//		OnComplete();
	}

	public IEnumerator AfterTrappedinBubble(){
		var path_speed = 20;
		StartCoroutine(Shared_ScriptForGeneralFunctions.ScaleUp (gameObject, 1f, .3f));
		iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath(fruitPath),"speed",path_speed,"easetype","linear","oncomplete","MovetoOriginalPosition"));
		yield return new WaitForSeconds (1f);
		GetComponent<BasketGame_DetectTouch>().enabled = true;
		GetComponent<CircleCollider2D> ().enabled = true;
	}

	public IEnumerator MovetoOriginalPosition(){
		
		ResetProperties ();
//		GetComponent<Rigidbody2D>().isKinematic = true;

		StartCoroutine(MoveToTarget (original_position));
		yield return StartCoroutine(Shared_ScriptForGeneralFunctions.ScaleDown (this.gameObject, original_size.x, 0.3f));


	}


	void ResetProperties(){
		if (transform.childCount > 0) {
			foreach (Transform child in transform) {
				Destroy (child.gameObject);
			}
		}
		Destroy (GetComponent<Rigidbody2D>());
	}

	public IEnumerator MoveToTarget ( Vector3 target)
	{
		if (Vector3.Distance (transform.position, target) > Mathf.Min(minDistance,0.0001f)) {
			//			Debug.Log ("first if");
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			//			yield return new WaitForSeconds(1f);
			StartCoroutine (MoveToTarget (target));
		} else {
			transform.position = target;
			tag = BasketGame_SceneVariables.hangingFruitTag;
			OnComplete ();
		}


	}
}
