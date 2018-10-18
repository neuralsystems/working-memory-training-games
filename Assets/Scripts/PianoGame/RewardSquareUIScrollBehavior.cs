using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSquareUIScrollBehavior : MonoBehaviour {

    private float height;
    private Vector2 hidePos;
    private Vector2 showPos;

    // Use this for initialization
    void Start () {
        height = this.GetComponent<RectTransform>().sizeDelta.y;
        showPos = this.GetComponent<RectTransform>().anchoredPosition;
        hidePos = new Vector2(showPos.x, showPos.y + height);
    }

    public void Show(bool show)
    {
        this.GetComponent<RectTransform>().anchoredPosition = (show) ? showPos: hidePos;
    }

    //scrolls to required reward square UI (wrt pivot)
    public void SnapTo(RectTransform target)
    {
        var scrollRect = transform.GetComponent<ScrollRect>();
        var contentPanel = GameObject.FindWithTag(Camera.main.GetComponent<SceneVariables>().REWARD_SQUARE_UI_PARENT_TAG).GetComponent<RectTransform>();
        var rewardSqUIWidth = GameObject.FindWithTag(Camera.main.GetComponent<SceneVariables>().REWARD_SQUARE_UI_TAG).GetComponent<RectTransform>().sizeDelta.x;
        Canvas.ForceUpdateCanvases();

        contentPanel.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
        contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x + rewardSqUIWidth / 2, 0); // assumes top-left pivot
    }

    // Update is called once per frame
    void Update () {
		
	}
}
