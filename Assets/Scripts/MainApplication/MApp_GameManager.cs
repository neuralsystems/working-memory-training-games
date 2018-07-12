using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MApp_GameManager : MonoBehaviour {

    public string HomeScreen;
    public GameObject MasterGo;
    public const string masterGO = "MasterGameObject";
    // Use this for initialization
    void Start () {
		
	}
	
	public void LoadHomeScreen()
    {
        SceneManager.LoadScene(HomeScreen);
    }

    public void SetUser(User user_obj)
    {
        //Debug.Log("value passed: " + user_obj.Username);
        MApp_DataServices _ds = new MApp_DataServices(MApp_UserInforFormScript.database_Name);
        var _users = _ds.GetUser(user_obj.Username);
        foreach(var _user in _users)
        {
            MasterGo.GetComponent<Shared_PersistentScript>().SetCurrentPlayer(_user);
        }
        LoadHomeScreen();
    }
}
