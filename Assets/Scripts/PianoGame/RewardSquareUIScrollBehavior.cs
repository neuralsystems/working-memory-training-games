using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Update is called once per frame
	void Update () {
		
	}
}
