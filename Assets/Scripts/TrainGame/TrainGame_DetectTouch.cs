using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGame_DetectTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1)
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPos = new Vector2(wp.x, wp.y);
			if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
			{
				if(gameObject.tag == TrainGame_SceneVariables.BOGIE_TAG){
					GetComponent<TrainGame_BogieBehavior> ().OnMouseDown ();
				} else if(gameObject.tag == TrainGame_SceneVariables.KEYLOCK_TAG){
					GetComponentInParent<TrainGame_BogieBehavior> ().OnMouseDown ();
				}
			}
		}
	}


			
}
