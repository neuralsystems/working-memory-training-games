using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;
public class PlayTone : MonoBehaviour {


	public static string sample,original_tone;
	Tones tones;
	public int initial_length, gradient, consequtive_correct = 0;
	public int current_length;
	public int previous = 0;
	public int incrementForNextLevel = 1;
	List <string> toneTOBePlayed = new List<string>(); 
	public Transform blockSquare;
	void Awake(){
		current_length = initial_length;
	}
	// Use this for initialization
	void Start () {
//		Debug.Log ("playing..1" );
		tones = Camera.main.GetComponent<Tones>();
//		initial_length = 50;
		gradient = initial_length -1;
		current_length = initial_length;
		sample = "";
		original_tone  = tones.GetToneAtRandom ();
		GetNotes(original_tone, tones.GetDelimeter ());
		var numOfEmptyRewardSquare = initial_length + (gradient * (Camera.main.GetComponent<SceneVariables> ().CONSECUTIVE_CORRECT_THRESHOLD - 1));
		StartCoroutine(Camera.main.GetComponent<SceneVariables>().PlaceEmptyRewardSquare(numOfEmptyRewardSquare));
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log ("calling update from play sound");
	}

	public void Repeat(){
		
//		SceneVariables.IS_USER_MODE = false;
//		consequtive_correct = 0;
		sample = "";
		SceneVariables.IS_READY = true;
//		Camera.main.GetComponent<SceneVariables> ().Reset ();
		GameObject.Find (Camera.main.GetComponent<SceneVariables> ().playSound).GetComponent<SpriteRenderer> ().enabled = true;
	}

	public void Next(){
		Debug.Log ("called next");
//		previous = (current_length -1);
		if (SceneVariables.error_count > SceneVariables.max_allowed_error) {
			current_length = Mathf.Max (current_length - gradient, SceneVariables.min_tone_length);
		} else {
			SceneVariables.error_count -= Mathf.Max (SceneVariables.error_count - 1, 0);
			current_length += gradient;
			previous += gradient;
		}
		consequtive_correct++;
		sample = "";
		SceneVariables.IS_READY = true;
//		SceneVariables.IS_USER_MODE = false;

	}

	public IEnumerator LoadNextLevel(){
		Debug.Log ("called LoadNextLevel");
//		yield return StartCoroutine(DisplayOnLevelComplete());
		consequtive_correct = 0;
		initial_length += incrementForNextLevel;
		current_length = initial_length;
		gradient = 1;
		previous = 0;
		Camera.main.GetComponent<SceneVariables> ().Reset ();
		GameObject.Find (Camera.main.GetComponent<SceneVariables> ().playSound).GetComponent<SpriteRenderer> ().enabled = true;
		var numOfEmptyRewardSquare = initial_length + (gradient * (Camera.main.GetComponent<SceneVariables> ().CONSECUTIVE_CORRECT_THRESHOLD - 1));
		StartCoroutine(Camera.main.GetComponent<SceneVariables>().PlaceEmptyRewardSquare(numOfEmptyRewardSquare));
		yield return null;
	}


	public IEnumerator DisplayOnLevelComplete(){
		Debug.Log ("called DisplayOnLevelComplete");
		DestroyCueSquares (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG);
		DestroyCueSquares (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_TAG);

		GameObject.Find (GetComponent<SceneVariables> ().REWARD_SQUARE_PARENT).transform.position = GameObject.Find (GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).transform.position;
		yield return StartCoroutine(RepeatTone ());
		//		yield return new WaitForSeconds(.5f);
		var rain_particle_system_object = GameObject.FindGameObjectWithTag (Camera.main.GetComponent<SceneVariables> ().RAIN_PARTICLE_SYSTEM_TAG);
		rain_particle_system_object.GetComponent<ParticleSystem> ().Play ();
//		Debug.Log (rain_particle_system_object.GetComponent<ParticleSystem> ().main.duration);
		yield return StartCoroutine(WaitForRainToStop(rain_particle_system_object.GetComponent<ParticleSystem>()));
//		yield return null;
//		StartCoroutine(LoadNextLevel ());
//		LoadNextLevel();
//		yield return new WaitForSeconds(.5f);

	}

	public IEnumerator WaitForRainToStop(ParticleSystem rain){
		if (rain.isPlaying) {
			//			yield return new WaitForSeconds (.5f);
			yield return null;
			StartCoroutine (WaitForRainToStop (rain));
		} else {
			StartCoroutine (LoadNextLevel ());
		}
	}



//	This functions plays a tone with the help of audioSource attached to the main camera
//	at the start the audiosource is played which calls the overrided function OnAudioFilterRead in the Sinus script
// 	to play sound of a note, the frequency in the sinus script is set to frequency of that note. 

