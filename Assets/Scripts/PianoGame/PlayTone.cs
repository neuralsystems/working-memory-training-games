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
	Vector3 camera_position;
    public float WAIT_TIME, PLAY_TIME;
    const string KeySquareObjectPool = "KeySquareObjectPool";
    int CONSECUTIVE_CORRECT_THRESHOLD;
    void Awake(){
		current_length = initial_length;
		camera_position = Camera.main.transform.position;
	}

	// Use this for initialization
	void Start () {
        GetLevelDetails();
        tones = Camera.main.GetComponent<Tones>();
  //      initial_length = 1;
  //      WAIT_TIME = 1.0f;
  //      PLAY_TIME = 1.0f;
		//gradient = 1;
		
		sample = "";
		original_tone  = tones.GetToneAtRandom ();
		GetNotes(original_tone, tones.GetDelimeter ());
		var numOfEmptyRewardSquare = initial_length + (gradient * (CONSECUTIVE_CORRECT_THRESHOLD - 1));
		StartCoroutine(Camera.main.GetComponent<SceneVariables>().PlaceEmptyRewardSquare(numOfEmptyRewardSquare));
	}

    void GetLevelDetails()
    {
        var persistan_go = GameObject.Find(SceneVariables.masterGO);
        var level_obj = persistan_go.GetComponent<Shared_PersistentScript>().GetNewPianoGameLevelDetails();
        initial_length = level_obj.InitialLength;
        gradient = level_obj.Gradient;
        WAIT_TIME = level_obj.WaitTime;
        PLAY_TIME = level_obj.PlayTime;
        current_length = initial_length;
        CONSECUTIVE_CORRECT_THRESHOLD = level_obj.Threshold;
    }
	
	public void Repeat(){
		sample = "";
		SceneVariables.IS_READY = true;
		GameObject.Find (Camera.main.GetComponent<SceneVariables> ().playSound).GetComponent<HomeScreenButtons> ().SetHaloToggle(true);
	}

	public void Next(){
		//if (SceneVariables.error_count > SceneVariables.max_allowed_error) {
  //          current_length = current_length * 1;
		//} else {
		SceneVariables.error_count -= Mathf.Max (SceneVariables.error_count - 1, 0);
		current_length += gradient;
		previous += gradient;
		//}
		consequtive_correct++;
		sample = "";
		SceneVariables.IS_READY = true;
	}

	public IEnumerator LoadNextLevel(){
		Debug.Log ("called LoadNextLevel");
		consequtive_correct = 0;
		//initial_length += incrementForNextLevel;
		//current_length = initial_length;
		//gradient = 1;
        GetLevelDetails();
        previous = 0;
		Camera.main.GetComponent<SceneVariables> ().Reset ();
		var numOfEmptyRewardSquare = initial_length + (gradient * (CONSECUTIVE_CORRECT_THRESHOLD - 1));
		StartCoroutine(Camera.main.GetComponent<SceneVariables>().PlaceEmptyRewardSquare(numOfEmptyRewardSquare));
		yield return null;
	}


	public IEnumerator DisplayOnLevelComplete(){
		Debug.Log ("called DisplayOnLevelComplete");
        ChangeLevel();
        DestroyCueSquares (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG);
		DestroyCueSquares (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_TAG);
//		GameObject.Find (GetComponent<SceneVariables> ().REWARD_SQUARE_PARENT).transform.position = GameObject.Find (GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).transform.position;
		StartCoroutine(GameObject.Find (GetComponent<SceneVariables> ().REWARD_SQUARE_PARENT).GetComponent<PG_RewardSquareParentBehavior>().MoveToTarget( GameObject.Find (GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).transform.position));
		yield return StartCoroutine(RepeatTone (true));
		var rain_particle_system_object = GameObject.FindGameObjectWithTag (Camera.main.GetComponent<SceneVariables> ().RAIN_PARTICLE_SYSTEM_TAG);
		rain_particle_system_object.GetComponent<ParticleSystem> ().Play ();
		yield return StartCoroutine(WaitForRainToStop(rain_particle_system_object.GetComponent<ParticleSystem>()));
	}

    void ChangeLevel()
    {
        var persistan_go = GameObject.Find(SceneVariables.masterGO);
        persistan_go.GetComponent<Shared_PersistentScript>().IncreaseLevelPianoGame(1);
    }
	public IEnumerator WaitForRainToStop(ParticleSystem rain){
		if (rain.isPlaying) {
			yield return null;
			StartCoroutine (WaitForRainToStop (rain));
		} else {
			StartCoroutine (LoadNextLevel ());
		}
	}



