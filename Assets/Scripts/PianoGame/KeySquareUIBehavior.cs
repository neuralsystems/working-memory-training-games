using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySquareUIBehavior : MonoBehaviour {

    Vector2 originalPosition; 
    Vector3 originalScale;
    // Use this for initialization
    void Start () {
        originalPosition = transform.GetComponent<RectTransform>().anchoredPosition;
        originalScale = transform.GetComponent<RectTransform>().localScale;
    }
	
    public void SetImage(Sprite keySprite)
    {
        this.GetComponent<Image>().enabled = true;
        this.GetComponent<Image>().sprite = keySprite;
    }

    public void Pop(bool value)
    {
        if(value)
        {
            transform.GetComponent<RectTransform>().localScale += new Vector3(1, 1, 1) * .3f;
            transform.GetComponent<Rigidbody2D>().gravityScale = 40;
            transform.GetComponent<Rigidbody2D>().velocity = new Vector3(200f, 0f, 0f);
        }
        else
        {
            transform.GetComponent<RectTransform>().localScale = originalScale;
            transform.GetComponent<RectTransform>().anchoredPosition = originalPosition;
            transform.GetComponent<Rigidbody2D>().gravityScale = 0;
            transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            transform.GetComponent<Image>().enabled = false;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
