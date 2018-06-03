using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MApp_GameManager : MonoBehaviour {

    public string HomeScreen;
    // Use this for initialization
	void Start () {
		
	}
	
	public void LoadHomeScreen()
    {
        SceneManager.LoadScene(HomeScreen);
    }

    public void SetUser(string username)
    {
        MApp_DataServices _ds = new MApp_DataServices(MApp_UserInforFormScript.database_Name);
        var _users = _ds.GetUser(username);
        foreach(var _user in _users)
        {
            MApp_AcrossSceneStaticVariables.SetCurrentPlayer(_user);
        }

    }
}
