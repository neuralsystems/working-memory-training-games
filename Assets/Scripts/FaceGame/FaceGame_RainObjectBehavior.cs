using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceGame_RainObjectBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < (Shared_ScriptForGeneralFunctions.GetPointOnScreen(0f, 0f).y - 1))
        {
            Destroy(gameObject);
        }
	}

   
}
