using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ExitButtonScript : MonoBehaviour {

    float pressed_for = 0f;
    public Canvas levelCanvas;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitProtocol()
    {
        pressed_for += Time.deltaTime;
        if(pressed_for > 3f && !levelCanvas.gameObject.activeSelf )
        {
            ShowLevelScreen();
        }

    }

    void ShowLevelScreen()
    {
        
    }

    private void Reset()
    {
        pressed_for = 0f;
        levelCanvas.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
