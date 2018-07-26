using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;  

public class LevelManager : MonoBehaviour
{
	public GameObject FaceComponentUIPrefab;
    
    //used in public function
	private int highestLevel = 0;
	private List<GameObject> sortedPortalList;
	private GameObject unlocked;
	private Database database = new Database();
	private FaceGame_DataService dataController;
	private string user = "";
    public Text error_text;

    void Start()
	{
       
       Database database = FindObjectOfType<Database>();
       dataController = new FaceGame_DataService(database.tagsAndNames_sqliteDB);
       
        var persistant_go = GameObject.Find(Database.masterGo);
        var user_obj = persistant_go.GetComponent<Shared_PersistentScript>().GetCurrentPlayer();
        user = user_obj.Username;


        GameObject[] portals = GameObject.FindGameObjectsWithTag(database.tagsAndNames_portals);

		sortedPortalList = portals.OrderBy(go => int.Parse(go.name.Substring(6))).ToList();

		//Set sprites for locked-level buttons
		for (int i = 2; i < sortedPortalList.Count; i++)
		{
			string lev = sortedPortalList[i].transform.parent.name;
			int count = 0;
			if ((i-1) % 3 != 0)
				count = (i-1) % 3;
			else
				count = 3;
			
			while (count >= 1)
			{
				GameObject FaceComp = Instantiate(FaceComponentUIPrefab, sortedPortalList[i].transform);
				FaceComp.GetComponent<Image>().sprite = Resources.Load<Sprite>("FaceGame/Level_" + lev + "/Parts/Part_" + count + "1");
				FaceComp.GetComponent<RectTransform>().sizeDelta = FaceComp.transform.parent.GetComponent<RectTransform>().sizeDelta;
				count--;
			}

		}
        
		StartCoroutine(UpdateMap());

    }

	IEnumerator UpdateMap()
	{
        try
        {
            foreach (var lev in dataController.GetLevelData(user))
            {
                highestLevel = lev.LevelNumber;
            }
        } catch (Exception e)
        {
            error_text.text = e.ToString();
        }
		unlocked = GameObject.Find("Level " + Convert.ToString(highestLevel));
        
		float delY = 0.25f * Convert.ToInt16(unlocked.transform.parent.name);
		FindObjectOfType<Scrollbar>().value = 1f - delY;
        
		foreach (var lev in sortedPortalList)
        {
            int levNum = int.Parse(lev.name.Substring(6));
            if (levNum == highestLevel - 1)
            {
				sortedPortalList[levNum + 1].GetComponent<Button>().onClick.RemoveAllListeners();
			}
			if (levNum != highestLevel)
			{
				sortedPortalList[levNum + 1].GetComponent<Button>().interactable = false;
			}
			else
			{
				sortedPortalList[levNum + 1].GetComponent<Button>().interactable = true;
			}
		}
        

		unlocked.GetComponent<WobbleEffect>().StartWobble();

		//Set sprites and level link for unlocked-level buttons
		unlocked.GetComponent<Button>().onClick.AddListener(() => GameObject.Find(database.tagsAndNames_mapPanel).SetActive(false));
		if (int.Parse(unlocked.name.Substring(6)) >= 0)
		{
			unlocked.GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<FaceGame_GameManager>().StartLevel());
		}
		else
		{
			unlocked.GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<BasicGameManager>().StartLevel());
		}

		yield return null;
	}

	public void StartUpdateMap()
	{
		StartCoroutine(UpdateMap());
	}

}