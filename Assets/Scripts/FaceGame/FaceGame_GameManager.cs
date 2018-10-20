using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FaceGame_GameManager : MonoBehaviour
{
	//Start() >> Levels() >> GameStart() >> LevelComplete()

	// PRELEVEL
    // 0 : single option single face (modal)
    // 1 : single face match practice
    // 2 : double face match practice (with tutorial - i,e., modal being shown)
    // 3 : no tutorial-- a mimic of the main game : cover and uncover modal

	private Database database = new Database();
	private float faceTime = 5f;
	public float totalWaitingTime = 5f;
	private int score = 0;
	private int mainIndex = -1;
	private int level;
	private int progressInSubLevel = 1;
	private FaceGame_DataService dataController;
	private FaceGame_GameData gameData;
	private float deltaProgress;
	public string user;
	private FaceGame_GameData levelData;
	private bool tutorial = false;
	private bool preLevel = true;
	private int repeat = 0;

	//Prefabs used for dynamic instantiation
	public GameObject itemPrefab;
	public GameObject faceComponentsPrefab;
	public Transform faceComponentPrefab;
	public SpriteRenderer modalPrefab;
	public SpriteRenderer modalPartPrefab;
	public SpriteRenderer facePrefab;
	public Option optionPrefab;
	public AudioClip applause;
    public GameObject sound_manager;
	void Start()
	{
		//StartLevel();
    }

	public void StartLevel()
	{

        database = FindObjectOfType<Database>();
        
        //Initial amount of time for which face is shown
        faceTime = Database.constants_faceShownForSeconds;
        float faceTimeInit = faceTime;

        //Creating object for sqlite database access 
        dataController = new FaceGame_DataService(Database.tagsAndNames_sqliteDB);
        var persistant_go = GameObject.Find(Shared_Scenevariables.masterGO);
        var user_obj = persistant_go.GetComponent<Shared_PersistentScript>().GetCurrentPlayer();
		user = user_obj.Username;
        //var lev = dataController.GetLevelData(user);
        //Retrieving data(of type -FaceGame_GameData) from database for 'user'
        foreach (var lev in dataController.GetLevelData(user))
        {
            levelData = lev;
        }
        level = levelData.FaceLevel - 1;

        if (levelData.LevelNumber == 0)
        { preLevel = true; }
        else
        { preLevel = false; }

        var prog = dataController.GetProgress(user);
        //foreach (var prog in dataController.GetProgress(user))
        //{
            progressInSubLevel = prog.Progress;
        //}

        deltaProgress = 100 / (levelData.NumOfCompletions) + 1;
		//3 random face parts selected from pile for options (first of which is assigned right choice) 

		StartCoroutine(Levels(faceTimeInit));
	}

	IEnumerator Levels(float faceTimeInit)
	{
		List<List<int>> randomOptions = new List<List<int>>();
		GenerateRandomOptionsForFaceComponent(randomOptions);

		ProgressBarBehaviour progressBar = GameObject.Find(Database.tagsAndNames_progressBar).GetComponent<ProgressBarBehaviour>();
		SetProgressBar(progressBar);

		GameObject.Find(Database.tagsAndNames_tempBackground).GetComponent<SpriteRenderer>().enabled = false;

		//Repeat face NumOfRepeat times for every NumOfComp
		for (mainIndex = progressInSubLevel; mainIndex < levelData.NumOfCompletions; mainIndex++)
		{
			if (mainIndex <= 2 && preLevel)
			{
				tutorial = true;
			}
			else
			{
				tutorial = false;
			}

			if (preLevel && (repeat != 0))
			{
				GenerateRandomOptionsForFaceComponent2(randomOptions[mainIndex]);
			}

			//Enter level on good score, else decrease level
			dataController.SetProgress(user, mainIndex);
			if (score > -3 || preLevel)
				yield return StartCoroutine(GameStart(randomOptions[mainIndex], progressBar, faceTimeInit));
			else
			{
				dataController.SetProgress(user, 0);
				dataController.SetLevel(levelData.LevelNumber - 1, user);
				Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
			}
		}

		//Level Successfully Completed
		StartCoroutine(LevelComplete());
	}

	IEnumerator GameStart(List<int> randomOptions, ProgressBarBehaviour progressBar, float faceTimeInit)
	{
		float smoothTime = Database.constants_smoothTime;
		GameObject item = Instantiate(itemPrefab);
		GameObject faceComponents = Instantiate(faceComponentsPrefab, item.transform);
		Transform[] faceComponent = new Transform[Database.constants_noOfFaceComponents];
		SpriteRenderer modal = (SpriteRenderer)Instantiate(modalPrefab, item.transform);
		SpriteRenderer face = (SpriteRenderer)Instantiate(facePrefab, item.transform);
		SpriteRenderer modalPart = new SpriteRenderer();
		SpriteRenderer facePart = new SpriteRenderer();

		Canvas canvas = FindObjectOfType<Canvas>();

		modal.sprite = Resources.Load<Sprite>("FaceGame/Level_" + Convert.ToString(level + 1) + "/Facebases/Facebase_" + Convert.ToString(randomOptions[0])); //First index of the 'NO_OF_OPTIONS' option-indexes generated is assigned as facebase
		face.sprite = Resources.Load<Sprite>("FaceGame/Level_" + Convert.ToString(level + 1) + "/Facebases/Facebase_" + Convert.ToString(randomOptions[0])); //First index of the 'NO_OF_OPTIONS' option-indexes generated is assigned as a facebase

		//Instantiating each modal, faceComponent and its options
		for (int i = 0; i < levelData.NumOfComponents; i++)
		{
			//Creating a modal by adding a random modalPart
			ModalAssign(i, randomOptions, modal, modalPart);

			//Creating Options by adding NO_OF_OPTIONS options for each faceComponent
			OptionsAssign(i, randomOptions, faceComponents, faceComponent);

		}

		//if level succeeded : yes - if database.ifRight is true at end of game
		database.ifRight = true;

		yield return StartCoroutine(ShowModalPhoto(modal));

		if (!(preLevel && mainIndex <= 2))
		{
			yield return StartCoroutine(Show(face, faceTime));
		}
		else if (preLevel && mainIndex == 2)
		{
			yield return StartCoroutine(Show(face, 0f));
		}

		yield return new WaitForSeconds(1.5f);
        
		//Code dor PRE LEVEL
		if (tutorial)
		{
			faceComponent[0].GetChild(0).transform.position = new Vector3(Database.constants_faceBaseOffset2, 0f);
		}
		else
		{
			Cover(modal, true); // cover modal, then uncover
		}
              
		//Showing 'database.NO_OF_OPTIONS' no. of options for each faceComponent
		for (int i = 0; i < levelData.NumOfComponents; i++)
		{
			yield return StartCoroutine(faceComponent[i].GetComponent<Component>().SelectOption(preLevel, tutorial, mainIndex));
		}

		if (!tutorial)
		{
			Cover(modal, false);
		}

		//All faceComponents matched correctly
		if (database.ifRight)
		{
			yield return StartCoroutine(RightMatch(progressBar, smoothTime, faceTimeInit));
		}

		//One or more than one incorrect faceComponent/s selected
		else
		{
			yield return StartCoroutine(WrongMatch());
		}

		if (!(preLevel && mainIndex <= 1))
		{
			yield return StartCoroutine(FaceModalOverlap(modal, face));
		}
		else if (database.ifRight)
		{
			ParticleSystem particleEffect = FindObjectOfType<ParticleSystem>();
			particleEffect.Play();
            yield return new WaitForSeconds(sound_manager.GetComponent<SoundManager_Script>().PlayHappySound());
			//yield return new WaitForSeconds(2f);
		}
		//else
		//{
  //          //yield return new WaitForSeconds(sound_manager.GetComponent<SoundManager_Script>().PlaySadSound());
  //          //yield return new WaitForSeconds(2f);
		//}

        //repeat level if wrongly matched
		if (!database.ifRight)
			mainIndex--;
		Destroy(item);

	}

	void Cover(SpriteRenderer modal, bool val)
	{
		GameObject cover = GameObject.Find(Database.tagsAndNames_cover);
		if (val)
		{
			cover.transform.position = modal.transform.position;
		}
		else
		{
			cover.GetComponent<SmoothTransition>().SetTarget(new Vector3(modal.transform.position.x, 10f), cover.transform.localScale);
		}
	}

    IEnumerator DropComponents(SpriteRenderer modal)
    {
        for (int i = 0; i < modal.transform.childCount; i++)
        {
            modal.transform.GetChild(i).gameObject.AddComponent<Rigidbody2D>();
        }
        yield return null;
    }

    void ModalAssign(int i, List<int> randomOptions,SpriteRenderer modal, SpriteRenderer modalPart)
    {
        modalPart = (SpriteRenderer)Instantiate(modalPartPrefab, modal.gameObject.transform); //Each modalComponent is being added to its parent modal
		modalPart.sprite = Resources.Load<Sprite>("FaceGame/Level_" + Convert.ToString(level + 1) + "/Parts/Part_" + Convert.ToString(i + 1) + Convert.ToString(randomOptions[0])); //First index of the 'NO_OF_OPTIONS' option-indexes generated is assigned as a modalComponent
		modalPart.transform.position = Vector3.zero;
    }

    void OptionsAssign(int i, List<int> randomOptions, GameObject faceComponents, Transform[] faceComponent)
    {
        faceComponent[i] = (Transform)Instantiate(faceComponentPrefab, faceComponents.transform); //Each faceComponent to consist of 'database.NO_OF_OPTIONS' options
        
		//Randomizing optionPositions under this particular faceComponent
		RandomizeOptionsWithoutRepeat(Database.constants_optionPosLevel);

		//Code dor PRE LEVEL
		Option[] option;
		if (preLevel && mainIndex == 0)
		{
			option = new Option[1]; //options to be added under this particular faceComponent
			option[0] = (Option)Instantiate(optionPrefab, faceComponent[i].transform);
			option[0].gameObject.transform.position = Database.constants_optionPosLevel[0]; //optionPosition
			option[0].transform.position -= new Vector3(0f, Database.constants_optionBGShift[level][i]);
            option[0].pos = new Vector3(Database.constants_faceBaseOffset2, 0f); //Position to transfer to on clicking
			option[0].gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("FaceGame/Level_" + Convert.ToString(level + 1) + "/Parts/Part_" + Convert.ToString(i + 1) + Convert.ToString(randomOptions[0])); //Sprite Position allocation
			option[0].correctKey = true; //Since first option-index is assigned as the modalComponent    
			option[0].transform.GetChild(0).position += new Vector3(0f,Database.constants_optionBGShift[level][i]);
			option[0].GetComponent<WobbleEffect>().StartWobble();
            //tag = Database.tagsAndNames_optionTag;
		}
		else
		{
			option = new Option[Database.constants_NO_OF_OPTIONS]; //options to be added under this particular faceComponent      
			for (int k = 0; k < Database.constants_NO_OF_OPTIONS; k++)
			{
				option[k] = (Option)Instantiate(optionPrefab, faceComponent[i].transform);
				option[k].gameObject.transform.position = Database.constants_optionPosLevel[k]; //optionPosition
			    option[k].transform.position -= new Vector3(0f, Database.constants_optionBGShift[level][i]);
                option[k].gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("FaceGame/Level_" + Convert.ToString(level + 1) + "/Parts/Part_" + Convert.ToString(i + 1) + Convert.ToString(randomOptions[k])); //Sprite Position allocation
				option[k].transform.GetChild(0).position += new Vector3(0f, Database.constants_optionBGShift[level][i]);
				option[k].GetComponent<WobbleEffect>().StartWobble();
                
				if (preLevel && mainIndex == 1)
                {
                    option[k].pos = new Vector3(Database.constants_faceBaseOffset2, 0f); //Position to transfer to on clicking
                }
                else
                {
                    option[k].pos = new Vector3(Database.constants_faceBaseOffset, 0f); //Position to transfer to on clicking
                }

				//the correct option is the first option of the list
				if (k == 0)
				{
					option[k].correctKey = true; //Since first option-index is assigned as the modalComponent                    
				}
				else
				{
					option[k].correctKey = false;
				}

			}
		}
    }
   
    //shows modal for "faceTime" seconds
    IEnumerator Show(SpriteRenderer modal, float time)
    {
        modal.gameObject.SetActive(true);
        modal.transform.position = new Vector3(Camera.main.rect.width * 10f, 0f);
        modal.gameObject.AddComponent<SmoothTransition>();
		modal.GetComponent<SmoothTransition>().SetTarget(new Vector3(Database.constants_faceBaseOffset, 0f), new Vector3(Database.constants_faceComponentScale, Database.constants_faceComponentScale));
		yield return new WaitForSeconds(time); //shows modal for "faceTime" seconds and then returns

    }

    //Hides modal when called
    void Hide(SpriteRenderer modal)
    {
		modal.GetComponent<SmoothTransition>().SetTarget(new Vector3(modal.transform.position.x - 10f, modal.transform.position.y), modal.transform.localScale);
	}

    IEnumerator ShowModalPhoto(SpriteRenderer modal)
    {
        modal.gameObject.SetActive(true);
		modal.transform.position = new Vector3(Camera.main.rect.width * 10f, 0f);
        modal.gameObject.AddComponent<SmoothTransition>();
		modal.GetComponent<SmoothTransition>().SetTarget(new Vector3(Database.constants_faceBaseOffset2, 0f), new Vector3(Database.constants_faceComponentScale, Database.constants_faceComponentScale));
        yield return null;
    }

	void SetProgressBar(ProgressBarBehaviour progressBar)
	{
		float prog = deltaProgress * progressInSubLevel;
		progressBar.SetFillerSizeAsPercentage(prog);
	}

	void RandomizeOptionsWithoutRepeat(List<Vector3> rand)
	{
		List<Vector3> rnd = new List<Vector3>();
		for (int i = 0; i < rand.Count; i++)
		{
			rnd.Add(rand[i]);
		}

		int count = 0;
		while ((rnd[0] == rand[0])&&count<10)
		{
			FindObjectOfType<RandomizeArray>().Randomize<Vector3>(Database.constants_optionPosLevel);
			count++;

		}
	    
	}

    //if all faceComponents have been matched properly
    IEnumerator RightMatch(ProgressBarBehaviour progressBar, float smoothTime, float faceTimeInit)
    {
		ParticleSystem particleEffect = FindObjectOfType<ParticleSystem>();
        float currentValue = progressBar.Value;
        float faceTimeDecr = Database.constants_faceTimeDecr;
        float finalValue = currentValue + deltaProgress;

        //Increase progress value in progress bar
        progressBar.SetFillerSizeAsPercentage(finalValue);
        yield return new WaitForSeconds(3*smoothTime);

        //score = score + (int)deltaProgress;
        //on right match, decreasing time for which face shown - by 0.75 seconds
		if(faceTime - faceTimeDecr >= faceTimeInit/2)
		faceTime = faceTime - faceTimeDecr;

		//Repeat counts no of times the same face is repeated
		repeat = 0;

    }

	//if one or more faceComponents have been matched incorrectly
	IEnumerator WrongMatch()
	{

        GameObject[] wrongCh = GameObject.FindGameObjectsWithTag(Database.tagsAndNames_wrongChoice);
        int count = wrongCh.Length;

		//loop count decreased due to repetition
		//mainIndex--;

		//Increasing repeats for a particular face matched wrongly
		repeat++;

		//score decreased for wrong match
		score--;
        yield return new WaitForSeconds(0.1f);

        //blink all wrong face components
        for (int i = 0; i < count; i++)
        {
			wrongCh[i].transform.gameObject.GetComponent<Blink>().StartBlink();
        }
        

    }

	IEnumerator FaceModalOverlap(SpriteRenderer modal, SpriteRenderer face)
	{
		GameObject[] rightCh = GameObject.FindGameObjectsWithTag(Database.tagsAndNames_rightChoice);
		GameObject[] wrongCh = GameObject.FindGameObjectsWithTag(Database.tagsAndNames_wrongChoice);
		modal.transform.GetChild(0).gameObject.AddComponent<FadeAway>();
		modal.GetComponentInChildren<FadeAway>().StartFadeOut();

		face.GetComponent<SmoothTransition>().SetTarget(new Vector3(Database.constants_faceBaseOffset2, face.transform.position.y), face.transform.localScale);
        foreach (GameObject obj in rightCh)
        {
            obj.GetComponent<SmoothTransition>().SetTarget(new Vector3(Database.constants_faceBaseOffset2, obj.transform.position.y), obj.transform.localScale);
        }
        foreach (GameObject obj in wrongCh)
        {
            obj.GetComponent<SmoothTransition>().SetTarget(new Vector3(Database.constants_faceBaseOffset2, obj.transform.position.y), obj.transform.localScale);
        }

        //Comparison for wrong selection
		if (!database.ifRight)
		{
			yield return new WaitForSeconds(2f);
		
			face.GetComponent<SmoothTransition>().SetTarget(new Vector3(Database.constants_faceBaseOffset, face.transform.position.y), face.transform.localScale);
			foreach (GameObject obj in rightCh)
			{
				obj.GetComponent<SmoothTransition>().SetTarget(new Vector3(Database.constants_faceBaseOffset, obj.transform.position.y), obj.transform.localScale);
			}
			foreach (GameObject obj in wrongCh)
			{
				obj.GetComponent<SmoothTransition>().SetTarget(new Vector3(Database.constants_faceBaseOffset, obj.transform.position.y), obj.transform.localScale);
			}

			yield return new WaitForSeconds(2f);

            //yield return new WaitForSeconds(sound_manager.GetComponent<SoundManager_Script>().PlaySadSound());
			//yield return new WaitForSeconds(2f);
		}
		else
		{
			yield return new WaitForSeconds(1.5f);
			ParticleSystem particleEffect = FindObjectOfType<ParticleSystem>();
			particleEffect.Play();
            yield return new WaitForSeconds(sound_manager.GetComponent<SoundManager_Script>().PlayHappySound());

			//yield return new WaitForSeconds(2f);
		}

        Hide(face);
        foreach (GameObject obj in rightCh)
        {
            SpriteRenderer spr = obj.GetComponent<SpriteRenderer>();
            Hide(spr);
        }
        foreach (GameObject obj in wrongCh)
        {
            SpriteRenderer spr = obj.GetComponent<SpriteRenderer>();
            Hide(spr);
        }
        Hide(modal);

        yield return new WaitForSeconds(3f);
	}
    void GenerateRandomOptionsForFaceComponent(List<List<int>> randomOptions)
    {
        for (int i = 1; i <= levelData.NumOfCompletions; i++)
        {
            List<int> randIndicesMain = new List<int>();
            List<int> randIndices = new List<int>();

            randIndicesMain.Add(i);

            for (int k = 1; k <= levelData.NumOfCompletions; k++)
            {
                randIndices.Add(k);
            }
            randIndices.Remove(i);

            //Randomizing the index list
            FindObjectOfType<RandomizeArray>().Randomize<int>(randIndices);

            //adding first two of random indices - along with the correct option index
            randIndicesMain.Add(randIndices[0]);
            randIndicesMain.Add(randIndices[1]);

            randomOptions.Add(randIndicesMain);
        }

        //adding same option indexes for mouth as that for eyes
        //randomOptionsAll.Add (randomOptionsAll [database.noOfFaceComponents - 1]);
    }

	void GenerateRandomOptionsForFaceComponent2(List<int> rnd)
	{
		rnd.Clear();
		for (int k = 1; k <= levelData.NumOfCompletions; k++)
        {
			rnd.Add(k);
        }
		FindObjectOfType<RandomizeArray>().Randomize<int>(rnd);

	}

    IEnumerator LevelComplete()
    {
		GetComponent<AudioSource>().PlayOneShot(applause);
        yield return new WaitForSeconds(applause.length);     
		Camera.main.GetComponent<MakeItRainObjects>().enabled = true;
		yield return new WaitForSeconds(totalWaitingTime);      
        
		int maxLevel = 0;
		foreach (var lev in dataController.GetMaxLevel())
		{
			maxLevel = lev.LevelNumber;
		}

		dataController.SetProgress(user, 0);
        //Level Increment on current level completion
		if (levelData.LevelNumber < maxLevel)
		{
			dataController.SetLevel(levelData.LevelNumber + 1, user);
			foreach (var mapPanel in Resources.FindObjectsOfTypeAll<MapPanel>())
			{
				mapPanel.gameObject.SetActive(true);
			}
			FindObjectOfType<LevelManager>().StartUpdateMap();
		}
		else
		{
			SceneManager.LoadScene(Database.scenes_gameStart);
		}
		yield return null;
    }

}
