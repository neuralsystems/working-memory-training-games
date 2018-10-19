using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSquareUIScrollBehavior : MonoBehaviour {

    float height;
    Vector2 hidePos;
    Vector2 showPos;
    Vector2 velocity = Vector2.zero;
    float smoothTime = .5f;
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

    //scrolls by shiftCount reward squares
    public IEnumerator ScrollTo(int rewardIndex, int n, int maxVisibleRewardSq)
    {
        var scrollStep = 1f / (n - maxVisibleRewardSq);
        var initScrollVal = GetComponent<ScrollRect>().horizontalNormalizedPosition;
        var scrollTarget = scrollStep * rewardIndex;

        if(scrollTarget > 1f)
        {
            scrollTarget = 1f;
        }
        //Debug.Log("Scroll Step: " + scrollStep);
        Debug.Log("Scrolling to " + scrollTarget);

        var t = 0f;
        while (Mathf.Abs(GetComponent<ScrollRect>().horizontalNormalizedPosition - scrollTarget) > 0.01f)
        {
            GetComponent<ScrollRect>().horizontalNormalizedPosition = Mathf.Lerp(initScrollVal, scrollTarget, t);
            t += 0.05f;
            yield return null;
        }
        GetComponent<ScrollRect>().horizontalNormalizedPosition = scrollTarget;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
