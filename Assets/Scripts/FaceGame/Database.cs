using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public const string scenes_map = "Map";
    public const string scenes_gameStart = "GameStart";
	public const string scenes_level = "Level";
    public const string scenes_basic = "Basic";
    //public const string masterGo = "MasterGameObject";

    public const string tagsAndNames_sqliteDB = "WorkingMemoryGames_DB1.db";
	public const string tagsAndNames_background = "Background";
	public const string tagsAndNames_tempBackground = "TempBackground";
	public const string tagsAndNames_mapPanel = "MapPanel";
	public const string tagsAndNames_shiftMap = "ShiftMap";
    public const string tagsAndNames_progressBar = "ProgressBarLabelFollow";
    public const string tagsAndNames_wrongChoice = "WrongChoice";
    public const string tagsAndNames_rightChoice = "RightChoice";
    public const string tagsAndNames_portals = "Portal";
    public const string tagsAndNames_unlocked = "Unlocked";
    public const string tagsAndNames_changeScene = "ChangeScene";
	public const string tagsAndNames_option = "Option";
	public const string tagsAndNames_cover = "Cover";
    public const string tagsAndNames_optionTag = "OptionTag";
    public const string tagsAndNames_levelButton = "Level ";

    public const float constants_faceShownForSeconds = 5.0f;
    public const float constants_rainingTime = 5.0f;
    public const float constants_rainingIntervalTime = 0.1f;
    public const float constants_smoothTime = 0.3F;
    public const float constants_blinkTime = 0.3f;
    public const float constants_faceTimeDecr = 0.3f;
    public const float constants_faceBaseOffset = -6.2f;
	public const float constants_faceBaseOffset2 = -2.4f;
	public const float constants_faceBaseOffset3 = -5f;

    public const float constants_scaleToRadiusConversionFactor = 5.2f;
    public const float constants_sizeToScaleConversionFactor = 0.08f;
    public const float constants_faceComponentScale = 1f;

    public const float constants_fadePerSecond = 2.5f;
    public const float constants_popSpeed = 3f;
    public const float constants_wobbleDeltaScale = 0.04f;
    public const float constants_wobbleGrowFactor = 0.2f;
    public const float constants_transitionSpeed = 10f;
    public const float constants_diminishSpeed = 1f;
    public const float constants_epsilon = 0.001f;

    public const float viewPortWidth = 1f;
    public const float viewPortHeight = 1f;
    public const int constants_rainingPositionsCount = 40;
    public const int constants_noOfFaceComponents = 3;
    public const int constants_NO_OF_OPTIONS = 3;
	public const int constants_NO_OF_COMPONENTS_LV1 = 7;

	public bool ifRight = true;
	public bool ifOptionSelected = false;
	public bool fadeIn = false;
	public bool fadeOut = false;
	public string user = "pk";

    public static Vector3 constants_preLevOptionPos = new Vector3(6f, -12f);
    public static Vector3 constants_stdScale = new Vector3(1f, 1f);
	public static List<Vector3> constants_optionBGShift = new List<Vector3>() {
        new Vector3(0.33f, 0f, -1.27f), new Vector3(0.33f, 0f, -1.27f), new Vector3(1.48f, 0.4f, -1.0f), new Vector3(0.33f, 0.33f, -0.95f)
    };
    public static List<Vector3> constants_optionPosLevel = new List<Vector3>(){
            new Vector3(1.9f,-1.8f,0), new Vector3(2.7f,2.5f,0), new Vector3(6.1f,-0.4f,0)
    };
	public static List<Vector3> constants_optionPosBasic = new List<Vector3>()
    {
		new Vector3(3.8f,2f,0), new Vector3(5.6f,-2.6f,0), new Vector3(0.6f,-1.8f,0)
	};
    
	public static Color32 constants_faceShadedColor = new Color32(207, 196, 182, 255);
    public static Color32 constants_noColor = new Color32(255, 255, 255, 255);
	private static Database databaseInstance;

    void Awake()
    {
        DontDestroyOnLoad(this);

		if (databaseInstance == null)
        {
			databaseInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
//camera.main.pixelHeight
//screentoworldpoint