	public IEnumerator PlaySomeTone(string notes, string delimeter, int start, int end, bool isRepeat = false){
		OnKeyPress.numOfKeysPressed = 0;
		SceneVariables.IS_USER_MODE = false;
		Debug.Log (notes + "is played");
//		Sinus cameraScript = Camera.main.GetComponent<Sinus> ();
		for (int i = start; i < end; i++) {
			if (toneTOBePlayed[i] != "") {
				var token = toneTOBePlayed [i];
				string token1 =token.ToUpper ();
				var g = GameObject.Find (token1).gameObject;
//				yield return new WaitForSeconds (SceneVariables.PLAY_TIME);
				g.GetComponent<OnKeyPress> ().PlaySound(isRepeat);
				yield return new WaitForSeconds (SceneVariables.WAIT_TIME);
			} else {
				yield return new WaitForSeconds (SceneVariables.WAIT_TIME);
			}
		}
		yield return new WaitForSeconds (SceneVariables.PLAY_TIME);
		if (!isRepeat) {
			SceneVariables.IS_USER_MODE = true;
		}
//		HideSquares ();
	}

	public IEnumerator RepeatTone(){
		string delimeter = tones.GetDelimeter ();
		sample = GetFirstnNotes (original_tone, delimeter,current_length);
		yield return StartCoroutine(PlaySomeTone (sample,tones.GetDelimeter(), 0, current_length-gradient,true));
		GameObject.Find (Camera.main.GetComponent<SceneVariables> ().playSound).GetComponent<SpriteRenderer> ().enabled = true;
	}
	public void HideSquares(){
		GameObject[] keySquares = GameObject.FindGameObjectsWithTag (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_TAG);
		foreach (GameObject keySquare in keySquares) {
			keySquare.GetComponent<SpriteRenderer> ().sprite = blockSquare.gameObject.GetComponent<SpriteRenderer> ().sprite;
		}
	}

//	public void PlayTillComplete(){
//		string delimeter = tones.GetDelimeter ();
//		sample = GetFirstnNotes (original_tone, delimeter, length);
//		StartCoroutine(PlaySomeTone (sample,tones.GetDelimeter(), previous, current_length));
//	}
	public void PlayGame(int length){
//		Debug.Log ("Length is: "+ length);
		string delimeter = tones.GetDelimeter ();
		sample = GetFirstnNotes (original_tone, delimeter, length);
		StartCoroutine(PlaySomeTone (sample,tones.GetDelimeter(), previous, current_length));

	}

	// this function returns first n notes of a sequence
	string GetFirstnNotes(string tone, string delimeter, int n){
		string[] tokens = TailorTone(tone).Split(new[] { delimeter }, StringSplitOptions.None);
//		Debug.Log (tokens.Length);

		string output = "";
		for (int i = previous; i < n; i++) {
			output += tokens [i] + " ";

		}
//		Debug.Log ("output is " + output);
		return output;
	}



	void GetNotes(string tone,string delimeter){
		string[] tokens = TailorTone(tone).Split(new[] { delimeter }, StringSplitOptions.None);
		foreach (string token in tokens) {
			toneTOBePlayed.Add (token);
		}
	}



	// 	this function takes a string and replace consecutive same characters with one same character eg. aaccc to ac,
	// used here to replace consequtive multiple spaces with one space
	string TailorTone(string tone){
		var regex = new Regex("(.)\\1+");
//		Debug.Log(regex.Replace(tone, "$1"));
		return regex.Replace(tone, "$1");
	}


	// simple delay function 
	public IEnumerator Delay(float delay, int current_length){
		yield return new WaitForSeconds (delay);

	}


	// this function is called at correct guess of the played sequence
	public void AtSuccess(){
		Debug.Log (current_length);
		PlayGame (current_length);
	}

	// act as a handler function to call the PlayGame function from external to this script
	public void PlayToneFromExternal(){
		DestroyCueSquares (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG);
		DestroyCueSquares (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_TAG);
		DestroyCueSquares (Camera.main.GetComponent<SceneVariables> ().KEY_SQUARE_TAG);
		Camera.main.GetComponent<SceneVariables> ().DivideScreen ();
//		if (SceneVariables.IS_USER_MODE) {
		SceneVariables.IS_USER_MODE = false;
		PlayGame (current_length);
//		}
	}

	public int GetTuneLength(){
		return current_length - previous;
	}

	public void DestroyCueSquares(string tag){
		var gs = GameObject.FindGameObjectsWithTag (tag);
		foreach (var g in gs) {
			Destroy (g);
		}

	}

	public void CheckOnComplete(){
		Debug.Log ("Checked for level complete");
		if (consequtive_correct >= Camera.main.GetComponent<SceneVariables> ().CONSECUTIVE_CORRECT_THRESHOLD) {
			SceneVariables.IS_USER_MODE = false;
			StartCoroutine (DisplayOnLevelComplete ());
		} else {
			StartCoroutine(RepeatTone ());

		}

	}
}
