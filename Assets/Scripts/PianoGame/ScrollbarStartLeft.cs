using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarStartLeft : MonoBehaviour {

    public GameObject scrollbarContent;
    //public IEnumerator StartLeft()
    //{
    //    yield return null;
    //    var viewportWidth = transform.GetComponent<RectTransform>().sizeDelta.x;
    //    var contentWidth = scrollbarContent.GetComponent<RectTransform>().sizeDelta.x;
    //    var anchPos = scrollbarContent.GetComponent<RectTransform>().anchoredPosition;
    //    var leftPos = (contentWidth - viewportWidth) / 2;
    //    if (leftPos > 0f)
    //    {
    //        scrollbarContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(leftPos, anchPos.y);
    //    }
    //}

    public IEnumerator StartLeft()
    {
        yield return null;
        GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
