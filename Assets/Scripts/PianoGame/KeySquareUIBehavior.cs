using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySquareUIBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void SetImage(Sprite keySprite)
    {
        this.GetComponent<Image>().enabled = true;
        this.GetComponent<Image>().sprite = keySprite;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
