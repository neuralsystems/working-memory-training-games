using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shared_PersistentScript : MonoBehaviour {

	public string GAME_NAME;
	//public int currentLevel;			// game with always start with this level
	public static Shared_PersistentScript Instance;
    private static User CurrentPlayer;
    int min_level_value = 1;

    float time_in_sec = 0f;

    public User GetCurrentPlayer()
    {
        return CurrentPlayer;
    }

    public void SetCurrentPlayer(User _player)
    {
        CurrentPlayer = _player;
    }

    void Awake(){
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy (gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void CommonFunction(){
		
	}

    public BasketGame_Levels GetNewBasketGameLevelDetails(){
		var ds = new BasketGame_DataService (BasketGame_SceneVariables.DATABASE_NAME);
        var currentLevel = ds.GetUserProgress(GetCurrentPlayer().Username);
        var current_level_objects = ds.GetLevelsObject (currentLevel.Level_Obj);
		BasketGame_Levels x = new BasketGame_Levels ();
		x.LevelNumber = 1;
		x.NumBasket = 1;
		x.Capacity = 1;
		foreach (var current_level in current_level_objects) {
			x = current_level;
            //Debug.Log("capacity and num basket= " + x.Capacity + " " + x.NumBasket);
            //break;
		}
		//Debug.Log (x.Capacity +" "+ x.NumBaskets );
		return x;

	}

    //used for changing levels including both increase as well as decrease
	public void IncreaseLevelBasketGame( float error_count, float total ){
        //currentLevel += val;
        int val = SetLevel(error_count, total);
        var ds = new BasketGame_DataService(BasketGame_SceneVariables.DATABASE_NAME);
        var current_level = ds.GetUserProgress(GetCurrentPlayer().Username);
        ds.UpdateUserProgress(GetCurrentPlayer().Username, Mathf.Max(current_level.Level_Obj + val, min_level_value));

    }

    //used for changing levels including both increase as well as decrease
    public void IncreaseLevelTrainGame(float error_count, float total)
    {
        int val = SetLevel(error_count, total);
        var ds = new TrainGame_DataServices(TrainGame_SceneVariables.DATABASE_NAME);
        var current_level = ds.GetUserProgress(GetCurrentPlayer().Username);
        ds.UpdateUserProgress(GetCurrentPlayer().Username, Mathf.Max(current_level.Level_Obj + val, min_level_value));
    }

    public TrainGame_Levels GetNewTrainGameLevelDetails(){
        var ds = new TrainGame_DataServices(TrainGame_SceneVariables.DATABASE_NAME);
        var currentLevel = ds.GetUserProgress(GetCurrentPlayer().Username);
  //      var Value_For_Block = 1;
		//var ds = new TrainGame_DataServices (TrainGame_SceneVariables.DATABASE_NAME);
        //var currentLevel = 1;
		var current_level_objects = ds.GetLevelsObject (currentLevel.Level_Obj);
		TrainGame_Levels x = new TrainGame_Levels();
		x.LevelNumber = 1;
		x.NumOfBogie = 1;
		x.ShouldBlock = 0;
		foreach (var current_level in current_level_objects) {
			x = current_level;
			break;
		}
		//Debug.Log ((x.ShouldBlock  == Value_For_Block)+" "+ x.NumOfBogie );
		return x;

	}

    //used for changing levels including both increase as well as decrease
    public void IncreaseLevelPianoGame(float error_count, float total)
    {
        int val = SetLevel(error_count, total);
        var ds = new DataService(SceneVariables.DATABASE_NAME);
        var current_level = ds.GetUserProgress(GetCurrentPlayer().Username);
        ds.UpdateUserProgress(GetCurrentPlayer().Username, Mathf.Max(current_level.Level_Obj + val, min_level_value));
    }

    public PianoGame_Levels GetNewPianoGameLevelDetails()
    {
        var ds = new DataService(SceneVariables.DATABASE_NAME);
        var currentLevel = ds.GetUserProgress(GetCurrentPlayer().Username);
        //      var Value_For_Block = 1;
        //var ds = new TrainGame_DataServices (TrainGame_SceneVariables.DATABASE_NAME);
        //var currentLevel = 1;
        var current_level_objects = ds.GetLevelsObject(currentLevel.Level_Obj);
        PianoGame_Levels x = new PianoGame_Levels();
        foreach (var current_level in current_level_objects)
        {
            x = current_level;
            break;
        }
        //Debug.Log ((x.ShouldBlock  == Value_For_Block)+" "+ x.NumOfBogie );
        return x;

    }

     int SetLevel(float error_count, float total)
    {
        var error_rate_for_increment = .2f;
        var error_rate_for_decrement_1 = .7f;
        var error_rate_for_decrement_2 = .9f;
        //int change = 0;
        // [****** LEVEL CHANGE SCHEMA ******] //
        // increase level by 1 if error rate is  <= 20%
        // decrease level by 1 if error rate is 70% or above 
        // decrease level by 2 if error rate is 90% or above
        // keep it the same if it is between 20 or 70
        if (error_count > total * error_rate_for_decrement_2)
        {
            Debug.Log("Decreasing level by 2");
            return -2;
        }
        else if (error_count > total * error_rate_for_decrement_1)
        {
            Debug.Log("decreasing level by 1");
            return -1;
        } else if (error_count > total * error_rate_for_increment)
        {
            Debug.Log("keeping the level same");
            return 0;
        }
        Debug.Log("increasing level by 1");
        return 1;
    }

    void LogTime()
    {

    }
}
