using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DatabaseRowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUp(string tex)
    {
        GetComponentInChildren<Text>().text = tex;
    }

    public void AddTrainGameLevels()
    {
        List<TrainGame_Levels> lt = new List<TrainGame_Levels>();
        lt.Add(new TrainGame_Levels() { NumOfBogie = 6, ShouldBlock = 1, LevelNumber = 12, WaitTime = 1.0f });
        lt.Add(new TrainGame_Levels() { NumOfBogie = 4, ShouldBlock = 1, LevelNumber = 13, WaitTime = 0.7f });
        lt.Add(new TrainGame_Levels() { NumOfBogie = 5, ShouldBlock = 1, LevelNumber = 14, WaitTime = 0.7f });
        lt.Add(new TrainGame_Levels() { NumOfBogie = 6, ShouldBlock = 1, LevelNumber = 15, WaitTime = 0.7f });
        lt.Add(new TrainGame_Levels() { NumOfBogie = 4, ShouldBlock = 1, LevelNumber = 16, WaitTime = 0.4f });
        lt.Add(new TrainGame_Levels() { NumOfBogie = 5, ShouldBlock = 1, LevelNumber = 17, WaitTime = 0.4f });
        lt.Add(new TrainGame_Levels() { NumOfBogie = 6, ShouldBlock = 1, LevelNumber = 18, WaitTime = 0.4f });
        var ds = new TrainGame_DataServices(MApp_UserInforFormScript.database_Name);
        ds.AddLevels(lt);
        ds.UpdateUserProgress("pip", 12);
        GetComponent<Button>().interactable = false;
    }
}






