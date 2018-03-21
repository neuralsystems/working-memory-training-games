using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBehavior : MonoBehaviour {

	float waitTime = .1f;
	public float speed = 1f;
	// Use this for initialization
	void Start () {
		GameObject[] go = GameObject.FindGameObjectsWithTag ("OptionTileTag");
		int x;
		if (go.Length > 1) {
			x = Random.Range (0, go.Length - 1);
		} else {
			x = 0;
		}
		Debug.Log ("choosing " + x + " image");
		GetComponent<SpriteRenderer> ().sprite = go [x].GetComponent<ImageEffect> ().oldSprite;
		StartCoroutine(MoveCue (CarGame_SceneVariables.stopVector));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator MoveCue(Vector3 target){
		
		var step = CarGame_SceneVariables.speed * Time.deltaTime;
//		var velocity = Vector3.zero;
		transform.position = Vector3.MoveTowards (transform.position, target,  step);
		if (transform.position != target) {
			yield return null;
			yield return StartCoroutine(MoveCue (target));
		}else  {
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


}
