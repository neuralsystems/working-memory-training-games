using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEffect : MonoBehaviour {

	public SpriteRenderer sprite;
	public Sprite newSprite,oldSprite;
	SpriteRenderer spriteRenderer;
	public Color spriteColor = Color.white;
	public Vector3 position_in_parking, position_in_game;


	public static float fadeInTime = 1.5f;
	public static float fadeOutTime = 1.5f;
	public static float delayToFadeOut = 2f;
	public static float delayToFadeIn = 2f;


	//	 maximum allowed time ideally they will very with the level 
	public static float maxFadeInTime = 2.5f;
	public static float maxFadeOutTime = 2.5f;
	public static float maxDelayToFadeOut = 3f;
	public static float maxDelayToFadeIn = 3f;

	//	minimum allowed time ideally they will very with the level
	public static float minFadeInTime = 0.5f;
	public static float minFadeOutTime = 0.5f;
	public static float minDelayToFadeOut = 1f;
	public static float minDelayToFadeIn = 1f;

	public bool secondTime = false;
	bool calledOnce = false;
	public static int consecutiveCorrect = 0,consecutiveIncorrect = 0; 

	void Awake(){

		newSprite = GameObject.Find (Camera.main.GetComponent<CarGame_SceneVariables>().blockObject).GetComponent<SpriteRenderer> ().sprite;
		oldSprite = GetComponent<SpriteRenderer> ().sprite;

	}

	void Start()
	{
		sprite = this.gameObject.GetComponent<SpriteRenderer> ();
		newSprite = GameObject.Find ("Block").GetComponent<SpriteRenderer> ().sprite;
		oldSprite = GetComponent<SpriteRenderer> ().sprite;
		spriteRenderer = GetComponent<SpriteRenderer> ();
		Initiate ();
//		Initiate ();
	}

	void Update(){
		if (CarGame_SceneVariables.isToggle && !calledOnce) {
//			Initiate ();
			calledOnce = true;
		}

	}

	// this function cause the fade in fade out of the images. This function is called recursively. 
	IEnumerator FadeCycle()
	{
		// touch is disabled while the fade in/out is in play
		GetComponent<CarGame_DetectTouch>().SetTouch(false);
		GetComponent<Scalling> ().SetScale(false);
		GetComponent<SpriteRenderer> ().sprite = oldSprite;
//		Debug.Log ("Called");
		float fade = 0f;
		float startTime;

//		while(true)
//		{
			startTime = Time.time;
			while(fade < 1f && CarGame_SceneVariables.isToggle)
			{
				fade = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeInTime);
				spriteColor.a = fade;
				sprite.color = spriteColor;
				yield return null;
			}
			//Make sure it's set to exactly 1f
			fade = 1f;
			spriteColor.a = fade;
			sprite.color = spriteColor;
			yield return new WaitForSeconds(delayToFadeOut);

			startTime = Time.time;
			while(fade > 0f && CarGame_SceneVariables.isToggle)
			{
				fade = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeOutTime);
				spriteColor.a = fade;
				sprite.color = spriteColor;
				yield return null;
			}

		spriteRenderer.sprite = newSprite;

		startTime = Time.time;
		while(fade < 1f && CarGame_SceneVariables.isToggle)
		{
			fade = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeInTime);
			spriteColor.a = fade;
			sprite.color = spriteColor;
			yield return null;
		}
		//Make sure it's set to exactly 1f
		fade = 1f;
		spriteColor.a = fade;
		sprite.color = spriteColor;
		yield return new WaitForSeconds(delayToFadeOut);
//		GetComponent<DetectTouch>().SetTouch(true);
		CarGame_SceneVariables.presentCue = true;
		Debug.Log ("presentCue " + CarGame_SceneVariables.presentCue);
}

	// this function initiates the toggle effect
	public void Initiate (){
		StartCoroutine("FadeCycle");
	}

	// this function increase the time variables in the fade in fade out showing images to user for a longer duration
	// should be called if user makew consecutive mismatches
	static void IncreaseTime(int consecutiveCorrect){
		
	}

	// this function increase the time variables in the fade in fade out showing images to user for a shorter duration
	// should be called if user makes consecutive matches
	static void DecreaseTime(int consecutiveIncorrect ){
		
	}

	// function to shuffle both the lists. This should be called from external scripts if images are to be shuffled not the ShuffleImages
	public static void ShuffleObjects(){
		Debug.Log ("shuffled");
//		ShuffleImages (leftOptionTileTag);
//		ShuffleImages (rightOptionTileTag);
	}
	// function shuffles the images of objects with ImageTag
	public static void ShuffleImages (string imageTag){

		GameObject[] GameObjects = GameObject.FindGameObjectsWithTag (imageTag);
		List<Sprite> tempImageList = new List<Sprite> ();
		foreach (GameObject go in GameObjects) {
			tempImageList.Add (go.GetComponent<SpriteRenderer> ().sprite);
		}
//		RandomizingArray ra = new RandomizingArray ();
		tempImageList = RandomizingArray.RandomizeSprite (tempImageList.ToArray());
		int i = 0;
		foreach (GameObject go in GameObjects) {
			go.GetComponent<SpriteRenderer> ().sprite = tempImageList [i];
			i++;
		}
	}

	public static void SetVisibility(string tag, bool Isvisible){
		GameObject[] GameObjects = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject g in GameObjects) {
			g.GetComponent<SpriteRenderer> ().enabled = Isvisible;
		}
	}


}
