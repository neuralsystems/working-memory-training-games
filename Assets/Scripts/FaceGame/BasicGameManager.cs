using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicGameManager : MonoBehaviour {

    private Database database;
    private FaceGame_DataService dataController;
    private FaceGame_GameData levelData;
	public string user;
    private int totalLevels = 2;
    private int iterations = 3;

	public Option faceComponentPrefab;
	public Option optionsPrefab;
	public AudioClip applause;
    //Start() >> BasicLevel() >> Iteration()

    private void Start()
    {
        GetDetails();
    }
    public void StartLevel()
    {
		StartCoroutine(StartLevel2());
    }
    public void GetDetails()
    {
        database = FindObjectOfType<Database>();
        dataController = new FaceGame_DataService(database.tagsAndNames_sqliteDB);

        var persistant_go = GameObject.Find(Database.masterGo);
        var user_obj = persistant_go.GetComponent<Shared_PersistentScript>().GetCurrentPlayer();
        user = user_obj.Username;

        //Retrieving data (of type - FaceGame_GameData) from database for 'user'
        foreach (var lev in dataController.GetLevelData(user))
        {
            levelData = lev;
        }
        totalLevels = levelData.NumOfComponents;
        iterations = levelData.NumOfCompletions;
        if (levelData.LevelNumber > -1)
        {
            SceneManager.LoadScene(database.scenes_level);
        }
    }
	public IEnumerator StartLevel2()
	{

        
        //dataController = new FaceGame_DataService(database.tagsAndNames_sqliteDB);
        for (int i = 0; i < totalLevels; i++)
		{
			yield return StartCoroutine(BasicLevel(i));
		}

		StartCoroutine(BasicLevelComplete());
	}

    IEnumerator BasicLevel(int index)
    {
        
        //Sub Levels of a level - no.of options increases
		for (int i = 0; i < iterations; i++)
        {
			yield return StartCoroutine(Iteration(index,i));
        }

    }

	IEnumerator Iteration(int index, int subIndex)
    {

        //All gameobjects per level are children to item
        GameObject item = new GameObject();
		Option faceComponent = new Option();
		ParticleSystem particleEffect = FindObjectOfType<ParticleSystem>();

		GameObject.Find(database.tagsAndNames_tempBackground).GetComponent<SpriteRenderer>().enabled = false;

		int noOfOptions = 1;
        if (index == 0)
        {
            noOfOptions = Math.Min(database.constants_NO_OF_OPTIONS, subIndex + 1);
        }
        else
        {
            noOfOptions = database.constants_NO_OF_OPTIONS;
        }
		Option[] options = new Option[noOfOptions];
        
        //Instantiating and enabling sprites and colliders for the face component and options
		SetFaceComponentnOptions(index,subIndex, noOfOptions, item, faceComponent, options);
		do
		{
			database.ifRight = true;

			//Until any option is clicked(touched)
			yield return new WaitUntil(() => database.ifOptionSelected);
			yield return new WaitForSeconds(1f);

			//if option chosen is correct
			if (database.ifRight)
			{
				FindObjectOfType<SoundManager>().PlayHappySound();
				particleEffect.Play();
			}
			//if option chosen is wrong
			else
			{
				GameObject[] objs = GameObject.FindGameObjectsWithTag(database.tagsAndNames_wrongChoice);
				Option obj = new Option();
				foreach (var temp in objs)
				{
					if (temp.GetComponent<Option>().selectedKey)
					{
						obj = temp.GetComponent<Option>();
					}
				}

				obj.GetComponent<Blink>().StartBlink();
				obj.GetComponent<WobbleEffect>().StopWobble();

				FindObjectOfType<SoundManager>().PlaySadSound();
				yield return new WaitForSeconds(1f);

				obj.GetComponent<Blink>().StopBlink();
				obj.GetComponent<WobbleEffect>().StartWobble();

				obj.GetComponentInChildren<FadeAway>().StartFadeIn();

				obj.GetComponent<SmoothTransition>().SetTarget(obj.optionPos, new Vector3(database.constants_faceComponentScale, database.constants_faceComponentScale));

				obj.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

				database.ifOptionSelected = false;
				obj.selectedKey = false;

			}
		} while (!database.ifRight);
		yield return new WaitForSeconds(2f);

		database.ifOptionSelected = false;

        //Dynamic deletion of all gameobjects after every iteration
		yield return StartCoroutine(Hide(item));
        yield return null;
    }

	void SetFaceComponentnOptions(int index, int subIndex, int noOfOptions, GameObject item, Option faceComponent,Option[] options)
    {
        //Face Component (eg. hair) to match
		faceComponent = (Option)Instantiate(faceComponentPrefab, item.transform);

		faceComponent.transform.position = new Vector3(database.constants_faceBaseOffset3, -database.constants_optionBGShift[index]);

		faceComponent.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("FaceGame/Level_1/Parts/Part_" + Convert.ToString(index + 1) + Convert.ToString(subIndex+1));
		faceComponent.GetComponent<SpriteRenderer>().enabled = true;

		RandomizeOptionsWithoutRepeat(database.constants_optionPosBasic);

		//getting 3 random options, first one being correct
		List<int> rand = GetRandOptions(subIndex);
      
		//Options
		for (int i = 0; i < noOfOptions; i++)
        {
			options[i] = (Option)Instantiate(optionsPrefab, item.transform);

			options[i].transform.position = database.constants_optionPosBasic[i];
			options[i].transform.position -= new Vector3(0f, database.constants_optionBGShift[index]);
			options[i].transform.GetChild(0).transform.position += new Vector3(0f, database.constants_optionBGShift[index]);
			options[i].optionPos = options[i].transform.position;

			options[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("FaceGame/Level_1/Parts/Part_" + Convert.ToString(index + 1) + Convert.ToString(rand[i]));
			options[i].GetComponent<SpriteRenderer>().enabled = true;
			options[i].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

			float targetScale = options[i].GetComponent<SpriteRenderer>().bounds.size.x * database.constants_sizeToScaleConversionFactor;
			options[i].transform.GetChild(0).localScale = new Vector3(targetScale, targetScale, 0f);
			options[i].GetComponent<CircleCollider2D>().radius = targetScale * database.constants_scaleToRadiusConversionFactor;
			options[i].GetComponent<CircleCollider2D>().enabled = true;

			options[i].GetComponent<WobbleEffect>().StartWobble();

			if (i != 0) //final position of option for incorrect match
			{
				options[i].pos = new Vector3(database.constants_faceBaseOffset3 + 0.4f, -database.constants_optionBGShift[index]);
			}
			else //final pos for correct match
			{
				options[i].pos = new Vector3(database.constants_faceBaseOffset3, -database.constants_optionBGShift[index]);
			}
        }
        
		options[0].correctKey = true;
        
    }

	IEnumerator Hide(GameObject item)
	{
		item.AddComponent<SmoothTransition>();
		item.GetComponent<SmoothTransition>().SetTarget(new Vector3(-20f, 0f), new Vector3(database.constants_faceComponentScale, database.constants_faceComponentScale), 0.2f, 2f);
		yield return new WaitForSeconds(0.5f);

		Destroy(item);
		yield return null;
	}
    
    //Correct option isnt at the same position consectutively
    void RandomizeOptionsWithoutRepeat(List<Vector3> rand)
    {
        List<Vector3> rnd = new List<Vector3>();
        for (int i = 0; i < rand.Count; i++)
        {
            rnd.Add(rand[i]);
        }

        int count = 0;
        while ((rnd[0] == rand[0]) && count < 10)
        {
			FindObjectOfType<RandomizeArray>().Randomize<Vector3>(database.constants_optionPosBasic);
            count++;

        }

    }

	IEnumerator BasicLevelComplete()
	{
		FindObjectOfType<SoundManager>().PlaySound(applause);

        Camera.main.GetComponent<MakeItRainObjects>().enabled = true;
        yield return new WaitForSeconds(6f);    

		dataController.SetLevel(levelData.LevelNumber + 1, user);
		dataController.SetProgress(user, 0);
        
		SceneManager.LoadScene(database.scenes_level);
	}

	List<int> GetRandOptions(int subIndex)
	{
		List<int> rand = new List<int>();
		List<int> rand1 = new List<int>();
		for (int i = 1; i <= database.constants_NO_OF_COMPONENTS_LV1; i++)
        {
            if (i != (subIndex + 1))
            {
                rand.Add(i);
            }
        }
		FindObjectOfType<RandomizeArray>().Randomize(rand);
		int count = rand.Count; 
		for (int i = count; i > 2; i--)
        {
			rand.RemoveAt(i - 1);
        }
		rand1.Add(subIndex + 1);
		foreach (var num in rand)
        {
			rand1.Add(num);
        }
		//Debug.Log(rand1.Count);
		return rand1;
	}
    
    // Update is called once per frame
    void Update () {
        
    }
}
