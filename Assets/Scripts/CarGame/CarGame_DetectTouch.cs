using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGame_DetectTouch : MonoBehaviour {


	public Vector3 origin;
	// Use this for initialization
	void Start () {
		origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// when the object is Touched


			if (Input.touchCount == 1)
			{
				Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				Vector2 touchPos = new Vector2(wp.x, wp.y);
				if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
				{
					// code when the object is tapped
					OnMouseDown();
				}
			}
		}

	void OnMouseDown(){
		GetComponentInChildren<SpriteRenderer> ().enabled = false;
		Debug.Log ("tapped me?");
		GameObject cueObject = GameObject.FindGameObjectWithTag (CarGame_SceneVariables.cueTag);
//		Debug.Log (cueObject.name);
		CarGame_GameManager gm = Camera.main.GetComponent<CarGame_GameManager>();
		GetComponent<SpriteRenderer> ().sprite = GetComponent<ImageEffect> ().oldSprite;
		Camera.main.GetComponent<Timer>().StopTimer();
		if (GetComponent<ImageEffect> ().oldSprite == cueObject.GetComponent<SpriteRenderer> ().sprite) {
			Debug.Log ("matched");
			tag = CarGame_SceneVariables.matchedTag;
			GetComponent<MergeOptionCue> ().enabled = true;
//			GetComponent<Scalling> ().SetScale (false);
//			var gmCamera.main.GetComponent<CarGame_GameManager>();
			gm.UpdateScoreText (1);
			gm.Match();
		} else {
			tag = CarGame_SceneVariables.selectedTileTag;
			gm.UpdateScoreText (0);
			gm.MisMatch ();
		}
	}

	public void SetTouch( bool value){
		Debug.Log ("Set Values with "+ value);
		GetComponent<CarGame_DetectTouch> ().enabled = true;
		GetComponent<BoxCollider2D> ().enabled = true;
	}
}
