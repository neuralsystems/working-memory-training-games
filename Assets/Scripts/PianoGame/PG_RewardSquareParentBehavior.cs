using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PG_RewardSquareParentBehavior : MonoBehaviour {

	public float smoothTime = 10.0050F;
	Vector3 velocity = Vector3.zero;
	Vector3 original_position, camera_position;
	Coroutine scroll_movement;

    public Transform content_panel;
    public GameObject RewardSquareUIPoolObject;
	// Use this for initialization
	void Start () {
		camera_position = Camera.main.transform.position;
		original_position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator MoveToTarget ( Vector3 target)
	{

		while (Vector3.Distance (transform.position, target) > 0.01f) {
			transform.position = Vector3.SmoothDamp (transform.position, target, ref velocity, smoothTime);
//			transform.position = Vector3.down * Time.deltaTime;
			yield return new WaitForSeconds(0.005f);
			yield return null;
//			StartCoroutine (MoveToTarget (target));
		} 
//		else {
		transform.position = target;
//			if (target == original_position) {
//				
//			}
//		}


	}

	public void ResetPosition(){
		StartCoroutine(MoveToTarget(original_position));
	}

    public int MaximumVisibleRewardSquares()
    {
        var rewardSqWidth = Camera.main.GetComponent<SceneVariables>().rewardSquare.GetComponent<SpriteRenderer>().bounds.size.x;
        var viewportWidth = Shared_ScriptForGeneralFunctions.GetPointOnScreen(1, 1).x * 2;
        return (int)(viewportWidth / rewardSqWidth);
    }

    public void SnapTo(int rewardIndex, int n, int maxVisibleRewardSq)
    {
        var rewardSqWidth = Camera.main.GetComponent<SceneVariables>().rewardSquare.GetComponent<SpriteRenderer>().bounds.size.x;
        var viewportWidth = Shared_ScriptForGeneralFunctions.GetPointOnScreen(1, 1).x * 2;
        float targetPosX;
        if ((n - rewardIndex) >= maxVisibleRewardSq)
        {
            targetPosX = -(rewardSqWidth * rewardIndex); //snap to position: rewardIndex
        }
        else
        {
            targetPosX = -(n * rewardSqWidth - viewportWidth); //snap to end of reward square list
        }
        var targetPos = new Vector3(targetPosX, transform.position.y);
        transform.position = targetPos;

        //Debug.Log("targetPos: " + targetPos);
        //Debug.Log("rewardSqParentPos: " + transform.position);
    }

    public void ScrollRewardSquares(bool direction){
		var target = original_position;
		// right swipe
		if (direction) {
			var last_square_gameobject = transform.GetChild(transform.childCount);
//			if(last_square_gameobject.x > Shared_ScriptForGeneralFunctions.GetPointOnScreen(

		} else {
			
		}
		scroll_movement = StartCoroutine(MoveToTarget(target));
	}

	public void OnMouseDown(){
		StopCoroutine (scroll_movement);
	}

	public void MoveCamera(Vector3 new_position){
//		Camera.main.transform.position = camera_position;
		StartCoroutine (MoveToTarget (new_position));
	}

    public IEnumerator ReflectOnScrollList()
    {
        //transform.parent = contentPanel;
        var rewardSquareScroll = GameObject.Find(Camera.main.GetComponent<SceneVariables>().REWARD_SQUARE_UI_SCROLL);  //rewardSquareUIScroll > ViewPort > content_panel
        int num_own_child = transform.childCount;
        int num_content_child = content_panel.transform.childCount;
        Debug.Log("num own count and num content count = " + num_own_child+ " "+ num_content_child  );
        for(int i =0; i < num_own_child; i++)
        {
            if (i >= num_content_child)
            {
                AddChildToContent();
            }
        }
        for(int i = num_own_child; i < num_content_child; i++)
        {
            Debug.Log("Calling Delete UI pool object");
            DeleteLastChildFromContent();
        }

        yield return StartCoroutine(rewardSquareScroll.GetComponent<RewardSquareUIScrollBehavior>().Show(true)); //show ui rewardsqs, then disable normal reward sqs

        for (int i = 0; i < num_own_child; i++)
        {
            transform.GetChild(i).GetComponent<PianoGame_RewardSquareBehavior>().SetObjAndChildVisibility(false);
        }
        //        rewardSquareUIScroll.GetComponent<RewardSquareUIScrollBehavior>().SnapTo(content_panel.GetChild(0).GetComponent<RectTransform>());
    }

    void AddChildToContent()
    {
        var UI_object = RewardSquareUIPoolObject.GetComponent<SimpleObjectPool>().GetObject();
        UI_object.transform.parent = content_panel;
        UI_object.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    void DeleteLastChildFromContent()
    {
        var _last_index = content_panel.transform.childCount - 1;
        RewardSquareUIPoolObject.GetComponent<SimpleObjectPool>().ReturnObject(content_panel.transform.GetChild(_last_index).gameObject);
        Debug.Log("Deleting UI pool object");
    }
}

