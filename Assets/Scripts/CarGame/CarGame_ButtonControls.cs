using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CarGame_ButtonControls : MonoBehaviour {

	bool isPaused = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		PauseOrExit ();
	}
		
	void PauseOrExit(){
		if (Input.GetKey(KeyCode.Escape)) {
			if (!isPaused) { // if game is not yet paused, ESC will pause it
				isPaused = true;
				PauseGame ();					// definition has to be added to pause the game. 
			} else { // if game is paused and ESC is pressed, it's the second press. QUIT
				ExitGame();
			}
		}

	}

	void PauseGame (){
	
	}

	void ExitGame(){
		SceneManager.LoadScene ("HomeScreen");
	}


}
