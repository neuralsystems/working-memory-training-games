using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWM_TowerScript : MonoBehaviour {
    int index = 0;
	// Use this for initialization
	void Start () {
        Reset();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Transform GetNextChild()
    {
        return transform.GetChild(index++);
    }

    private void Reset()
    {
        index = 0;
    }
}
