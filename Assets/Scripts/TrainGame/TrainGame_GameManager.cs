using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGame_GameManager : MonoBehaviour {

	public Transform KEYLOCK_BOGIE_PREFAB, COLOR_BOGIE_PREFAB, ENGINE_PREFAB, BOGIE_COVER_PREFAB;
	public GameObject BLOCK_TRAIN, static_game_object;
	public bool Should_Block;
	int numofBogies = 2;
    int error_count, single_match_error_count;
    public Canvas level_canvas;
    public Transform level_content;
    public Transform track_1, track_2;
    float wait_before_block;
	// Use this for initialization
	void Start () {
        //level_canvas.gameObject.SetActive(true);
        //StartCoroutine(level_content.GetComponent<LevelScreenManager>().ShowTransition());
	}
	
    public void StartGame()
    {
        static_game_object = GameObject.Find(Shared_Scenevariables.masterGO);
        error_count = 0;
        single_match_error_count = 0;
        var level_details = static_game_object.GetComponent<Shared_PersistentScript>().GetNewTrainGameLevelDetails();
        numofBogies = level_details.GetNumofBogie();
        Should_Block = level_details.ShouldBlockTrain();
        wait_before_block = level_details.WaitTime;
        CreateTrain(true, numofBogies);
    }
	// Update is called once per frame
	void Update () {
		
	}



	void CreateTrain(bool colored_or_keylock, int numOfBogies){
		var engine_position = Shared_ScriptForGeneralFunctions.GetPointOnScreen (1.2f, TrainGame_SceneVariables.height_percentage);
		var train_engine = Instantiate (ENGINE_PREFAB, engine_position, Quaternion.identity);
        var engine_size = train_engine.GetComponent<SpriteRenderer>().bounds.size.x ;
        var extra_size = 0f;
		if (colored_or_keylock) {
			var folder_location = TrainGame_SceneVariables.Game_Name + "/" + Camera.main.GetComponent<TrainGame_SceneVariables> ().GetSubFolderName ();
			Debug.Log (folder_location);
			var keys = Camera.main.GetComponent<TrainGame_SceneVariables> ().GetKeys (numOfBogies);
			var previous_lock = train_engine.transform.GetChild (0);
			for (int i = 0; i < numOfBogies; i++) {
				var key_lock_pair = Camera.main.GetComponent<TrainGame_SceneVariables> ().GetValue (keys [i]);
				previous_lock.GetComponent<SpriteRenderer> ().sprite = Resources.Load (folder_location + keys [i], typeof(Sprite)) as Sprite;
				var bogie_object = Instantiate (KEYLOCK_BOGIE_PREFAB, engine_position, Quaternion.identity);
				bogie_object.transform.GetChild (1).GetComponent<SpriteRenderer> ().sprite = Resources.Load (folder_location + key_lock_pair, typeof(Sprite)) as Sprite;
				previous_lock = bogie_object.transform.GetChild (0);
				float shift = 0;
//				if (i == 0) {
//					shift = (train_engine.GetComponent<SpriteRenderer> ().bounds.size.x / 2) + (bogie_object.GetComponent<SpriteRenderer> ().bounds.size.x / 2);
////					engine_position.y -= train_engine.GetComponent<SpriteRenderer> ().bounds.size.y / 2;
////					engine_position.y += bogie_object.GetComponent<SpriteRenderer> ().bounds.size.y / 2;
//				}else{
					shift = (bogie_object.GetComponent<SpriteRenderer> ().bounds.size.x) ;
//				}
				shift += bogie_object.transform.GetChild (1).GetComponent<SpriteRenderer> ().bounds.size.x - 0.1f;
				engine_position.x += shift;
				bogie_object.transform.position = engine_position;
				var temp_pos = bogie_object.transform.position;
				bogie_object.transform.parent = train_engine.transform;
				bogie_object.transform.position = temp_pos;
                extra_size = bogie_object.transform.GetChild(1).GetComponent<SpriteRenderer>().bounds.size.x;

            }
            var total = ((numofBogies + 1) * engine_size) + (2 * extra_size * numofBogies);
            var _ratio = (2*Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0f,0f)).x) / total;
            var _multiplier = (Mathf.Min(1, _ratio));
            train_engine.transform.localScale *= _multiplier;
            var train_position = train_engine.transform.position;
            train_position.y = track_1.transform.position.y + (train_engine.GetComponent<SpriteRenderer>().bounds.size.y * _multiplier * 0.5f + (track_1.GetComponent<SpriteRenderer>().bounds.size.y * .5f ));
            Debug.Log("ration " + _ratio + " " + Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + " " + total + " " + _multiplier );
            train_engine.transform.position = train_position;
            previous_lock.GetComponent<SpriteRenderer> ().sprite = null;
			StartCoroutine (train_engine.GetComponent<TrainGame_Engine_Behavior>().InitialAnimation (track_1.position,track_2.position));
		}
	}


	public IEnumerator BlockAndRandomize(){
        var y_offset = track_2.transform.position.y + track_2.GetComponent<SpriteRenderer>().bounds.size.y * 0.2f;
		GameObject.FindGameObjectWithTag(TrainGame_SceneVariables.ENGINE_TAG).GetComponent<TrainGame_Engine_Behavior> ().RandomizeBogies (!Should_Block, y_offset);
		yield return new WaitForSeconds (1.5f * numofBogies * wait_before_block);
        var all_bogies = GameObject.FindGameObjectsWithTag(TrainGame_SceneVariables.BOGIE_TAG);
        if (Should_Block)  //condition if true show the block train
		{
//			yield return StartCoroutine (BLOCK_TRAIN.GetComponent<TrainGame_BlockTrainBehavior> ().BlockView ());
			
			foreach (var bogie in all_bogies) {
				var spawn_pos = Shared_ScriptForGeneralFunctions.GetPointOnScreen (0f, -.1f);
				spawn_pos.x = bogie.transform.position.x;
				var block_go = Instantiate (BOGIE_COVER_PREFAB, spawn_pos, Quaternion.identity).gameObject;
				StartCoroutine(block_go.GetComponent<TrainGame_BogieCoverScript> ().BlockTarget (bogie));
			}

        }
       


	}

    public void CheckPerformance()
    {
        Debug.Log("performance check- single_match_error: " + single_match_error_count + " numOfBogies: " + numofBogies);
        int levelChange = ChangeGameLevel(single_match_error_count, numofBogies, false);
        if (levelChange!=0)
        {
            LoadNextLevel();
            Destroy(GameObject.FindGameObjectWithTag(TrainGame_SceneVariables.ENGINE_TAG));
        }
    }
    
	public void FinalAnimation()
    {
		ChangeGameLevel (error_count, numofBogies, true);
		StartCoroutine (GameObject.FindGameObjectWithTag (TrainGame_SceneVariables.ENGINE_TAG).GetComponent<TrainGame_Engine_Behavior> ().FinalAnimation ());
//		if (Should_Block) {
//			StartCoroutine (BLOCK_TRAIN.GetComponent<TrainGame_BlockTrainBehavior> ().UnBlockView ());	
//		}
	}

	public void ErrorDetected(){
		error_count++;
        single_match_error_count++;
	}

    public void ResetError()
    {
        single_match_error_count = 0;
    }

	int ChangeGameLevel(int error_count, int numofBogies, bool levelComplete){
        // ***************************** //
        // moved this function to the shared_persistantScript.cs to keep it common for train, basket and piano game
        // **************************** //
        // increase level by 1 if error rate is 20%, decrease level by 1 if error rate is 70% or above and by 2 if error rate is 90% or above
        //      var change = 0;
        //      if (error_count < .2f * numofBogies) {
        //	change = 1;
        //} else if(error_count > .9f * numofBogies){
        //          change = -2;
        //      } else if (error_count > .7f * numofBogies)
        //      {
        //          change = -1;
        //      }
        return static_game_object.GetComponent<Shared_PersistentScript>().IncreaseLevelTrainGame(error_count*1.0f, numofBogies*1.0f, levelComplete);
    }

    public void LoadNextLevel()
    {
        //StartGame();
        level_canvas.gameObject.SetActive(true);
        StartCoroutine(level_content.GetComponent<LevelScreenManager>().ShowTransition());
    }
}
