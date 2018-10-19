using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneVariables : MonoBehaviour {
	
	// database related variables

	public const string DATABASE_NAME = "WorkingMemoryGames_DB1.db"; 										// name of the sqlite database
	public const string Game_Name = "PianoGame";
    //public const string masterGO = "MasterGameObject";
    // tags used in the game
    public string KEY_SQUARE_TAG = "KeySquareTag";										// tag for the keys spawned when computer presses a piano key
	public string SAMPLE_SQUARE_TAG = "SampleSquareTag";									// not used yet and not sure why added this tag
	public string USER_INPUT_SQUARE_TAG = "UserInputSquareTag";             // tag for the keys spawned when user presses a piano key
    public string REWARD_SQUARE_TAG = "RewardSquareTag";									// // tag for the square spawned at the top of screen 
	public string NON_REWARD_SQUARE_TAG = "NonRewardSquareTag";
    public string REWARD_SQUARE_UI_TAG = "RewardSquareUITag";
    public string REWARD_SQUARE_UI_PARENT_TAG = "RewardSquareUIParent";
    public string KEY_SQUARE_UI_TAG = "KeySquareUITag";									
    public string RAIN_PARTICLE_SYSTEM_TAG = "RainParticleSystemTag";
	public string REWARD_SQUARE_CHILD_TAG = "RewardSquareChildTag";
	public const string PIANO_KEY_TAG = "PianoKeyTag";
    public Transform contentPanel;
	// gameobject position related variables 
	public float heightPercentageForRewardSquare = .9f, widthPercentageForRewardSquare = .50f; // for position of squares on top
    public float widthPercentage, heightPercentage;										// not used yet 
    public const int DEFAULT_VALUE_KEEP_VISIBLE = 1;

    // gameplay related variables 
    public static bool IS_PRESSED = false;												// to check that only one key is presses = true if any key is pressed, false otherwise
	public static float PLAY_TIME = .5f;													// time in sec for which a frequency is played	
	public static float WAIT_TIME = .6f;													// minimum time gap between two consequtive press 
	public static float delayAfterCorrect = 2.0f, delayAfterIncorrect = 2.0f;			// not used yet and not sure why added	
	public static string USER_STRING = "";												// not used yet and not sure why added
	public static bool IS_USER_MODE = false;												// to chek if press can be enabled for the user, is false when the computer is playing a tune, true at other time
	public static Color PRESSED_COLOR = new Color (0.5f, 0.5f, 0.5f, 1f);				// color of the key when it is in pressed state
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
    public GameObject sound_manager;
//	static string smileName ="Smile", sadName ="Sad", neutralName = "Neutral";




	// gameobject specific variables (eg: name of a specific gameobject, position for an spawned object)
	public string SAMPLE_SQUARE_PARENT = "SampleSquaresParent";
	public string REWARD_SQUARE_PARENT = "RewardSquareParent";
	public string USER_INPUT_SQUARE_PARENT = "UserInputSquareParent";
    public string REWARD_SQUARE_UI_SCROLL = "RewardSquareScroll";
    public Vector3 target, targetRewardSquare, targetUserSquare ;
	public Transform rewardSquare;
    public Transform reward_content_panel;


	// animation related variables 
	public string tappingHand = "TappingHand";
	public string fingerTapAnimation = "FingerTapAnimation";

	// AudioSource related variables
	public AudioClip[] Audio_Clips = new AudioClip[2] ;

    public GameObject RewardPoolObject;
    public GameObject RewardUIPoolObject;
    void Awake(){
		Reset ();
	}

	// Use this for initialization
	void Start () {
		SCREEN_WIDTH = Camera.main.pixelWidth;
		SCREEN_HEIGHT = Camera.main.pixelHeight;
		DivideScreen ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void DivideScreen(){

		var widthInPixels = GameObject.Find(questionSquare).GetComponent<SpriteRenderer> ().bounds.size.x;
		Debug.Log (width + " "+widthInPixels+ " "+Camera.main.GetComponent<PlayTone>().GetTuneLength());
		var normalizedWidth = GetNormalizedWidth ( widthInPixels, Camera.main.GetComponent<PlayTone>().GetTuneLength());
		target = Camera.main.ScreenToWorldPoint (new Vector3 (SCREEN_WIDTH* normalizedWidth, SCREEN_HEIGHT * heightPercentage, 100f));
		targetUserSquare = target;
	}

	public IEnumerator PlaceEmptyRewardSquare(int n){
		
		GameObject[] oldSquares = GameObject.FindGameObjectsWithTag (REWARD_SQUARE_TAG);
		foreach (GameObject old_square_object in oldSquares) {
			old_square_object.GetComponent<PianoGame_RewardSquareBehavior> ().SetVisibility(true);
            old_square_object.tag = NON_REWARD_SQUARE_TAG;
			foreach (Transform child in old_square_object.transform) {
				Destroy (child.gameObject);
			}
		}
		Debug.Log("already have "+ oldSquares.Length + "squares ");
		var SCREEN_WIDTH = Camera.main.pixelWidth;
		var SCREEN_HEIGHT = Camera.main.pixelHeight;
		var init_position = Shared_ScriptForGeneralFunctions.GetPointOnScreen (1, heightPercentageForRewardSquare);
		var originalPosition = targetRewardSquare;
		var reward_square_parent_gameobject = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().REWARD_SQUARE_PARENT);
        var size_of_one_object = rewardSquare.GetComponent<SpriteRenderer> ().bounds.size;
		int old =  oldSquares.Length;
		var widthInPixels = size_of_one_object.x;
		var normalizedWidth = GetNormalizedWidth ( widthInPixels, n);
        var maxVisibleRewardSq = reward_square_parent_gameobject.GetComponent<PG_RewardSquareParentBehavior>().MaximumVisibleRewardSquares();
        Debug.Log (normalizedWidth+" "+ width);
		targetRewardSquare = Shared_ScriptForGeneralFunctions.GetPointOnScreen(normalizedWidth/2, heightPercentageForRewardSquare);
		originalPosition = targetRewardSquare;
		for (int i = 0; i < n; i++) {
			if (i < old) {
                Debug.Log("moving old ones " + (i == n - 1));
				StartCoroutine (oldSquares [i].GetComponent<PianoGame_RewardSquareBehavior> ().MoveToTarget (targetRewardSquare, i == n-1, REWARD_SQUARE_TAG));
			} else {
				if (i == old) {
					yield return new WaitForSeconds (1f);
				}
                //var rs = Instantiate (rewardSquare, init_position, Quaternion.identity);
                var rs = RewardPoolObject.GetComponent<SimpleObjectPool>().GetObject();
                rs.transform.position = init_position;
                StartCoroutine(rs.GetComponent<PianoGame_RewardSquareBehavior>().MoveToTarget(targetRewardSquare, i == n - 1, REWARD_SQUARE_TAG));
//				rs.gameObject.tag = REWARD_SQUARE_TAG;
//				rs.transform.localScale = reward_square_parent_gameobject.transform.localScale;
				rs.transform.parent = reward_square_parent_gameobject.transform;
				init_position.x += size_of_one_object.x; 
			}
//			yield return new WaitForSeconds (1f);
//			Debug.Log (rs.transform.localScale + " is the size before");
//			rs.transform.parent = GameObject.Find (REWARD_SQUARE_PARENT).transform;
//			Debug.Log (rs.transform.localScale + " is the size after");
			targetRewardSquare.x += size_of_one_object.x;
//			if(targetRewardSquare.x > Camera.main.ScreenToWorldPoint (new Vector3 (SCREEN_WIDTH* .95f, SCREEN_HEIGHT , Camera.main.nearClipPlane)).x){
//				targetRewardSquare.x = originalPosition.x;
//				targetRewardSquare.y -= size_of_one_object.y;
//			}

            yield return null;
		}
        for (int i = n; i < old; i++)
        {
            RewardPoolObject.GetComponent<SimpleObjectPool>().ReturnObject(oldSquares[i]);
        }
        //		GameObject.Find (Camera.main.GetComponent<SceneVariables> ().playSound).GetComponent<HomeScreenButtons> ().SetHaloToggle(true);
        
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

	public IEnumerator ShowRewardSquares(){
		var userSquares = GameObject.FindGameObjectsWithTag (USER_INPUT_SQUARE_TAG);
        var keySquaresUI = GameObject.FindGameObjectsWithTag(KEY_SQUARE_UI_TAG);
		var x = REWARD_INDEX;
		var reward_square_parent_object = GameObject.Find (REWARD_SQUARE_PARENT);
        var master_go = GameObject.Find(Shared_Scenevariables.masterGO);
        var overlap = master_go.GetComponent<Shared_PersistentScript>().GetNewPianoGameLevelDetails().GetDifference();
		int last = userSquares.Length - 1;
        var user_square_parent_go = GameObject.Find(USER_INPUT_SQUARE_PARENT).gameObject;
        user_square_parent_go.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(sound_manager.GetComponent<SoundManager_Script>().PlayHappySound());

        var n = reward_square_parent_object.transform.childCount;
        var maxVisibleRewardSq = reward_square_parent_object.GetComponent<PG_RewardSquareParentBehavior>().MaximumVisibleRewardSquares();
        if (n > maxVisibleRewardSq && x!=0) //shift squares to visibility
        {
            yield return StartCoroutine(GameObject.Find(REWARD_SQUARE_UI_SCROLL).GetComponent<RewardSquareUIScrollBehavior>().ScrollTo(REWARD_INDEX, n, maxVisibleRewardSq));
            reward_square_parent_object.GetComponent<PG_RewardSquareParentBehavior>().SnapTo(REWARD_INDEX, n, maxVisibleRewardSq);
        }

        for (int i=0;i<=last;i++){
//		foreach (var userSquare in userSquares) {
			userSquares[i].transform.parent = null;
			var shouldCall = false;
			if (i == (last)) {
				shouldCall = true;
			}
            StartCoroutine(userSquares[i].GetComponent<KeySquareBehavior>().MoveToReward(reward_square_parent_object.transform.GetChild(x).gameObject, keySquaresUI[x], shouldCall));
            //keySquaresUI[x].GetComponent<KeySquareUIBehavior>().SetImage(userSquares[i].GetComponent<SpriteRenderer>().sprite);
            //StartCoroutine(userSquares[i].GetComponent<KeySquareBehavior>().MoveToReward(reward_content_panel.GetChild(x).gameObject, shouldCall));
            x++;
		}
        REWARD_INDEX = x - overlap;
		GameObject.Find (USER_INPUT_SQUARE_PARENT).GetComponent<PianoGame_UserInputSquareParentBehavior> ().ResetPosition ();
	}

	public void ShowSquares(){
		var sample_squares = GameObject.FindGameObjectsWithTag (SAMPLE_SQUARE_TAG);
		foreach (var sample_square in sample_squares) {
			sample_square.GetComponent<KeySquareBehavior> ().ResetSquare ();
		}
		StartCoroutine(ShowRewardSquares ());
	}

	public void Reset(){
		REWARD_INDEX = 0;
		IS_USER_MODE = false;	
		IS_READY = true;
        error_count = 0;
        OnKeyPress.numOfKeysPressed = 0;
		GameObject.Find (REWARD_SQUARE_PARENT).transform.position = GetPointOnScreen (widthPercentageForRewardSquare, heightPercentageForRewardSquare);
	}

	public IEnumerator GetRandomClapping(){
		var r_x = Random.Range (0, Audio_Clips.Length);
		GetComponent<AudioSource> ().PlayOneShot(Audio_Clips [r_x]);
		yield return new WaitForSeconds (Audio_Clips [r_x].length); 
	}
}
