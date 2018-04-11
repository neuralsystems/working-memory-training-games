using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBehavior : MonoBehaviour {

	float waitTime = .1f;
	public float speed = 1f;
	public Vector3 velocity = Vector3.zero, original_position;
	public float smoothTime = .5f;
	// Use this for initialization
	void Start () {
		var screenWidth = Camera.main.pixelWidth;
		var screenHeight = Camera.main.pixelHeight;
		var cue_start_position = Camera.main.ScreenToWorldPoint (new Vector3 (0 , screenHeight * .4f, Camera.main.nearClipPlane));
		transform.position = cue_start_position;

		var cue_end_postion = Camera.main.ScreenToWorldPoint (new Vector3 (screenWidth/2 , screenHeight * .4f, Camera.main.nearClipPlane));
		GameObject[] go = GameObject.FindGameObjectsWithTag ("OptionTileTag");
		int x;
		if (go.Length > 1) {
			x = Random.Range (0, go.Length - 1);
		} else {
			x = 0;
		}
		Debug.Log ("choosing " + x + " image");
		GetComponent<SpriteRenderer> ().sprite = go [x].GetComponent<ImageEffect> ().oldSprite;
		transform.localScale = new Vector3 (1, 1, 1) * go [x].GetComponent<Scalling> ().maxSize;
		original_position = transform.position;
		original_position.x -= GetComponent<SpriteRenderer> ().bounds.size.x;
		StartCoroutine(MoveCue (cue_end_postion,false));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator MoveCue(Vector3 target, bool isBack){
		
		var step = CarGame_SceneVariables.speed * Time.deltaTime;
//		var velocity = Vector3.zero;
		if (Vector3.Distance(transform.position, target) >  CarGame_SceneVariables.MIN_DISTANCE) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			StartCoroutine(MoveCue (target,isBack));
		}else  {
			if (isBack) {
				Destroy (gameObject);
			}
			Debug.Log ("timer started");
			Camera.main.GetComponent<Timer>().startTimer ();
			GameObject[] go = GameObject.FindGameObjectsWithTag (CarGame_SceneVariables.OptionTileTag);
			foreach (GameObject g in go) {
				g.GetComponent<CarGame_DetectTouch> ().SetTouch (true);
				g.GetComponent<Scalling> ().SetScale(true);
			}
		}
//		if (transform.position == GameObject.Find (SceneVariables.eor).transform.position) {
//			SceneVariables sc = new SceneVariables ();
//			sc.ResetorRestart ();
//			Destroy (this.gameObject);
//		}
	}

	public void MoveBackCue(){
		StartCoroutine(MoveCue (original_position,true));	
	}

}
