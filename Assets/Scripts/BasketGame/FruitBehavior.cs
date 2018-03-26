using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBehavior : MonoBehaviour {

	// Use this for initialization
	public int speed = 13;
	public float lowerLimit = -10f;
	public string fruitName;
	public string fruitPath;
	public Transform extrabubble, pivot;
	public SpriteRenderer sprite;

	public Color spriteColor = Color.white;



	public static float fadeInTime = 1.5f;
	public static float fadeOutTime = 1.5f;
	public static float delayToFadeOut = 2f;
	public static float delayToFadeIn = 2f;
	public static float x_max = 20f;
	public static float x_min= -20f;
	public string ps = "BubbleBurst";

	void Start () {

		StartCoroutine (FadeCycle());

		fruitPath = Camera.main.GetComponent<BasketGame_SceneVariables>().GetPath ();
		fruitName = Camera.main.GetComponent<BasketGame_SceneVariables>().GetColoredFruit ();
		GetComponent<SpriteRenderer> ().sprite = Resources.Load (BasketGame_SceneVariables.Game_Name+ "/" + Camera.main.GetComponent<BasketGame_SceneVariables>().GetFruitFolderName () + fruitName, typeof(Sprite)) as Sprite;
		int x = Random.Range (1, 3);
//		GameObject.Find (ps).GetComponent<ParticleSystem> ().Play ();
//		extrabubble = GameObject.Find ("Pivot");
//		iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath("FruitPath2"),"time",5));
//		GetComponent<Rigidbody2D> ().isKinematic = false;
//		GetComponent<Rigidbody> ().detectCollisions = false;
	}
	
	// Update is called once per frame
	void Update () {
		DestroyWhenOutofLimit ();
	}


	void DestroyWhenOutofLimit(){
		if (transform.position.y <= lowerLimit || transform.position.x >= x_max || transform.position.x <= x_min) {
			OnComplete ();
		}
	}

	public void OnComplete(){
		Debug.Log ("completed the fruit life cycle making a new one");
		StartCoroutine(Camera.main.GetComponent<BasketGame_GameManager> ().MakeFruit ());
		Destroy (gameObject);
	}

	public IEnumerator Fall(){
//		transform.position = move
	  transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
		yield return new WaitForSeconds (BasketGame_SceneVariables.waitTime);
		StartCoroutine(Fall ());
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

		iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath(fruitPath),"speed",speed,"easetype","linear","oncomplete","OnComplete"));
		GetComponent<BasketGame_DetectTouch>().enabled = true;
		GetComponent<CircleCollider2D> ().enabled = true;
	}

	public void Projectile(){
//			GetComponent<Rigidbody2D>().velocity = new Vector3(10,10,0);
		OnComplete();
	}
}
