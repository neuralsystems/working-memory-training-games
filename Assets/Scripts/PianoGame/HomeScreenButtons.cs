using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HomeScreenButtons : MonoBehaviour {

	float maxSize = 2f, minSize = 0f, growFactor = 0.1f, waitTime = 0.1f;
	public string sceneToLoad;
	public bool shouldToggle ;
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
			SceneManager.LoadScene (sceneToLoad);
		} else if (this.gameObject.name == "Home") {
			
		}else if (this.gameObject.name == Camera.main.GetComponent<SceneVariables>().playSound) {
			if (GetComponent<SpriteRenderer> ().enabled) {
				OnKeyPress.userString = "";
				Debug.Log ("Clicked me?");
				Camera.main.GetComponent<AudioSource>().Stop();
				Camera.main.GetComponent<PlayTone> ().PlayToneFromExternal ();
				SetHaloToggle(false);
			}
		}else {
			SceneManager.LoadScene (sceneToLoad);
		}

	}


	IEnumerator ChangeHaloIntensity(){
		float timer = 0;


		// we scale all axis, so they will have the same value, 
		// so we can work with a float instead of comparing vectors

		while(maxSize > GetComponent<Light>().intensity )
		{
			timer += Time.deltaTime;
			GetComponent<Light>().intensity += growFactor;
			yield return null;
		}
		// reset the timer

		//			yield return new WaitForSeconds(waitTime);

		timer = 0;
		while(minSize < GetComponent<Light>().intensity )
		{
			timer += Time.deltaTime;
			GetComponent<Light>().intensity -=  growFactor;
			yield return null;
		}

		timer = 0;
		yield return new WaitForSeconds(waitTime);
		if (shouldToggle) {
//			StopCoroutine (ChangeHaloIntensity ());
			StartCoroutine (ChangeHaloIntensity ());
		}
	}

	public void SetHaloToggle(bool value){
		Debug.Log ("Set halo called with value = " + value);
		GetComponent<SpriteRenderer> ().enabled = value;
		shouldToggle = value;
		GetComponent<Light> ().intensity = 0;
		if (value) {
			StopCoroutine (ChangeHaloIntensity ());
			StartCoroutine (ChangeHaloIntensity ());
		} else {
			StopCoroutine (ChangeHaloIntensity ());	
		}
	}

}
