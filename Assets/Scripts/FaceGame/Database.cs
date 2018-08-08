using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public string scenes_map = "Map";
    public string scenes_gameStart = "GameStart";
	public string scenes_level = "Level";
    public string scenes_basic = "Basic";
    //public const string masterGo = "MasterGameObject";

    public string tagsAndNames_sqliteDB = "WorkingMemoryGames_DB1.db";
	public string tagsAndNames_background = "Background";
	public string tagsAndNames_tempBackground = "TempBackground";
	public string tagsAndNames_mapPanel = "MapPanel";
	public string tagsAndNames_shiftMap = "ShiftMap";
    public string tagsAndNames_progressBar = "ProgressBarLabelFollow";
    public string tagsAndNames_wrongChoice = "WrongChoice";
    public string tagsAndNames_rightChoice = "RightChoice";
    public string tagsAndNames_portals = "Portal";
    public string tagsAndNames_unlocked = "Unlocked";
    public string tagsAndNames_changeScene = "ChangeScene";
	public string tagsAndNames_option = "Option";
	public string tagsAndNames_cover = "Cover";

    public float constants_faceShownForSeconds = 5.0f;
    public float constants_rainingTime = 5.0f;
    public float constants_rainingIntervalTime = 0.1f;
    public float constants_smoothTime = 0.3F;
    public float constants_blinkTime = 0.3f;
    public float constants_faceTimeDecr = 0.3f;
    public float constants_faceBaseOffset = -6.2f;
	public float constants_faceBaseOffset2 = -2.4f;
	public float constants_faceBaseOffset3 = -5f;

    public float constants_scaleToRadiusConversionFactor = 5.2f;
    public float constants_sizeToScaleConversionFactor = 0.08f;
    public float constants_faceComponentScale = 1f;

    public float constants_fadePerSecond = 2.5f;
    public float constants_popSpeed = 3f;
    public float constants_wobbleDeltaScale = 0.04f;
    public float constants_wobbleGrowFactor = 0.2f;
    public float constants_transitionSpeed = 10f;
    public float constants_diminishSpeed = 1f;
	public float constants_epsilon = 0.001f;

	public float viewPortWidth = 1f;
	public float viewPortHeight = 1f;
    public int constants_rainingPositionsCount = 40;
    public int constants_noOfFaceComponents = 3;
    public int constants_NO_OF_OPTIONS = 3;
	public int constants_NO_OF_COMPONENTS_LV1 = 7;

	public bool ifRight = true;
	public bool ifOptionSelected = false;
	public bool fadeIn = false;
	public bool fadeOut = false;
	public string user = "pk";

	public Vector3 constants_preLevOptionPos = new Vector3(6f, -12f);
	public Vector3 constants_optionBGShift = new Vector3(0.33f, -0.16f, -1.27f);
	public Vector3 constants_stdScale = new Vector3(1f, 1f);
	public List<Vector3> constants_optionPosLevel = new List<Vector3>(){
            new Vector3(2.3f,-1.8f,0), new Vector3(3.4f,2.5f,0), new Vector3(6.7f,-0.4f,0)
    };
	public List<Vector3> constants_optionPosBasic = new List<Vector3>()
    {
		new Vector3(3.8f,2f,0), new Vector3(5.6f,-2.6f,0), new Vector3(0.6f,-1.8f,0)
	};
    
	public Color32 constants_faceShadedColor = new Color32(207, 196, 182, 255);
	public Color32 constants_noColor = new Color32(255, 255, 255, 255);
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