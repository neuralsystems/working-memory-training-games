using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shared_PersistentScript : MonoBehaviour {

	public string GAME_NAME;
	public int basketgame_currentLevel = 1;			// game with always start with this level
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

	public Levels GetNewLevelDetails(){
		var ds = new BasketGame_DataService (BasketGame_SceneVariables.DATABASE_NAME);
		var current_level_objects = ds.GetLevelsObject (basketgame_currentLevel);
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

	public void IncreaseLevel(){
		basketgame_currentLevel += 1;
	}

}
