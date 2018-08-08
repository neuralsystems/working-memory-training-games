using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Shared_ScriptForButtonOnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Current player is " + GameObject.Find("MasterGameObject").GetComponent<Shared_PersistentScript>().GetCurrentPlayer().Username);
    }

    // Update is called once per frame
    void Update () {
		
	}

	public void LoadSceneByName(string sceneName){
		SceneManager.LoadScene (sceneName);
	}

    public void BasketGame()
    {
        var m_obj = GameObject.Find(Shared_Scenevariables.masterGO);
        var current_user = m_obj.GetComponent<Shared_PersistentScript>().GetCurrentPlayer();
        var ds = new BasketGame_DataService(BasketGame_SceneVariables.DATABASE_NAME);
        var user_progress = ds.GetUserProgress(current_user.Username);
        var MainGameSceneName = "BasketGame_Scene2";
        var PreGameSceneName = "BasketGame_PreScene1";
        if (user_progress.hasCompletedPreLevel())
        {
            SceneManager.LoadScene(MainGameSceneName);
        }
        else
        {
            SceneManager.LoadScene(PreGameSceneName);
        }
    }

   public void TrainGame()
    {
        var m_obj = GameObject.Find(Shared_Scenevariables.masterGO);
        var current_user = m_obj.GetComponent<Shared_PersistentScript>().GetCurrentPlayer();
        var ds = new TrainGame_DataServices(TrainGame_SceneVariables.DATABASE_NAME);
        var user_progress = ds.GetUserProgress(current_user.Username);
        var MainGameSceneName = "TrainGame_Scene1";
        var PreGameSceneName = "TrainGame_PreScene1";
        if (user_progress.HasCompletedPreLevel())
        {
            SceneManager.LoadScene(MainGameSceneName);
        }
        else
        {
            SceneManager.LoadScene(PreGameSceneName);
        }
    }
}
