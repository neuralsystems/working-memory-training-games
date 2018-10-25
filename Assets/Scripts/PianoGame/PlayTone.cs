using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class PlayTone : MonoBehaviour {


    public static string sample;
    string original_tone;
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
    public Canvas level_canvas;
    public Transform level_content;
    int num_of_notes;
    public GameObject piano_go;
    void Awake(){
		current_length = initial_length;
		camera_position = Camera.main.transform.position;
	}

	// Use this for initialization
	void Start () {
        //StartGame();
	}

    public void StartGame()
    {
        GetLevelDetails();
        
        tones = Camera.main.GetComponent<Tones>();

        //      initial_length = 1;
        //      WAIT_TIME = 1.0f;
        //      PLAY_TIME = 1.0f;
        //gradient = 1;
        consequtive_correct = 0;
        sample = "";
        previous = 0;
        GetNotes(original_tone, tones.GetDelimeter());
        Camera.main.GetComponent<SceneVariables>().Reset();
        var numOfEmptyRewardSquare = initial_length + (gradient * (CONSECUTIVE_CORRECT_THRESHOLD - 1));
        num_of_notes = numOfEmptyRewardSquare;
        StartCoroutine(Camera.main.GetComponent<SceneVariables>().PlaceEmptyRewardSquare(numOfEmptyRewardSquare));
    }

    void GetLevelDetails()
    {

        Debug.Log("getting levels details");
        var persistan_go = GameObject.Find(Shared_Scenevariables.masterGO);
        var level_obj = persistan_go.GetComponent<Shared_PersistentScript>().GetNewPianoGameLevelDetails();
        tones = Camera.main.GetComponent<Tones>();
        original_tone = tones.GetToneAtRandomForLevel(level_obj.LevelNumber);
        initial_length = level_obj.InitialLength;
        gradient = level_obj.Gradient;
        WAIT_TIME = level_obj.WaitTime;
        PLAY_TIME = level_obj.PlayTime;
        SceneVariables.PLAY_TIME = PLAY_TIME;
        SceneVariables.WAIT_TIME = WAIT_TIME;
        current_length = initial_length;
        CONSECUTIVE_CORRECT_THRESHOLD = level_obj.Threshold;
    }
	
	public void Repeat(){
		sample = "";
		SceneVariables.IS_READY = true;
		
	}

	public void Next(){
        //if (SceneVariables.error_count > SceneVariables.max_allowed_error) {
        //          current_length = current_length * 1;
        //} else {
        //SceneVariables.error_count -= Mathf.Max (SceneVariables.error_count - 1, 0);
        Debug.Log("next is called");
		current_length += gradient;
		previous += gradient;
		//}
		consequtive_correct++;
		sample = "";
		SceneVariables.IS_READY = true;
	}

	public IEnumerator LoadNextLevel(){
        //Debug.Log ("called LoadNextLevel");
        //consequtive_correct = 0;
        ////initial_length += incrementForNextLevel;
        ////current_length = initial_length;
        ////gradient = 1;
        //      GetLevelDetails();
        //      previous = 0;
        //Camera.main.GetComponent<SceneVariables> ().Reset ();
        //var numOfEmptyRewardSquare = initial_length + (gradient * (CONSECUTIVE_CORRECT_THRESHOLD - 1));
        //StartCoroutine(Camera.main.GetComponent<SceneVariables>().PlaceEmptyRewardSquare(numOfEmptyRewardSquare));
        //StartGame();
        level_canvas.gameObject.SetActive(true);
        StartCoroutine(level_content.GetComponent<LevelScreenManager>().ShowTransition());
		yield return null;
	}


    public IEnumerator DisplayOnLevelComplete(bool total_sequence_played) {
        Debug.Log("called DisplayOnLevelComplete");

        DestroyCueSquares(Camera.main.GetComponent<SceneVariables>().USER_INPUT_SQUARE_TAG);
        DestroyCueSquares(Camera.main.GetComponent<SceneVariables>().SAMPLE_SQUARE_TAG);
        //		GameObject.Find (GetComponent<SceneVariables> ().REWARD_SQUARE_PARENT).transform.position = GameObject.Find (GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).transform.position;

        if (total_sequence_played)
        {
            //StartCoroutine(reward_square_scroll.GetComponent<RewardSquareUIScrollBehavior>().MoveDown());
            //Debug.Log("RewardSquareUIScrollBehavior: MoveDown()");
            //StartCoroutine(GameObject.Find (GetComponent<SceneVariables> ().REWARD_SQUARE_PARENT).GetComponent<PG_RewardSquareParentBehavior>().MoveToTarget( GameObject.Find (GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).transform.position));
            ChangeLevel(SceneVariables.error_count, num_of_notes, true);

            yield return StartCoroutine(RepeatTone(true));
        }

        var reward_square_scroll = GameObject.Find(GetComponent<SceneVariables>().REWARD_SQUARE_UI_SCROLL);
        yield return StartCoroutine(reward_square_scroll.GetComponent<RewardSquareUIScrollBehavior>().Show(false));

        if (total_sequence_played)
        { 
            var rain_particle_system_object = GameObject.FindGameObjectWithTag(Camera.main.GetComponent<SceneVariables>().RAIN_PARTICLE_SYSTEM_TAG);
            rain_particle_system_object.GetComponent<ParticleSystem>().Play();
            yield return StartCoroutine(WaitForRainToStop(rain_particle_system_object.GetComponent<ParticleSystem>()));
        }
	}

    int ChangeLevel(int error_count, int total, bool levelComplete)
    {
        var persistan_go = GameObject.Find(Shared_Scenevariables.masterGO);
        // ***************************** //
        // moved this function to the shared_persistantScript.cs to keep it common for train, basket and piano game
        // **************************** //

        //int change_by = 0; 
        //if(SceneVariables.error_count > 0.9f  * num_of_notes)
        //{
        //    change_by = -2;
        //} else if(SceneVariables. error_count > 0.7f * num_of_notes)
        //{
        //    change_by = -1;
        //} else if( SceneVariables.error_count < 0.2f * num_of_notes)
        //{
        //    change_by = 1;
        //}
        //Debug.Log(change_by + "is the change_by and error count = " + SceneVariables.error_count);
        return persistan_go.GetComponent<Shared_PersistentScript>().IncreaseLevelPianoGame(error_count *1.0f, total *1.0f, levelComplete);
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
		var reward_square_ui_scroll = GameObject.Find(Camera.main.GetComponent<SceneVariables>().REWARD_SQUARE_UI_SCROLL);
		var reward_square_ui_parent = GameObject.FindGameObjectWithTag(Camera.main.GetComponent<SceneVariables>().REWARD_SQUARE_UI_PARENT_TAG);
        //		if (isRepeat) {
        //			var shift =  reward_square_parent.transform.GetChild (0).transform.GetChild (0).GetComponent<SpriteRenderer> ().bounds.size;
        //			Debug.Log ("moving camera down by " + shift);
        //			
        //			transform.position -= Vector3.down * shift.y ;
        //		}
        OnKeyPress.numOfKeysPressed = 0;
		SceneVariables.IS_USER_MODE = false;
        
        for (int i = start; i < end; i++) {
			if (toneTOBePlayed[i] != "") {
                Debug.Log(toneTOBePlayed[i] + "is played");
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
                    var maxVisibleRewardSq = reward_square_parent.GetComponent<PG_RewardSquareParentBehavior>().MaximumVisibleRewardSquares();
                    var n = reward_square_parent.transform.childCount;
                    if (n > maxVisibleRewardSq)
                    {
                        yield return StartCoroutine(reward_square_ui_scroll.GetComponent<RewardSquareUIScrollBehavior>().ScrollTo(i, n, maxVisibleRewardSq));
                    }

                    try
                    {
                        //var reward_square_child_object = reward_square_parent.transform.GetChild(i).transform.GetChild(0);
                        //reward_square_child_object.transform.localScale += new Vector3(1, 1, 1) * .3f;
                        //reward_square_child_object.GetComponent<Rigidbody2D>().gravityScale = 2;
                        //reward_square_child_object.GetComponent<Rigidbody2D>().velocity = new Vector3(2f, 0f, 0f);

                        var reward_square_ui_child_object = reward_square_ui_parent.transform.GetChild(i).transform.GetChild(0);
                        reward_square_ui_child_object.GetComponent<KeySquareUIBehavior>().Pop(true);

                    }
                    catch (Exception e)
                    {
                        Debug.Log("Nothing to SHow");
                    }


                }

                yield return StartCoroutine(g.GetComponent<OnKeyPress> ().PlaySound(isRepeat));
                yield return new WaitForSeconds(.2f);
                if (isRepeat && !isLastRepeat) {
					var reward_square_child_object = reward_square_parent.transform.GetChild (i).transform.GetChild (0);
					reward_square_child_object.GetComponent<SpriteRenderer> ().sortingLayerName = original_layer;
                    reward_square_child_object.transform.localScale -= new Vector3(1, 1, 1) * .3f;
                } 

			} else {
				yield return new WaitForSeconds (WAIT_TIME);
			}
		}
		//yield return new WaitForSeconds (PLAY_TIME);
		if (!isRepeat) {
			SceneVariables.IS_USER_MODE = true;

            // uncomment next line to enable wobble effect
            ChangeUserModeDisplay(true);
        }
//		else if (isRepeat) {
//			var shift =  reward_square_parent.transform.GetChild (0).transform.GetChild (0).GetComponent<SpriteRenderer> ().bounds.size;
//			Debug.Log ("moving camera down by " + shift);
//
//			transform.position -= Vector3.up * shift.y ;
//		}
	}
		

    public void ChangeUserModeDisplay(bool val)
    {
        var num_child = piano_go.transform.childCount;
        //piano_go.GetComponent<Scalling>().SetScale(true);
        //for(int i = 0;i < num_child;i++)
        //{
        //   piano_go.transform.GetChild(i).GetComponent<OnKeyPress>().DisplayUserMode(val);
        //}
    }

	public IEnumerator RepeatTone(bool isLastRepeat = false){
		string delimeter = tones.GetDelimeter ();
		sample = GetFirstnNotes (original_tone, delimeter,current_length);
        var til = current_length - gradient;
        if (isLastRepeat)
        {
            WAIT_TIME = 0.7f;
            PLAY_TIME = 0.5f;
            til = toneTOBePlayed.Count;
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
        toneTOBePlayed.Clear();
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
        Debug.Log("PG_RewardSquareParentBehavior: MoveCamera()");
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

    public void CheckWhilePlay()
    {
        Debug.Log("CheckWhilePlay");
        int levelChange = ChangeLevel(SceneVariables.sequence_error_count, CONSECUTIVE_CORRECT_THRESHOLD, false);
        if (levelChange != 0)
        {
            StartCoroutine(DisplayOnLevelComplete(false));
            StartCoroutine(LoadNextLevel());
        }
        Debug.Log("sequence_error_count: " + SceneVariables.sequence_error_count + " threshold: " + CONSECUTIVE_CORRECT_THRESHOLD);
    }

	public void CheckOnComplete(){
		Debug.Log ("Checked for level complete");
		if (consequtive_correct >= CONSECUTIVE_CORRECT_THRESHOLD) {
			SceneVariables.IS_USER_MODE = false;
			StartCoroutine (DisplayOnLevelComplete (true));
		}
        else
        {
            //startcoroutine(repeattone ());
            GameObject.Find(Camera.main.GetComponent<SceneVariables>().playSound).GetComponent<HomeScreenButtons>().SetHaloToggle(true);
        }

    }
}
