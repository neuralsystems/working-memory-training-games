using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
public class PianoGame_RewardSquareBehavior : MonoBehaviour {

	public float smoothTime = .0050F;
	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public IEnumerator MoveToTarget ( Vector3 target, bool show_play_button, string object_tag = null)
	{
		
		while (Vector3.Distance (transform.position, target) > 0.01f) {
//			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
			transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * 3f);
			yield return null;
//			StartCoroutine (MoveToTarget (target, object_tag));
		} 
//		else {
		transform.position = target;
		if(object_tag == Camera.main.GetComponent<SceneVariables>().REWARD_SQUARE_TAG){
			tag = object_tag;
            //if (GameObject.FindGameObjectsWithTag (Camera.main.GetComponent<SceneVariables> ().NON_REWARD_SQUARE_TAG).Length == 0) {
            Debug.Log("Length 0");
            if (show_play_button)
            {
                GameObject.Find(Camera.main.GetComponent<SceneVariables>().playSound).GetComponent<HomeScreenButtons>().SetHaloToggle(true);
                //transform.parent.GetComponent<PG_RewardSquareParentBehavior>().ReflectOnScrollList();
            }
            //transform.parent = Camera.main.GetComponent<SceneVariables>().contentPanel;
            //} else {
            //Debug.Log ("Length !0");
            //}
        }
        //		Debug.Log (smoothTime);
        if (GetComponent<Animator> ()) {
			Stand ();
		}
	}

	public void SetVisibility(bool value){
		GetComponent<SpriteRenderer> ().enabled = value;
//		GetComponent<Outline> ().eraseRenderer = value;
	}

	public void Stand(){
		GetComponent<Animator> ().SetInteger ("WalkorStand", 1);
	}

}
