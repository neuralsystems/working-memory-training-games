using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using SQLite4Unity3d;

public class ArrangeTiles : MonoBehaviour
{

	public static int[] numOptionTile = new int[]{0,1,2,3,4, 5, 6, 7, 8, 9 };
	public int level = 1;
	public Transform optionTile;
	string  optionTileTag = "OptionTileTag";
	bool isPaused = false;
	List<String> ImageList;
	GameObject[] allGameObjects;
	public Text scoreText;
	public static bool shuffleList = false;
	public string ImageFolder = "GameImages/", game_name;
	string category ;
	int categoryId;
	int MAX_NUMBER_OF_OBJECTS = 5;
	public Transform PARKING_SLOT;
	public GameObject PARKING_SLOT_PARENT_GAMEOBJECT;
	List<Vector3> Option_Tile_Positions = new List<Vector3>();
	public float scaleDownFactor = .6f;
	int numofRows = 2;
	int numofColumns = 5;
	//	Control controlScript = new Control ();
	// Use this for initialization
	void Start ()
	{
		level = CarGame_FileForPersistantData.level;
		game_name = CarGame_SceneVariables.Game_Name;
		ImageList = new List<String> ();
		PlaceParkingSlots (MAX_NUMBER_OF_OBJECTS);
//		ReadDatabase ();

//		CreateScene (numOptionTile [level]);
	}
	
	// Update is called once per frame
	void Update ()
	{
		EscapeorPause ();
	}

	void EscapeorPause(){
		if (Input.GetKey (KeyCode.Escape)) {
			if (!isPaused) { // if game is not yet paused, ESC will pause it
				isPaused = true;
				PauseGame ();					// definition has to be added to pause the game. 
			} else { // if game is paused and ESC is pressed, it's the second press. QUIT
				ExitGame ();
			}
		}

	}


	// creates scene by setting a backgroud and assigning images to pre placed objects on an empty scene
	void CreateScene (int numOptionTile)
	{
		SetBackground ();
//		SetOptionTiles ();
	}


	// creates scene by setting a backgroud 
	void SetBackground ()
	{
		
	}

	// read the image file names from the database and assigns images to the pre placed empty game objects
	//	after and image is assigne two scripts ImageEffect and Control are enabled for each objects. 
	void SetOptionTiles ()
	{
		// for selecting a category add code here
//		category = "Bus";
//		categoryId = 2;
		string category = GetCategory();
		GameObject.Find (Camera.main.GetComponent<CarGame_SceneVariables>().blockObject).GetComponent<SpriteRenderer> ().sprite = Resources.Load (ImageFolder + category+ "/" + "Block "+ category, typeof(Sprite)) as Sprite;
//		GameObject block = GameObject.Find(SceneVariables
		Debug.Log (category + " is the category");
		var isOutlined = "WithoutOutline/";
		if (Camera.main.GetComponent<CarGame_SceneVariables> ().outline) {
			isOutlined = "WithOutline/";
		}
		ReadDatabaseNew ();
		int i = 0;
		allGameObjects = GameObject.FindGameObjectsWithTag (optionTileTag);
		foreach (GameObject g in allGameObjects) {

			// assign the sprite to each game object here
			Debug.Log("tried at: "+ game_name + "/" + ImageFolder + category + "/" + isOutlined + ImageList [i]);
			g.GetComponent<SpriteRenderer> ().sprite = Resources.Load (game_name + "/" +ImageFolder + category + "/" + isOutlined + ImageList [i], typeof(Sprite)) as Sprite;
//			g.GetComponent<SpriteRenderer> ().sprite = Resources.Load (ImageFolder + category + "/"  + "b1", typeof(Sprite)) as Sprite;
			i++;

			AddScripts (g);
		}

	}

	string GetCategory(){
		var ds = new CarGame_DataService (CarGame_SceneVariables.databaseName);

		var image = ds.GetRandomCategory();
		//		Debug.Log (image);
		foreach (var i in image) {
			categoryId = i.Id;
			Debug.Log ("Category id is: "+categoryId);
			return i.CategoryName;
		}
		return "Car";
	}

	public void AddScripts(GameObject g){
		g.GetComponent<ImageEffect> ().enabled = true;
//		g.GetComponent<Control> ().enabled = true;
//		g.GetComponent<ObjectMovement> ().enabled = true;
	}


	// return the position of game object named value of targetTile
	public Transform GetTargetPositionForTargetTile (string targetTile )
	{
//		Debug.Log(this.gameObject.transform.Find (targetTile).transform);
		return GameObject.Find (targetTile).transform;
	}

	void ExitGame ()
	{
		SceneManager.LoadScene ("HomeScreen");
	}

	void PauseGame ()
	{
		
	}

	void SetOptionTilesNew(){
		string category = GetCategory();
//		GameObject.Find (Camera.main.GetComponent<CarGame_SceneVariables>().blockObject).GetComponent<SpriteRenderer> ().sprite = Resources.Load (game_name + "/"  + ImageFolder + category+ "/" + "Block "+ category, typeof(Sprite)) as Sprite;
		var isOutlined = "WithoutOutline/";
		if (Camera.main.GetComponent<CarGame_SceneVariables> ().outline) {
			isOutlined = "WithOutline/";
		}
		var all_sprites = Resources.LoadAll(ImageFolder + category + "/" + isOutlined,typeof(Sprite));
//		for(
		for (int i = 0; i < MAX_NUMBER_OF_OBJECTS; i++) {
//			Instantiate(optionTile,
		}
	}

