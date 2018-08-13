using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
public class KeySquareBehavior : MonoBehaviour
{

	Vector3 target, target_y;
	public float smoothTime = 0.1F;
	float widthPercentage, heightPercentage;
	private Vector3 velocity = Vector3.zero;
	public string tagForGameObject;
	public Sprite originalSprite;
	string intermediate_1 = "Intermediate1";
	float overlapfraction = 0.1f;
	Color original_color;

	// Use this for initialization
	void Start ()
	{

		originalSprite = GetComponent<SpriteRenderer> ().sprite;
		original_color = GetComponent<SpriteRenderer> ().color;
//		widthPercentage = Camera.main.GetComponent<SceneVariables> ().widthPercentage;
//		heightPercentage = Camera.main.GetComponent<SceneVariables> ().heightPercentage;
		var shift = GetComponent<SpriteRenderer> ().bounds.size;
		if (tagForGameObject == Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG) {
			target = Camera.main.GetComponent<SceneVariables> ().targetUserSquare;
			target.y += (shift.y) * (1-overlapfraction);
			Camera.main.GetComponent<SceneVariables> ().targetUserSquare.x += shift.x;
		} else {
			target = Camera.main.GetComponent<SceneVariables> ().target;
			Camera.main.GetComponent<SceneVariables> ().target.x += shift.x;
			GetComponent<SpriteRenderer> ().sortingLayerName = intermediate_1;
		}
		StartCoroutine (MoveUp (target, tagForGameObject));
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
		var Sample_square_parent_object = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_PARENT);
		var user_input_parent_object = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT);
		if (Vector3.Distance (transform.position, target) > SceneVariables.MIN_DISTANCE) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
			StartCoroutine (MoveUp (target, tagForGameObject));
		} else {
			transform.position = target;
			if (tagForGameObject == Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_TAG) {
				transform.parent = user_input_parent_object.gameObject.transform;
			} else {
				transform.parent = Sample_square_parent_object.gameObject.transform;
				GetComponent<SpriteRenderer> ().sprite = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().questionSquare).gameObject.GetComponent<SpriteRenderer> ().sprite;
			}
			Transform[] user_squares_gameobject = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).GetComponentsInChildren<Transform>();
			Transform[] sample_squares_gameobject = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_PARENT).GetComponentsInChildren<Transform>();
			var num_of_user_squares = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().USER_INPUT_SQUARE_PARENT).transform.childCount;
			var num_of_sample_squares = GameObject.Find (Camera.main.GetComponent<SceneVariables> ().SAMPLE_SQUARE_PARENT).transform.childCount;
			if (num_of_sample_squares == num_of_user_squares) {
				var all_matched = true;
				for (int i = 1; i <= num_of_user_squares; i++) {
					var matched = (user_squares_gameobject [i].gameObject.GetComponent<SpriteRenderer> ().sprite == sample_squares_gameobject [i].gameObject.GetComponent<KeySquareBehavior> ().originalSprite);
					all_matched = all_matched && matched;
					Debug.Log (all_matched + " all_matched");
					sample_squares_gameobject [i].gameObject.GetComponent<KeySquareBehavior> ().ResetSquare ();
					if (!matched) {
						user_squares_gameobject[i].transform.parent =null;
					}
				}
                if (!all_matched)
                {
                    //SceneVariables.error_count++;
                    Debug.Log("not matched and error count increased to " + SceneVariables.error_count);
                }
				yield return new WaitForSeconds (0.5f);
				var shift = GetComponent<SpriteRenderer>().sprite.bounds.size.y * (1-overlapfraction);
				var target_for_parent = user_input_parent_object.transform.position;
				target_for_parent.y -= shift;
				//user_input_parent_object.GetComponent<PianoGame_UserInputSquareParentBehavior> ().All_Match = all_matched;
				StartCoroutine(user_input_parent_object.GetComponent<PianoGame_UserInputSquareParentBehavior>().MoveToTarget(target_for_parent,all_matched));
			}
		}

	}

	public IEnumerator MoveToTarget (Vector3 target)
	{
		if (Vector3.Distance (transform.position, target) > Mathf.Min(SceneVariables.MIN_DISTANCE,0.0001f)) {
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

	public IEnumerator MoveToReward ( GameObject rewardTile, bool shouldCall)
	{
		var target = rewardTile.transform.position;
//		transform.localScale = rewardTile.transform.localScale * (rewardTile.GetComponent<SpriteRenderer>().bounds.size.x / GetComponent<SpriteRenderer>().bounds.size.x);
		transform.localScale = rewardTile.transform.localScale * .5f;
		while(Vector3.Distance (transform.position, target) > SceneVariables.MIN_DISTANCE) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			yield return null;
//			StartCoroutine (MoveToReward (rewardTile,shouldCall));
		} 
//		else {
		transform.position = target;
		if (rewardTile.transform.childCount == 0) {
			transform.parent = rewardTile.transform;
            transform.localPosition = new Vector3(-.17f,-.13f,0f);
			tag = Camera.main.GetComponent<SceneVariables> ().REWARD_SQUARE_CHILD_TAG;
			GetComponentInParent<ParticleSystem> ().Play ();
			if (shouldCall) {
				Camera.main.GetComponent<PlayTone> ().CheckOnComplete ();
			}
//				rewardTile.GetComponent<PianoGame_RewardSquareBehavior> ().SetVisibility(false);
//				rewardTile.GetComponent<Outline> ().eraseRenderer = true;
		} else {
			Destroy (gameObject);
		}

//		}


	}

	public void ChangeColor(Color color_value){
		GetComponent<SpriteRenderer> ().color = color_value;
	}

	public void ResetColor(){
		GetComponent<SpriteRenderer> ().color = original_color;
	}
}
