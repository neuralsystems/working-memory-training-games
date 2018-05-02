using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControlSwipe : MonoBehaviour {

	Vector3 fp,lp;					// inital and last position of the sipe
	public float DragDistance;		// minimum distance 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				fp = touch.position;
				lp = touch.position;
			}

			if (touch.phase == TouchPhase.Moved)
			{
				lp = touch.position;
			}

			if (touch.phase == TouchPhase.Ended)
			{
				//First check if it’s actually a drag
				Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
//				DisplayText.text += "11";
				Vector2 touchPos = new Vector2(wp.x, wp.y);
				if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
				{
//					DisplayText.text += "1";
					if (Mathf.Abs(lp.x-fp.x) > DragDistance )
					{ 
						OnSwipe (lp.x > fp.x);
					}
					else
					{
						//It’s a tap
//						DisplayText.text = "Tapping";
						GetComponent<PG_RewardSquareParentBehavior>().OnMouseDown();
//						StopCoroutine (MoveObjectToTarget (new Vector3(0f,0f,0f)));
					}
				}

			}
		}
	}


	// direction = 1 for right swipe and 0 for left swipe
	void OnSwipe(bool direction){
		if (name == Camera.main.GetComponent<SceneVariables> ().REWARD_SQUARE_PARENT) {
			GetComponent<PG_RewardSquareParentBehavior>().ScrollRewardSquares(direction);
		}
	}


}
