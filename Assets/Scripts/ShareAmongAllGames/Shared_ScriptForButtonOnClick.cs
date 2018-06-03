using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Shared_ScriptForButtonOnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Current player is " + MApp_AcrossSceneStaticVariables.GetCurrentPlayer().Username);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadSceneByName(string sceneName){
		SceneManager.LoadScene (sceneName);
	}
}
