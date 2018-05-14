using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TrainGame_SceneVariables : MonoBehaviour {


	// Tags 
	public const string ENGINE_TAG = "EngineTag";
	public const string BOGIE_TAG = "BogieTag";
	public const string KEYLOCK_TAG = "KeyLockTag";
	public const string ATTACHED_BOGIE_TAG = "AttachedBogieTag";
	public const string Game_Name = "TrainGame";
	public const string DATABASE_NAME = "TrainGame_DB.db";
	Dictionary <string, string> key_lock_map = new Dictionary<string, string>{
		{"K1_l","K1_r"},
		{"K2_l","K2_r"},
		{"K3_l","K3_r"},
		{"K4_l","K4_r"},
		{"K5_l","K5_r"},
		{"K6_l","K6_r"}
	};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<string> GetKeys(int n){
		System.Random rand = new System.Random();
		List<string> values = new List<string>(key_lock_map.Keys);
		return RandomizingArray.RandomizeStrings (values.ToArray ());
	}

	public string GetValue(string key){
		if(key_lock_map.ContainsKey(key)){
			return (key_lock_map [key]);
		}
		return "";
	}
	public string GetBasketFolderName (){
		return "LockAndKey/New/";
	}
}
