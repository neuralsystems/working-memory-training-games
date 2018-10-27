using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSquareUIScrollBehavior : MonoBehaviour {

    float height;
    
    // Use this for initialization
    void Start () {
        height = this.GetComponent<RectTransform>().sizeDelta.y;
    }

    public IEnumerator Show(bool val)
    {
        if (val)
        {
            yield return null;
            GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
        }
        yield return null;
        var contentPanel = Camera.main.GetComponent<SceneVariables>().contentPanel;
        foreach (Transform rewardSq in contentPanel)
        {
            rewardSq.GetComponent<Image>().enabled = val;
        }
    }

    public IEnumerator MoveToAnchoredPosition(Vector2 pos)
    {
        while(Vector2.Distance(transform.GetComponent<RectTransform>().anchoredPosition, pos) > 0.01f)
        {
            Vector2.MoveTowards(transform.GetComponent<RectTransform>().anchoredPosition, pos, Time.deltaTime);
            yield return null;
        }
        transform.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public IEnumerator MoveDown()
    {
        var shiftHeight = height;
        var anchPos = transform.GetComponent<RectTransform>().anchoredPosition;
        yield return StartCoroutine(MoveToAnchoredPosition(new Vector2(anchPos.x, anchPos.y - shiftHeight)));
    }

    //scrolls by shiftCount reward squares
    public IEnumerator ScrollTo(int rewardIndex, int n, int maxVisibleRewardSq)
    {
        var scrollStep = 1f / (n - maxVisibleRewardSq);
        var initScrollVal = GetComponent<ScrollRect>().horizontalNormalizedPosition;
        var scrollTarget = Mathf.Min(1f,scrollStep * rewardIndex);
        
        //Debug.Log("Scroll Step: " + scrollStep);
        Debug.Log("Scrolling to " + scrollTarget);

        var t = 0f;
        while (Mathf.Abs(GetComponent<ScrollRect>().horizontalNormalizedPosition - scrollTarget) > 0.02f)
        {
            GetComponent<ScrollRect>().horizontalNormalizedPosition = Mathf.Lerp(initScrollVal, scrollTarget, t);
            t += 0.15f;
            yield return null;
        }
        GetComponent<ScrollRect>().horizontalNormalizedPosition = scrollTarget;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