	public void PlaceParkingSlots(int n){
//		category = "Bus";
//		categoryId = 2;
		string category = GetCategory();
		GameObject.Find (Camera.main.GetComponent<CarGame_SceneVariables>().blockObject).GetComponent<SpriteRenderer> ().sprite = Resources.Load (game_name + "/"  + ImageFolder + category+ "/" + "Block "+ category, typeof(Sprite)) as Sprite;
		var isOutlined = "WithoutOutline/";
		if (Camera.main.GetComponent<CarGame_SceneVariables> ().outline) {
			isOutlined = "WithOutline/";
		}
		var all_objects = Resources.LoadAll(game_name + "/" + ImageFolder + category + "/" + isOutlined,typeof(Sprite));
		int all_sprites_len = all_objects.Length;
		Sprite[] all_sprites =  new Sprite[all_sprites_len];
		for (int i =0;i< all_sprites_len;i++ ) {
			all_sprites[i] = all_objects[i] as Sprite;
		}
		all_sprites = RandomizingArray.RandomizeSprite (all_sprites).ToArray();
		float height_percentage = .00f, width_percentage = .95f, margin = .05f;
		var percent_for_one_object = (width_percentage - margin) / n;
		var max_allowed_size = 1f;
		var position_for_parking_slot = Shared_ScriptForGeneralFunctions.GetPointOnScreen (width_percentage, height_percentage);
		Debug.Log (percent_for_one_object +"percent_for_one_object");
		var length_span = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width * percent_for_one_object , 0f, Camera.main.nearClipPlane)).x - Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, Camera.main.nearClipPlane)).x;
		for (int i = 0; i < n-1; i++) {
			var option_tile_gameobject = Instantiate (optionTile, position_for_parking_slot, Quaternion.identity);
			option_tile_gameobject.transform.parent = PARKING_SLOT_PARENT_GAMEOBJECT.transform;
			option_tile_gameobject.GetComponent<SpriteRenderer> ().sprite = all_sprites [i] as Sprite;

//			var local_scale_for_one_object = (Screen.width * percent_for_one_object * option_tile_gameobject.transform.localScale.x)/ (option_tile_gameobject.GetComponent<SpriteRenderer>().bounds.size.x * option_tile_gameobject.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
			var local_scale_for_one_object = (length_span * option_tile_gameobject.transform.localScale.x )/ (option_tile_gameobject.GetComponent<SpriteRenderer>().bounds.size.x  );
			local_scale_for_one_object = Mathf.Min (local_scale_for_one_object, max_allowed_size);
			Debug.Log(local_scale_for_one_object + "local_scale_for_one_object");
			option_tile_gameobject.transform.localScale = new Vector3 (local_scale_for_one_object,local_scale_for_one_object,local_scale_for_one_object);
			option_tile_gameobject.tag = CarGame_SceneVariables.matchedTag;
			position_for_parking_slot.x -= option_tile_gameobject.GetComponent<SpriteRenderer>().sprite.bounds.size.x * option_tile_gameobject.transform.localScale.x;
			if (i == 0) {
				Debug.Log ("option_tile_gameobject.GetComponent<SpriteRenderer> ().sprite.bounds.size.y " + option_tile_gameobject.GetComponent<SpriteRenderer> ().sprite.bounds.size.y);
				position_for_parking_slot.y += option_tile_gameobject.GetComponent<SpriteRenderer> ().sprite.bounds.size.y;
			}
			option_tile_gameobject.transform.position = position_for_parking_slot;
			position_for_parking_slot.x -= length_span * .1f;
			option_tile_gameobject.GetComponent<ImageEffect> ().position_in_parking = option_tile_gameobject.transform.position;
		}
