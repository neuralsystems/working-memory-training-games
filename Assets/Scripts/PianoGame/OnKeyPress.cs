using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnKeyPress : MonoBehaviour {

//	public AudioSource sound;
	public static string delimeter,userString ="";
	public delegate void AudioCallback();
	SpriteRenderer spriteRenderer;
	Color originalColor;
	public Transform keySquare;
	public string keySquareImageName, keyName;
	string folderName = "Buttons/", game_name;
	public static int numOfKeysPressed = 0;

	void Start () {
		game_name = Camera.main.GetComponent<SceneVariables> ().Game_Name;
		keySquareImageName = keyName + "_square";
		this.gameObject.GetComponent<AudioSource> ().Play ();
		this.gameObject.GetComponent<AudioSource> ().Pause ();
		SceneVariables.IS_PRESSED = false;
		Tones tone = Camera.main.GetComponent<Tones>();
		delimeter = tone.GetDelimeter ();
		userString = "";
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
		originalColor = spriteRenderer.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1)
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPos = new Vector2(wp.x, wp.y);
			if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
			{
				OnMouseDown();
			}
		}
	
	}

	// when the object is clicked 
	public void OnMouseDown(){
		Debug.Log ("is user mode: " + SceneVariables.IS_USER_MODE);
		if (!SceneVariables.IS_PRESSED && SceneVariables.IS_USER_MODE && SceneVariables.IS_READY) {
			Debug.Log(Camera.main.GetComponent<PlayTone> ().GetTuneLength () + " " +numOfKeysPressed);
			if (Camera.main.GetComponent<PlayTone> ().GetTuneLength () > numOfKeysPressed) {
				SceneVariables.IS_PRESSED = true;
				PlaySound ();
				if ((PlayTone.sample != "")) {
					numOfKeysPressed++;
					userString += this.gameObject.name + delimeter;
					userString = userString.ToUpper ();
					if (Camera.main.GetComponent<PlayTone> ().GetTuneLength () == numOfKeysPressed) {
						//Debug.Log ("in check");
						if (CheckWithSampleTune ()) {
							userString = "";
							Camera.main.GetComponent<SceneVariables> ().correctMatch = true;
							//Debug.Log ("calling the next");
							Camera.main.GetComponent<PlayTone> ().Next ();
						} else {
							SceneVariables.error_count += 1;
							//Debug.Log ("error count is" + SceneVariables.error_count);
							Camera.main.GetComponent<PlayTone> ().Repeat ();
						}
					}
				}
			}
		}


	}
		

	public void ShowKeySquare(){
		var keysquare = Instantiate (keySquare, transform.position, Quaternion.identity);
		Debug.Log (SceneVariables.IS_USER_MODE);
		if (SceneVariables.IS_USER_MODE) {
			keysquare.GetComponent<KeySquareBehavior>().tagForGameObject = Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG;
			keysquare.tag = Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG;
		} else {
			keysquare.GetComponent<KeySquareBehavior>().tagForGameObject = Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_TAG;
			keysquare.tag = Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_TAG;
		}
		var file_name = keySquareImageName;
		//Debug.Log ("tried at: "+ folderName + file_name);
		keysquare.GetComponent<SpriteRenderer> ().sprite = Resources.Load (game_name + "/" + folderName + file_name, typeof(Sprite)) as Sprite;
	}


	public void PlaySound(bool isRepeat = false)
	{
		SetTransparency (true);
		if (!isRepeat) {
			var tappinghand = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().tappingHand).gameObject;
			StartCoroutine (tappinghand.GetComponent<FingerBehavior> ().SetPositionAndPlay (transform.position, keyName));
		}
//		GetComponent<SpriteRenderer> ().color = SceneVariables.PRESSED_COLOR;
		GetComponent<AudioSource> ().Play ();
		StartCoroutine (DelayedCallback (SceneVariables.PLAY_TIME, AudioFinished));
//		spriteRenderer.color = SceneVariables.PRESSED_COLOR;
	}


	// this function checks if the user guessed sequence is same as the played one or not
	bool CheckWithSampleTune(){
		if (userString.ToLower().Contains(PlayTone.sample.ToLower()))
			return true;
		return false;

	}

	public void SetTransparency(bool val){
		var all_keys = GameObject.FindGameObjectsWithTag(SceneVariables.PIANO_KEY_TAG);
		foreach (var key in all_keys) {
			if (key != gameObject) {
				key.GetComponent<OnKeyPress> ().ChangeTransperancy (val);
			}
		}
	}
	// used in case if there is a clip to play and some code has to be executed at the end of the clip
	public void PlaySoundWithCallBack(AudioClip clip, AudioCallback callback){

		StartCoroutine(DelayedCallback(SceneVariables.PLAY_TIME, callback));
	
	}

	private IEnumerator DelayedCallback(float time, AudioCallback callback){

		yield return new WaitForSeconds (time);
		callback ();
	}

	// this function is called when the audio clip ends
	void AudioFinished(){
		GetComponent<AudioSource> ().Pause ();
		SceneVariables.IS_PRESSED = false;
		spriteRenderer.color = originalColor;
		SetTransparency (false);
	}

	void ResetGame()
	{
		
	}

	public void ChangeTransperancy( bool val){
		var tem_color = gameObject.GetComponent<SpriteRenderer> ().material.color;
		//Debug.Log ("called for other keys " + val);
		if (val) {
			tem_color.a = .3f;
		} else {
			tem_color.a = 1;
		}
		gameObject.GetComponent<SpriteRenderer> ().material.color = tem_color;
	}
	
}
