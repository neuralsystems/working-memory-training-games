using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class CarGame_SceneVariables : MonoBehaviour {

	// Use this for initialization
	public static bool isToggle = true;		
	public static string OptionTileTag = "OptionTileTag", cueTag = "CueTag", selectedTileTag = "SelectedTileTag", matchedTag = "SuccessfulTag",fragmentTag ="FragmentTag", trophyTag = "TrophyTag" ;
	public const string databaseName ="ImageMatchingv3.db";
	public const string Game_Name = "CarGame";
	public const string scoretext = "Score";
	public static bool presentCue = false;
//	public static Vector3 stopVector = new Vector3 (0f, -1.5f,0f);
	public static Vector3 initVector = new Vector3 (-11f, -1.5f,0f);
	public static float speed = 20f;
	public static string targetTile = "TargetPosition";
	public static string[] eor = new string[] {"EndofRoad1","EndofRoad2","EndofRoad3","EndofRoad4"};
	public int minScoreToShow = 0;
	public int match_value = 2;		// set 1 for match and 0 for mismatch
	public static float minDistance = 0.05f;
	public static float MIN_DISTANCE = 0.001f;
	public string blockObject = "Block";
	public Vector3[] screenCorners = new Vector3[4];
	public bool  outline = false;
	public float widthPercentageForOptionTile = 0.05f, heightPercentageForOptionTile = .75f;
	public float widthPercentageForTrophyTile = 0.85f, heightPercentageForTrophyTile = .1f;
	public float screenWidth, screenHeight;
	public float leftMargin = .01f, rightMargin = 0.01f;

	void Start () {
		screenWidth = Camera.main.pixelWidth;
		screenHeight = Camera.main.pixelHeight;
		screenCorners[0] = Camera.main.ScreenToWorldPoint (new Vector3 (0, screenHeight, Camera.main.nearClipPlane));
		screenCorners[1] = Camera.main.ScreenToWorldPoint (new Vector3 (screenWidth, screenHeight , Camera.main.nearClipPlane));
		screenCorners[2] = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane));
		screenCorners[3] = Camera.main.ScreenToWorldPoint (new Vector3 (screenWidth , 0  , Camera.main.nearClipPlane));
		DivideScreenAndPlaceOptionTiles ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void ResetGame(){
		Camera.main.GetComponent<Timer>().ResetTimer ();
		Camera.main.GetComponent<CarGame_GameManager> ().RandomizeObjects ();
		GameObject[] go = GameObject.FindGameObjectsWithTag (OptionTileTag);
		foreach (GameObject g in go) {
			g.GetComponent<ImageEffect> ().Initiate ();
		}
		//				ShowParticlesAndPlay ();
	}

	public void ResetorRestart(){
		GameObject[] go = GameObject.FindGameObjectsWithTag (OptionTileTag);
		if (go.Length == 0) {
			var scoreText = GameObject.Find (scoretext).GetComponent<Text> ().text;
			var totalScore = Convert.ToInt32 (scoreText);
			CarGame_DataService ds = new CarGame_DataService (databaseName);
			var scenes = ds.GetNextLevelToLoad (SceneManager.GetActiveScene().name);
			foreach (var scene in scenes) {
				//remaining in the same level
				if (scene.GetUpperLimit () > totalScore && scene.GetLowerLimit () < totalScore) {
					SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
				} else if (scene.GetUpperLimit () <= totalScore) {
					SceneManager.LoadScene (scene.GetNextLevel ());
				} else {
					SceneManager.LoadScene (scene.GetPreviousLevel ());
				}
			}
		} else {
			ResetGame ();
		}
	}

	void DivideScreenAndPlaceOptionTiles(){
		GameObject[] optionTiles = GameObject.FindGameObjectsWithTag (OptionTileTag);
		float x = (1.0f/ArrangeTiles.numOptionTile[Camera.main.GetComponent<ArrangeTiles>().level]) - leftMargin - rightMargin;
		Debug.Log (x);
		float i = 0.5f;
		foreach (GameObject optiontile in optionTiles) {
			optiontile.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (screenWidth *(widthPercentageForOptionTile + i*x), screenHeight * heightPercentageForOptionTile, Camera.main.nearClipPlane));
			i+=1;
			optiontile.transform.localScale = Camera.main.ScreenToWorldPoint (new Vector3 (screenWidth * (x), 0f, 0f));
		}
		GameObject.Find (targetTile).transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (screenWidth *(widthPercentageForTrophyTile), screenHeight * heightPercentageForTrophyTile, Camera.main.nearClipPlane));

	}

	public Vector3 GetPointOnScreen(float width_percentage, float height_percentage){
		var screen_height = Camera.main.pixelHeight;
		var screen_width = Camera.main.pixelWidth;
		return Camera.main.ScreenToWorldPoint (new Vector3 (screen_width * width_percentage, screen_height * height_percentage, Camera.main.nearClipPlane));

	}


}
