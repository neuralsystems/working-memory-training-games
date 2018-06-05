using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWM_TowerScript : MonoBehaviour {
    int index = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Transform GetNextChild()
    {
        return transform.GetChild(index++);
    }
}
