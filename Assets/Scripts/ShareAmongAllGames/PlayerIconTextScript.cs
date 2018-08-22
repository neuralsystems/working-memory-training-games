using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerIconTextScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = transform.parent.name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
