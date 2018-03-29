using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CarGame_HomeScreenButtons : MonoBehaviour {

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
				//				originSprite = spriteRenderer.sprite;
				OnMouseDown ();

			}
		}
	}

	void OnMouseDown(){
		Debug.Log ("Called for" + this.gameObject.name);
		if (this.gameObject.name == "Close") {
			Application.Quit ();
//		}else if (this.gameObject.name == "Test") {
//			SceneManager.LoadScene ("Test");
//		}else if (this.gameObject.name == "level_1") {
//			SceneManager.LoadScene ("Scene1");
//		}else if (this.gameObject.name == "level_1") {
//			SceneManager.LoadScene ("Scene2");
//		}else if (this.gameObject.name == "level_1") {
//			SceneManager.LoadScene ("Scene3");
//		}else if (this.gameObject.name == "level_1") {
//			SceneManager.LoadScene ("Scene4");
		} 
//		else if (this.gameObject.name == "Home") {
			
//		} 
	else {
			SceneManager.LoadScene (sceneToLoad);
		}

	}
}
