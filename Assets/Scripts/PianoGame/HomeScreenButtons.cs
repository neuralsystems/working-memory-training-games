using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HomeScreenButtons : MonoBehaviour {

	public string sceneToLoad;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		DetectTouch ();
	}


	// when the object is Touched
	void DetectTouch(){

		if (Input.touchCount == 1)
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPos = new Vector2(wp.x, wp.y);
			if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
			{
				OnMouseDown ();
			}
		}
	}

	void OnMouseDown(){
		Debug.Log ("Called for" + this.gameObject.name);
		if (this.gameObject.name == "Close") {
			Application.Quit ();
		} else if (this.gameObject.name == "Play") {
			SceneManager.LoadScene ("Scene2");
		} else if (this.gameObject.name == "Home") {
			
		}else if (this.gameObject.name == Camera.main.GetComponent<SceneVariables>().playSound) {
//			PlayTone.sample = "";
			if (GetComponent<SpriteRenderer> ().enabled) {
				OnKeyPress.userString = "";
				Debug.Log ("Clicked me?");
//			Camera.main.GetComponent<SceneVariables> ().ShowNeutral();
				Camera.main.GetComponent<PlayTone> ().PlayToneFromExternal ();
				GetComponent<SpriteRenderer> ().enabled = false;
			}
		}else {
			SceneManager.LoadScene (sceneToLoad);
		}

	}
}