//		StartCoroutine(MoveObjectsOutOfParking(numOptionTile[level]));
		StartCoroutine(MoveObjectsOutOfParking(n-2));
	}
	
	public void ShuffleImages (string ImageTag)
	{
		
		GameObject[] GameObjects = GameObject.FindGameObjectsWithTag (ImageTag);
		List<Sprite> tempImageList = new List<Sprite> ();
		foreach (GameObject go in GameObjects) {
			tempImageList.Add (go.GetComponent<SpriteRenderer> ().sprite);
		}
//		RandomizingArray ra = new RandomizingArray ();
		tempImageList = RandomizingArray.RandomizeSprite (tempImageList.ToArray ());
		int i = 0;
		foreach (GameObject go in GameObjects) {
			go.GetComponent<SpriteRenderer> ().sprite = tempImageList [i];
//			go.GetComponent<ImageEffect> ().oldSprite = tempImageList [i];
//			go.GetComponent<Control> ().oldSprite = tempImageList [i];
			i++;
		}
	}


	void ReadDatabaseNew ()
	{
		try {
			var ds = new CarGame_DataService (CarGame_SceneVariables.databaseName);
			//		var images = ds.GetImages();
			//		foreach(var ij in images){
			//			Debug.Log(ij);
			//		}

			var image = ds.GetnImagesFromCategory (numOptionTile [level], categoryId);
//			Debug.Log(image);
			//		Debug.Log (image);
			foreach (var i in image) {
				
				ImageList.Add (i.GetImageName ());
				Debug.Log(i.GetImageName());
			}
			//		Debug.Log(ds.GetImages());
		} catch (Exception e) {
			Debug.Log (e.ToString ());
			scoreText = GameObject.Find ("Score").GetComponent<Text> ();
//			scoreText.text = e.ToString ();
			Debug.Log (e.ToString ());
		}
	}

	IEnumerator MoveObjectsOutOfParking(int numOfObjects){
		DivideScreen ();
		yield return new WaitForSeconds (5f);
		var screen_width = Screen.width;
		var screen_height = Screen.height;
		float height_percentage = .8f, width_percentage = .95f, margin = .05f;
		var percent_for_one_object = (width_percentage - margin) / numofColumns;
		var max_allowed_size = 1f;
//		var length_span = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width * percent_for_one_object , 0f, Camera.main.nearClipPlane)).x - Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, Camera.main.nearClipPlane)).x;
		var length_span = Shared_ScriptForGeneralFunctions.GetPointOnScreen(percent_for_one_object,0f).x - Shared_ScriptForGeneralFunctions.GetPointOnScreen(0f,0f).x;
		Debug.Log ("length span "+ length_span);
		var origin_point = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, Camera.main.nearClipPlane)).x;
		var option_gameobject_height = GetComponent<CarGame_SceneVariables> ().GetPointOnScreen (0, height_percentage);
		var num_of_gameobject_in_parking = PARKING_SLOT_PARENT_GAMEOBJECT.transform.childCount;
		for (int i = 0; i < numOfObjects; i++) {
			var option_tile_gameobject = PARKING_SLOT_PARENT_GAMEOBJECT.transform.GetChild (num_of_gameobject_in_parking - i - 1);
//			option_tile_gameobject.transform.position = new Vector3 (origin_point + ((i + 1) * length_span *.95f) , option_gameobject_height.y, option_gameobject_height.z);
			option_tile_gameobject.transform.position = Option_Tile_Positions[i];
			option_tile_gameobject.tag = CarGame_SceneVariables.OptionTileTag;
//			option_tile_gameobject.transform.parent = null;
//			PARKING_SLOT_PARENT_GAMEOBJECT.transform.GetChild (num_of_gameobject_in_parking - i - 1).GetComponent<
//			AddScripts(PARKING_SLOT_PARENT_GAMEOBJECT.transform.GetChild (num_of_gameobject_in_parking - i - 1).gameObject);
			yield return new WaitForSeconds (.5f);
//			yield return null;

			var local_scale_for_one_object = (length_span*.9f * option_tile_gameobject.transform.localScale.x )/ (option_tile_gameobject.GetComponent<SpriteRenderer>().bounds.size.x  );
			option_tile_gameobject.GetComponent<Scalling>().maxSize = Mathf.Min (local_scale_for_one_object, max_allowed_size);
		}
		yield return new WaitForSeconds (.5f);
		for (int i = 0; i < numOfObjects; i++) {
			AddScripts(PARKING_SLOT_PARENT_GAMEOBJECT.transform.GetChild (num_of_gameobject_in_parking - i - 1).gameObject);
		}
	
	}


	void DivideScreen(){
		Debug.Log ("Screen width: " + Camera.main.pixelWidth + "Screne height: "+ Camera.main.pixelHeight);
		//		var x = 
		var screenWidth = Camera.main.pixelWidth;
		var screenHeight = Camera.main.pixelHeight;
		var xGrad = (screenWidth / (numofColumns + 1) );
		var yGrad = ((screenHeight /(2f* (numofRows + 1 ))));
		var dist = Vector3.Distance(Camera.main.ScreenToWorldPoint (new Vector3 (0,  0, Camera.main.nearClipPlane)),Camera.main.ScreenToWorldPoint (new Vector3 (0,  yGrad, Camera.main.nearClipPlane))) * scaleDownFactor;
		//		maxSize = dist * scaleDownFactor;
		Debug.Log (dist + " far apart ");
		var cueRow = UnityEngine.Random.Range (1, numofRows);
		//		Debug.Log ("screen positions are for Level " + GameObject.Find (SceneVariables.permanentGameObject).GetComponent<PersistantDataOnLoad> ().level);
		for (int i = 1; i <= numofColumns; i++) {
			for (int j = 1; j <= numofRows; j++) {
				Vector3 location = Camera.main.ScreenToWorldPoint (new Vector3 (i*xGrad,(screenHeight/2) +j*yGrad, Camera.main.nearClipPlane + 100f));
				Option_Tile_Positions.Add (location);
			}
		}
		Option_Tile_Positions = RandomizingArray.RandomizeVector3 (Option_Tile_Positions);
	}
}

