using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySquareBehavior : MonoBehaviour
{

	Vector3 target, target_y;
	public float smoothTime = 0.1F;
	float widthPercentage, heightPercentage;
	private Vector3 velocity = Vector3.zero;
	public string tagForGameObject;
	public Sprite originalSprite;
	string intermediate_1 = "Intermediate1";
	// Use this for initialization
	void Start ()
	{

		originalSprite = GetComponent<SpriteRenderer> ().sprite;
//		blockSprite = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().blockSquare).gameObject.GetComponent<SpriteRenderer> ().sprite;
		widthPercentage = Camera.main.GetComponent<SceneVariables> ().widthPercentage;
		heightPercentage = Camera.main.GetComponent<SceneVariables> ().heightPercentage;
		var shift = GetComponent<SpriteRenderer> ().bounds.size;
		if (tagForGameObject == Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG) {
//			transform.parent = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).gameObject.transform;
			target = Camera.main.GetComponent<SceneVariables> ().targetUserSquare;
			target.y += (shift.y) * 0.9f;
			Camera.main.GetComponent<SceneVariables> ().targetUserSquare.x += shift.x;
		} else {
//			transform.parent = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_PARENT).gameObject.transform;
			target = Camera.main.GetComponent<SceneVariables> ().target;
			Camera.main.GetComponent<SceneVariables> ().target.x += shift.x;
			GetComponent<SpriteRenderer> ().sortingLayerName = intermediate_1;
		}
		StartCoroutine (MoveUp (target, tagForGameObject));
//		ChangeTag ();
	}


	// Update is called once per frame
	void Update ()
	{
		
		
	}

	public void ResetSquare ()
	{
		GetComponent<SpriteRenderer> ().sprite = originalSprite;
	}

	public IEnumerator MoveUp (Vector3 target, string tagForGameObject)
	{

		if (Vector3.Distance (transform.position, target) > SceneVariables.MIN_DISTANCE) {
//			Debug.Log ("first if");
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			StartCoroutine (MoveUp (target, tagForGameObject));
		} else {
			transform.position = target;
//			tag = tagForGameObject;
			if (tagForGameObject == Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG) {
				if (GameObject.FindGameObjectsWithTag (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG).Length == 1) {
					GameObject.Find (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).gameObject.transform.position = transform.position;
				}
				transform.parent = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).gameObject.transform;
			} else {
				if (GameObject.FindGameObjectsWithTag (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_TAG).Length == 1) {
					GameObject.Find (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_PARENT).gameObject.transform.position = transform.position;
				}
				transform.parent = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_PARENT).gameObject.transform;
				GetComponent<SpriteRenderer> ().sprite = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().questionSquare).gameObject.GetComponent<SpriteRenderer> ().sprite;
			}
//			if (tagForGameObject == Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_TAG) {
//				GetComponent<SpriteRenderer> ().sprite = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().questionSquare).gameObject.GetComponent<SpriteRenderer> ().sprite;
//			}
//			yield return new WaitForSeconds (0.4f);
//			if (Camera.main.GetComponent<SceneVariables> ().correctMatch ) {
////				var userSquares = GameObject.FindGameObjectsWithTag (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG);
////				var computerSquares = GameObject.FindGameObjectsWithTag (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_TAG);
			Transform[] user_squares_gameobject = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).GetComponentsInChildren<Transform>();
			Transform[] sample_squares_gameobject = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_PARENT).GetComponentsInChildren<Transform>();
			var num_of_user_squares = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).transform.childCount;
			var num_of_sample_squares = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_PARENT).transform.childCount;
			if (num_of_sample_squares == num_of_user_squares) {
//				Camera.main.GetComponent<SceneVariables> ().correctMatch = false;
				var all_matched = true;
				for (int i = 1; i <= num_of_user_squares; i++) {
					var matched = (user_squares_gameobject [i].gameObject.GetComponent<SpriteRenderer> ().sprite == sample_squares_gameobject [i].gameObject.GetComponent<KeySquareBehavior> ().originalSprite);
					all_matched = all_matched && matched;
					Debug.Log (all_matched + " all_matched");
					if (matched) {
						sample_squares_gameobject [i].gameObject.GetComponent<KeySquareBehavior> ().ResetSquare ();
						yield return new WaitForSeconds (0.5f);
						StartCoroutine(user_squares_gameobject [i].gameObject.GetComponent<KeySquareBehavior> ().MoveToTarget (sample_squares_gameobject [i].position));
						yield return new WaitForSeconds (0.5f);

					}

				}
				if (all_matched) {
					GetComponentInParent<ParticleSystem> ().Play ();
//					Camera.main.GetComponent<AudioSource> ().Play ();
//					Camera.main.GetComponent<PlayTone>().PlayTillComplete();
					Camera.main.GetComponent<SceneVariables> ().GetRandomClapping ();
					Camera.main.GetComponent<SceneVariables> ().ShowSquares ();
				}

				yield return new WaitForSeconds (1f);
				if (Camera.main.GetComponent<PlayTone> ().consequtive_correct >= Camera.main.GetComponent<SceneVariables> ().CONSECUTIVE_CORRECT_THRESHOLD) {
					SceneVariables.IS_USER_MODE = false;
					StartCoroutine (Camera.main.GetComponent<PlayTone> ().DisplayOnLevelComplete ());
				} else {
					GameObject.Find (Camera.main.GetComponent<SceneVariables> ().playSound).GetComponent<SpriteRenderer> ().enabled = true;
				}
			}
//			}
		}

	}

	public IEnumerator MoveToTarget (Vector3 target)
	{
		if (Vector3.Distance (transform.position, target) > Mathf.Min(SceneVariables.MIN_DISTANCE,0.0001f)) {
			//			Debug.Log ("first if");
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			StartCoroutine (MoveToTarget (target));
		} else {
			transform.position = target;

		}

	}


	void ChangeTag ()
	{

		if (!SceneVariables.IS_USER_MODE) {
			tag = Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG;
		} else {
			tag = Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_TAG;
		}
	}



}
