using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarStartLeft : MonoBehaviour {

    //set left pivot
    //public GameObject scrollbarContent;
    //public void StartLeft()
    //{
    //    var anchPos = scrollbarContent.GetComponent<RectTransform>().anchoredPosition;
    //    scrollbarContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, anchPos.y);
    //}

    public void StartLeft()
    {
        GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
