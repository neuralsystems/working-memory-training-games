using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shared_PersistentScript : MonoBehaviour {

	public string GAME_NAME;
	//public int currentLevel;			// game with always start with this level
	public static Shared_PersistentScript Instance;
    private static User CurrentPlayer;

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
	public void IncreaseLevelBasketGame( int val){
        //currentLevel += val;
        var ds = new BasketGame_DataService(BasketGame_SceneVariables.DATABASE_NAME);
        var current_level = ds.GetUserProgress(GetCurrentPlayer().Username);
        ds.UpdateUserProgress(GetCurrentPlayer().Username, Mathf.Max(current_level.Level_Obj + val, 0));

    }

    //used for changing levels including both increase as well as decrease
    public void IncreaseLevelTrainGame(int val)
    {
        var ds = new TrainGame_DataServices(TrainGame_SceneVariables.DATABASE_NAME);
        var current_level = ds.GetUserProgress(GetCurrentPlayer().Username);
        ds.UpdateUserProgress(GetCurrentPlayer().Username, Mathf.Max(current_level.Level_Obj + val, 0));
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
    public void IncreaseLevelPianoGame(int val)
    {
        var ds = new DataService(SceneVariables.DATABASE_NAME);
        var current_level = ds.GetUserProgress(GetCurrentPlayer().Username);
        ds.UpdateUserProgress(GetCurrentPlayer().Username, Mathf.Max(current_level.Level_Obj + val, 0));
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
}
