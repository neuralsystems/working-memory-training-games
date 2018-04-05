using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneVariables : MonoBehaviour {
	
	// database related variables

	public string DATABASE_NAME = "PianoPlay.db"; 										// name of the sqlite database
	public string Game_Name = "PianoGame";
	// tags used in the game
	public string KEY_SQUARE_TAG = "KeySquareTag";										// tag for the keys spawned when computer presses a piano key
	public string SAMPLE_SQUARE_TAG = "SampleSquareTag";									// not used yet and not sure why added this tag
	public string USER_INPUT_SQUARE_TAG = "UserInputSquareTag";							// tag for the keys spawned when user presses a piano key
	public string REWARD_SQUARE_TAG = "RewardSquareTag";									// // tag for the square spawned at the top of screen 
	public string RAIN_PARTICLE_SYSTEM_TAG = "RainParticleSystemTag";
	public string REWARD_SQUARE_CHILD_TAG = "RewardSquareChildTag";

	// gameobject position related variables 
	float heightPercentageForRewardSquare = .95f, widthPercentageForRewardSquare = .50f; // for position of squares on top
	public float widthPercentage, heightPercentage;										// not used yet 


	// gameplay related variables 
	public static bool IS_PRESSED = false;												// to check that only one key is presses = true if any key is pressed, false otherwise
	public static float PLAY_TIME = .3f;													// time in sec for which a frequency is played	
	public static float WAIT_TIME = .5f;													// minimum time gap between two consequtive press 
	public static float delayAfterCorrect = 2.0f, delayAfterIncorrect = 2.0f;			// not used yet and not sure why added	
	public static string USER_STRING = "";												// not used yet and not sure why added
	public static bool IS_USER_MODE = false;												// to chek if press can be enabled for the user, is false when the computer is playing a tune, true at other time
	public static Color PRESSED_COLOR = new Color (0.1f, 0.0f, 0.5f, 1f);				// color of the key when it is in pressed state
	// these variable were used earlier however now they are not part of the active gameplay but removing them will cause error
	public static float SPEED = 10.0f;													
	public static float DELAY_TO_START=.5f;
	public static float DELAY_TO_MOVE = .0001f;
	public static float INITIAL_ANGLE = 90;
	public static float stepSize = 2.14f;
	public static bool IS_READY = true;
	public static int error_count = 0, max_allowed_error = 5, min_tone_length = 2;		// used to make the game adaptive by counting the userś performace as # of consequtive correct or incorrect
	public static float MIN_DISTANCE = 0.1f;												// if an object is MIN_DISTANCE away from target we will set it's postion equal to target position (see move functions)
	public bool correctMatch = false;
	public int level = 1;
	public int CONSECUTIVE_CORRECT_THRESHOLD = 5;
	public string playSound = "PlaySound";
	public int REWARD_INDEX = 0;
	public string questionSquare = "Question_square";
	public float width;
	public float SCREEN_WIDTH ;
	public float SCREEN_HEIGHT ;
//	static string smileName ="Smile", sadName ="Sad", neutralName = "Neutral";




	// gameobject specific variables (eg: name of a specific gameobject, position for an spawned object)
	public string SAMPLE_SQUARE_PARENT = "SampleSquaresParent";
	public string REWARD_SQUARE_PARENT = "RewardSquareParent";
	public string USER_INPUT_SQUARE_PARENT = "UserInputSquareParent";
	public Vector3 target, targetRewardSquare, targetUserSquare ;
	public Transform rewardSquare;


	// animation related variables 
	public string tappingHand = "TappingHand";
	public string fingerTapAnimation = "FingerTapAnimation";

	// AudioSource related variables
	public AudioClip[] Audio_Clips = new AudioClip[2] ;

	void Awake(){
		Reset ();
	}

	// Use this for initialization
	void Start () {
//		ResetSmilies ();
//				source = GameObject.Find("Source").transform.position;
		SCREEN_WIDTH = Camera.main.pixelWidth;
		SCREEN_HEIGHT = Camera.main.pixelHeight;

//		Debug.Log (source);

		DivideScreen ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void DivideScreen(){

		var widthInPixels = GameObject.Find(questionSquare).GetComponent<SpriteRenderer> ().bounds.size.x;
		Debug.Log (width + " "+widthInPixels+ " "+Camera.main.GetComponent<PlayTone>().GetTuneLength());
		var normalizedWidth = GetNormalizedWidth ( widthInPixels, Camera.main.GetComponent<PlayTone>().GetTuneLength());
//		Debug.Log ("normalizedWidth "+normalizedWidth);
		target = Camera.main.ScreenToWorldPoint (new Vector3 (SCREEN_WIDTH* normalizedWidth, SCREEN_HEIGHT * heightPercentage, 100f));
		targetUserSquare = target;
	}

	public IEnumerator PlaceEmptyRewardSquare(int n){
		
		GameObject[] oldSquares = GameObject.FindGameObjectsWithTag (REWARD_SQUARE_TAG);
		foreach (GameObject oldSquare in oldSquares) {
			Destroy (oldSquare);
		}
		var SCREEN_WIDTH = Camera.main.pixelWidth;
		var SCREEN_HEIGHT = Camera.main.pixelHeight;
//		targetRewardSquare = Camera.main.ScreenToWorldPoint (new Vector3 (SCREEN_WIDTH* widthPercentageForRewardSquare, SCREEN_HEIGHT * heightPercentageForRewardSquare, Camera.main.nearClipPlane));
//		Debug.Log ("targetRewardSquare "+targetRewardSquare);
		var originalPosition = targetRewardSquare;
		var alredayPresent = GameObject.FindGameObjectsWithTag (REWARD_SQUARE_TAG).Length;
		//		target.x += shift * alredayPresent;
//		tag = rewardSquareTag;
//		StartCoroutine(MoveRewardToTarget 
		for (int i = 0; i < n; i++) {
			var rs = Instantiate (rewardSquare, targetRewardSquare, Quaternion.identity);
			if (i == 0) {
				var widthInPixels = rs.GetComponent<SpriteRenderer> ().bounds.size.x;
//				Debug.Log (widthInPixels*n+ " "+SCREEN_WIDTH);
//				GameObject.Find (REWARD_SQUARE_PARENT).transform.position = rs.transform.position;
				var normalizedWidth = GetNormalizedWidth ( widthInPixels, n);
				Debug.Log (normalizedWidth+" "+ width);
				targetRewardSquare = Camera.main.ScreenToWorldPoint (new Vector3 (SCREEN_WIDTH* normalizedWidth, SCREEN_HEIGHT * heightPercentageForRewardSquare, Camera.main.nearClipPlane));
				rs.transform.position = targetRewardSquare;
				originalPosition = targetRewardSquare;
//				Debug.Log ("targetRewardSquare1 "+targetRewardSquare);
			}
			rs.transform.parent = GameObject.Find (REWARD_SQUARE_PARENT).transform;
			rs.gameObject.tag = REWARD_SQUARE_TAG;
//			Debug.Log (rs.GetComponent<SpriteRenderer> ().bounds.size.x);
			targetRewardSquare.x += rs.GetComponent<SpriteRenderer> ().bounds.size.x;
			if(targetRewardSquare.x > Camera.main.ScreenToWorldPoint (new Vector3 (SCREEN_WIDTH* .9f, SCREEN_HEIGHT , Camera.main.nearClipPlane)).x){
				targetRewardSquare.x = originalPosition.x;
				targetRewardSquare.y -= rs.GetComponent<SpriteRenderer> ().bounds.size.y;
			}
			yield return null;
		}
	}

	float GetNormalizedWidth( float blockPercent, float blockNumbers){
		SCREEN_WIDTH = Camera.main.pixelWidth;
		SCREEN_HEIGHT = Camera.main.pixelHeight;
		var width = Camera.main.ScreenToWorldPoint (new Vector3 (SCREEN_WIDTH, SCREEN_HEIGHT * heightPercentageForRewardSquare, Camera.main.nearClipPlane)).x - Camera.main.ScreenToWorldPoint (new Vector3 (0, SCREEN_HEIGHT * heightPercentageForRewardSquare, Camera.main.nearClipPlane)).x;
		Debug.Log ("value is " + (width - (blockNumbers * blockPercent)) / (width * 2.0f));
		return Mathf.Max(.1f,(width - (blockNumbers * blockPercent)) / (width * 2.0f ));
	}

	public Vector3 GetPointOnScreen(float width_percentage, float height_percentage){
		var screen_height = Camera.main.pixelHeight;
		var screen_width = Camera.main.pixelWidth;
		return Camera.main.ScreenToWorldPoint (new Vector3 (screen_width * width_percentage, screen_height * height_percentage, Camera.main.nearClipPlane));

	}

	public void ShowRewardSquares(){
		var userSquares = GameObject.FindGameObjectsWithTag (USER_INPUT_SQUARE_TAG);
		var x = REWARD_INDEX;
		var reward_square_parent_object = GameObject.Find (REWARD_SQUARE_PARENT);
		foreach (var userSquare in userSquares) {
//			GameObject.Find (REWARD_SQUARE_PARENT).transform.GetChild (x).GetComponent<SpriteRenderer>().sprite =userSquare.GetComponent<SpriteRenderer>().sprite;
//			REWARD_INDEX++;
//			StartCoroutine(userSquare.GetComponent<KeySquareBehavior>().MoveToTarget(reward_square_parent_object.transform.GetChild (x).transform.localPosition));
			userSquare.transform.parent = null;
//			StartCoroutine(Camera.main.GetComponent<Shared_ScriptForGeneralFunctions>().MoveToTarget(userSquare, reward_square_parent_object.transform.GetChild(x).transform.position));
			StartCoroutine(userSquare.GetComponent<KeySquareBehavior>().MoveToReward(reward_square_parent_object.transform.GetChild(x).gameObject));
			x++;
		}
		REWARD_INDEX += 1;
		GameObject.Find (USER_INPUT_SQUARE_PARENT).GetComponent<PianoGame_UserInputSquareParentBehavior> ().ResetPosition ();
	}

	public void ShowSquares(){
		var sample_squares = GameObject.FindGameObjectsWithTag (SAMPLE_SQUARE_TAG);
		foreach (var sample_square in sample_squares) {
			sample_square.GetComponent<KeySquareBehavior> ().ResetSquare ();
		}
		ShowRewardSquares ();
	}
	public void Reset(){
		REWARD_INDEX = 0;
		IS_USER_MODE = false;	
		IS_READY = true;
		GameObject.Find (REWARD_SQUARE_PARENT).transform.position = GetPointOnScreen (.5f, .9f);
	}

	public void GetRandomClapping(){
		var r_x = Random.Range (0, Audio_Clips.Length);
//		Audio_Clips.cl
		GetComponent<AudioSource> ().PlayOneShot(Audio_Clips [r_x]);
	}


}
