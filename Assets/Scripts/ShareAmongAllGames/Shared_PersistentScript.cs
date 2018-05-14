using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shared_PersistentScript : MonoBehaviour {

	public string GAME_NAME;
	public int currentLevel = 1;			// game with always start with this level
	public static Shared_PersistentScript Instance;

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

	public Levels GetNewBasketGameLevelDetails(){
		var ds = new BasketGame_DataService (BasketGame_SceneVariables.DATABASE_NAME);
		var current_level_objects = ds.GetLevelsObject (currentLevel);
		Levels x = new Levels ();
		x.LevelNumber = 1;
		x.NumBaskets = 1;
		x.Capacity = 1;
		foreach (var current_level in current_level_objects) {
			x = current_level;
			break;
		}
		Debug.Log (x.Capacity +" "+ x.NumBaskets );
		return x;

	}

	public void IncreaseLevel( int val){
		currentLevel += val;
	}

	public TrainGame_Levels GetNewTrainGameLevelDetails(){
		var Value_For_Block = 1;
		var ds = new TrainGame_DataServices (TrainGame_SceneVariables.DATABASE_NAME);
		var current_level_objects = ds.GetLevelsObject (currentLevel);
		TrainGame_Levels x = new TrainGame_Levels();
		x.LevelNumber = 1;
		x.NumOfBogie = 1;
		x.ShouldBlock = 0;
		foreach (var current_level in current_level_objects) {
			x = current_level;
			break;
		}
		Debug.Log ((x.ShouldBlock  == Value_For_Block)+" "+ x.NumOfBogie );
		return x;

	}

}
