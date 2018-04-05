using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CarGame_ButtonControls : MonoBehaviour {

	bool isPaused = false;
	int back_count = 0, max_count = 3;
	// Use this for initialization
	void Start () {
		StartCoroutine (ResetBackCount ());
	}
	
	// Update is called once per frame
	void Update () {
		PauseOrExit ();
	}
		
	void PauseOrExit(){
		if (Input.GetKey(KeyCode.Escape)) {
			if (back_count < max_count) { // if game is not yet paused, ESC will pause it
//				isPaused = true;
				back_count ++;
				PauseGame ();					// definition has to be added to pause the game. 
			} else { // if game is paused and ESC is pressed, it's the second press. QUIT
				back_count = 0;
				ExitGame();
			}
		}

	}

	void PauseGame (){
	
	}

	void ExitGame(){
		SceneManager.LoadScene (CarGame_SceneVariables.Game_Name +"_HomeScreen");
	}

	IEnumerator ResetBackCount(){
		back_count = 0;
		yield return new WaitForSeconds(3f);
		StartCoroutine (ResetBackCount ());
	}

}
