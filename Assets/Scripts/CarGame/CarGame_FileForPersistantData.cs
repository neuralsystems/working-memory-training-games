using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGame_FileForPersistantData : MonoBehaviour {

	public static int level = 1;

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