//	This functions plays a tone with the help of audioSource attached to the main camera
//	at the start the audiosource is played which calls the overrided function OnAudioFilterRead in the Sinus script
// 	to play sound of a note, the frequency in the sinus script is set to frequency of that note. 

	public IEnumerator PlaySomeTone(string notes, string delimeter, int start, int end, bool isRepeat = false, bool isLastRepeat = false){
		var reward_square_parent = GameObject.Find(Camera.main.GetComponent<SceneVariables>().REWARD_SQUARE_PARENT);
//		if (isRepeat) {
//			var shift =  reward_square_parent.transform.GetChild (0).transform.GetChild (0).GetComponent<SpriteRenderer> ().bounds.size;
//			Debug.Log ("moving camera down by " + shift);
//			
//			transform.position -= Vector3.down * shift.y ;
//		}
		OnKeyPress.numOfKeysPressed = 0;
		SceneVariables.IS_USER_MODE = false;
		Debug.Log (notes + "is played");
		for (int i = start; i < end; i++) {
			if (toneTOBePlayed[i] != "") {
				var token = toneTOBePlayed [i];
				string token1 =token.ToUpper ();
				var g = GameObject.Find (token1).gameObject;
				var original_layer = "Game";
				string game_layer = "Game";
				if (isRepeat && !isLastRepeat) {
                    //Debug.Log ("Color set");
                    Debug.Log("i = " + i);
                    var reward_square_child_object = reward_square_parent.transform.GetChild (i).transform.GetChild (0);
					original_layer = reward_square_child_object.GetComponent<SpriteRenderer> ().sortingLayerName;
					reward_square_child_object.GetComponent<SpriteRenderer> ().sortingLayerName = game_layer;
					var y_index = reward_square_child_object.transform.localPosition;
					y_index.y = y_index.y -  1f;
					reward_square_child_object.transform.localScale += new Vector3(1,1,1) * .3f;
				}
                else if (isRepeat && isLastRepeat)
                {
                    try
                    {
                        var reward_square_child_object = reward_square_parent.transform.GetChild(i).transform.GetChild(0);
                        reward_square_child_object.transform.localScale += new Vector3(1, 1, 1) * .3f;
                        reward_square_child_object.GetComponent<Rigidbody2D>().gravityScale = 2;
                        reward_square_child_object.GetComponent<Rigidbody2D>().velocity = new Vector3(2f, 0f, 0f);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Nothing to SHow");
                    }


                }

                g.GetComponent<OnKeyPress> ().PlaySound(isRepeat);
				yield return new WaitForSeconds (WAIT_TIME);
				if (isRepeat && !isLastRepeat) {
					var reward_square_child_object = reward_square_parent.transform.GetChild (i).transform.GetChild (0);
					reward_square_child_object.GetComponent<SpriteRenderer> ().sortingLayerName = original_layer;
                    reward_square_child_object.transform.localScale -= new Vector3(1, 1, 1) * .3f;
                } 

			} else {
				yield return new WaitForSeconds (WAIT_TIME);
			}
		}
		yield return new WaitForSeconds (PLAY_TIME);
		if (!isRepeat) {
			SceneVariables.IS_USER_MODE = true;
		}
//		else if (isRepeat) {
//			var shift =  reward_square_parent.transform.GetChild (0).transform.GetChild (0).GetComponent<SpriteRenderer> ().bounds.size;
//			Debug.Log ("moving camera down by " + shift);
//
//			transform.position -= Vector3.up * shift.y ;
//		}
	}
		

	public IEnumerator RepeatTone(bool isLastRepeat = false){
		string delimeter = tones.GetDelimeter ();
		sample = GetFirstnNotes (original_tone, delimeter,current_length);
        var til = current_length - gradient;
        if (isLastRepeat)
        {
            WAIT_TIME = 0.7f;PLAY_TIME = 0.5f;
            til = Mathf.Min((current_length - gradient) * 3, toneTOBePlayed.Count);
         }
        Debug.Log("til = " + til);
		yield return StartCoroutine(PlaySomeTone (sample,tones.GetDelimeter(), 0, til ,true, isLastRepeat));
//		Camera.main.GetComponent<SceneVariables> ().GetRandomClapping ();
		if (consequtive_correct < CONSECUTIVE_CORRECT_THRESHOLD) {
			GameObject.Find (Camera.main.GetComponent<SceneVariables> ().playSound).GetComponent<HomeScreenButtons> ().SetHaloToggle (true);
		}
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
		string delimeter = tones.GetDelimeter ();
		sample = GetFirstnNotes (original_tone, delimeter, length);
		StartCoroutine(PlaySomeTone (sample,tones.GetDelimeter(), previous, current_length));

	}

	string GetFirstnNotes(string tone, string delimeter, int n){
		string[] tokens = TailorTone(tone).Split(new[] { delimeter }, StringSplitOptions.None);
		string output = "";
		for (int i = previous; i < n; i++) {
			output += tokens [i] + " ";

		}
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
		var reward_square = GameObject.FindGameObjectsWithTag (Camera.main.GetComponent<SceneVariables> ().REWARD_SQUARE_TAG)[0];
		var reward_square_parent = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().REWARD_SQUARE_PARENT);
		var new_position = Shared_ScriptForGeneralFunctions.GetPointOnScreen(Camera.main.GetComponent<SceneVariables>().widthPercentageForRewardSquare, Camera.main.GetComponent<SceneVariables> ().heightPercentageForRewardSquare);
		new_position.y += reward_square_parent.GetComponentInChildren<SpriteRenderer> ().bounds.size.y + 2f;
		reward_square_parent.GetComponent<PG_RewardSquareParentBehavior> ().MoveCamera(new_position);
		SceneVariables.IS_USER_MODE = false;
		PlayGame (current_length);
	}

	public int GetTuneLength(){
		return current_length - previous;
	}

	public void DestroyCueSquares(string tag){
		var gs = GameObject.FindGameObjectsWithTag (tag);
        var ks = GameObject.Find(KeySquareObjectPool);
        foreach (var g in gs) {
            //ks.GetComponent<SimpleObjectPool>().ReturnObject(g);
            Destroy(g);
        }

	}

	public void CheckOnComplete(){
		Debug.Log ("Checked for level complete");
		if (consequtive_correct >= CONSECUTIVE_CORRECT_THRESHOLD) {
			SceneVariables.IS_USER_MODE = false;
			StartCoroutine (DisplayOnLevelComplete ());
		} else {
			StartCoroutine(RepeatTone ());

		}

	}
}
