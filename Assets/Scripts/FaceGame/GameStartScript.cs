using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScript : MonoBehaviour {
    
	// Use this for initialization
	void Start () {

		Database database = FindObjectOfType<Database>();
		FaceGame_DataService dataController = new FaceGame_DataService(database.tagsAndNames_sqliteDB);
        // *** uncomment the block if always want to show the basic the one before the pre level 

        //var persistant_go = GameObject.Find(Database.masterGo);
        //var user_obj = persistant_go.GetComponent<Shared_PersistentScript>().GetCurrentPlayer();
        //var user = user_obj.Username;
        //dataController.SetLevel(-1,user);
        //dataController.SetProgress(user, 0);

        // ***** 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
