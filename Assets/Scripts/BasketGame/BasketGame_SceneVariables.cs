using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BasketGame_SceneVariables : MonoBehaviour {

    //* [Constant and static variables [shared throughout the games, like tags, database related variables, layer names, global variables, important spawn positions, foldernames etc ..]]
    public const string Game_Name = "BasketGame";
    //public const string masterGO = "MasterGameObject";
    public const int VALUE_FOR_PRE_LEVEL_COMPLETE = 1;
    public const string DATABASE_NAME = "WorkingMemoryGames_DB1.db";
    public static string basketTag = "BasketTag";
    public static string fruitTag ="FruitTag";
    public static string fullBasket = "FullBasketTag";
    public static string bubbleTag = "BubbleTag";
    public static string hangingFruitTag = "HangingFruitTag";
    public static string inBasketFruitTag = "InBasketFruitTag";
    public static string baloonTag = "BaloonTag";
	public const string eatenFruitTag = "EatenFruitTag";
	public string RAIN_PARTICLE_SYSTEM_TAG = "RainParticleSystemTag";
    public static Vector3 initVector = new Vector3(-5.6f,-1.5f,0f);
    public static string basketFolderName = "Basket/", fruitFolderName = "Fruits/", withOutline = "WithOutline/", withoutOutline = "WithoutOutline/";

    public int basketCapacity = 15; 				// max number of fruit in a basket
	public static float waitTime = .05f;
	public static string targetObject = "TargetObject";
	public bool isOutline = false;
    public static float minDistance = .1f;                                  // min distance after which the position of object is set equal to target

    // itween path names
    public string[] paths = new string[]{
		"FruitPath1",
		"FruitPath2"
	};
    // basket image names
	public static string[] baskets = new string[]{ 
//		"Bluebasket",
		"redbasket",
		"orangebasket",
		"yellowbasket",
//		"Brownbasket",
		"purplebasket",
		"greenbasket"
	
	};

    // fruit image names
	public static string[] fruits = new string[] {
		"Apple",
		"Banana",
		"Beetroot",
//		"Carrot",
//		"Cherry",
		"GreenApple",
		"Lemon",
		"Orange",
//		"Pumpkin",
//		"Tomato"
	};

	// a map as to which fruit should be captured in which basket
	public  Dictionary <string, string[]> objectColorMap = new Dictionary<string, string[]>{
//		{"Bluebasket",new string[]{}},
		{"redbasket",new string []{"Apple"}},
		{"orangebasket",new string[] {"Orange1"}},
		{"yellowbasket",new string[] {"Banana1", "Lemon1"}},
		{"purplebasket",new string[] {"Beetroot"}},
		{"greenbasket",new string[] {"GreenApple"}},
        //{"colored", new string[] {""} }
//		{"Brownbasket",new string[] {}},
	
	};
	// Use this for initialization
	void Start () {
//		initVector = GameObject.Find ("InitBubble").gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // function that returns n baskets of different color
	public static string[] GetBaskets(int n){
//		RandomizingArray ra = new RandomizingArray ();
		List <string> localBaskets = RandomizingArray.RandomizeStrings(baskets);
////		localBaskets = ra.RandomizeStrings (baskets);
//		localBaskets = baskets.ToList();
		string[] localBasktetsArray = new string[n];
		for (int i = 0; i < n; i++) {
			localBasktetsArray [i] = localBaskets [i];
		}
		return localBasktetsArray;
	}

	public string GetFruitFolderName(){
		if (isOutline) {
			return fruitFolderName + withOutline;
		}
		return fruitFolderName + withoutOutline;
	}

	public string GetBasketFolderName(){

		if (isOutline) {
			return basketFolderName + withOutline;
		}
		return basketFolderName + withoutOutline;
	}

	public string GetColoredFruit(){
//		Debug.Log ("till here");
		string fruitColor = GetEmptyBasket();
//		Debug.Log (fruitColor);
		var fruits = objectColorMap [fruitColor];
		int x = SafeRandom (0, fruits.Length);
		return fruits [x];
//		return fruits [i];
	}

	public string GetColoredFruit(string basket_color){
		//		Debug.Log ("till here");
		var fruits = objectColorMap [basket_color];
		int x = SafeRandom (0, fruits.Length);
		return fruits [x];
		//		return fruits [i];
	}

	public string GetEmptyBasket(){
		GameObject[] go = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag);
		List <string> emptyBaskets = new List <string> {};
		foreach (GameObject g in go) {
			if (g.GetComponent<BasketBehavior> ().spaceLeft ()) {
				emptyBaskets.Add (g.GetComponent<BasketBehavior> ().GetBasketName());
			}
//			Debug.Log (g.GetComponent<BasketBehavior> ().basketName);
		}
		int x = SafeRandom (0, emptyBaskets.Count);
		return emptyBaskets [x];
	}

	public string GetPath(){
//		return paths [SafeRandom (0, paths.Length)];
		return paths[0];
	}

	public int SafeRandom(int start,int end){
		if(end-start > 1){
			return Random.Range(start,end);
		}
		return start;
	}


	public Vector3 GetPointOnScreen(float width_percentage, float height_percentage){
		var screen_height = Camera.main.pixelHeight;
		var screen_width = Camera.main.pixelWidth;
		return Camera.main.ScreenToWorldPoint (new Vector3 (screen_width * width_percentage, screen_height * height_percentage, Camera.main.nearClipPlane));

	}

	float GetNormalizedWidth( float blockPercent, float blockNumbers){
		var screen_width = Camera.main.pixelWidth;
		var screen_height = Camera.main.pixelHeight;
		var width = Camera.main.ScreenToWorldPoint (new Vector3 (screen_width, screen_height, Camera.main.nearClipPlane)).x - Camera.main.ScreenToWorldPoint (new Vector3 (0, screen_height, Camera.main.nearClipPlane)).x;
		Debug.Log ("value is " + (width - (blockNumbers * blockPercent)) / (width * 2.0f));
		return Mathf.Max(.1f,(width - (blockNumbers * blockPercent)) / (width *2.0f ));
	}
}
