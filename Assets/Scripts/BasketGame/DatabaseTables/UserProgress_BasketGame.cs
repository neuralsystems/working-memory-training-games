using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System;
public class UserProgress_BasketGame  {

	[PrimaryKey, AutoIncrement]
    public int Id { set; get; }
    public string User_Obj { get; set; }
    public int Level_Obj { get; set; }
    public int PreLevelCompleted { get; set; }                  // set to 1 if pre level is complete any other value means pre level isn't complete
    public DateTime LastModified { get; set; }
    public DateTime DateCreated { get; set; }

    //public Set UserProgress_BasketGame(string username)
    //{
    //    this.User_Obj = username;
    //    this.Level_Obj = default_level;
    //}

    public bool hasCompletedPreLevel()
    {
        return this.PreLevelCompleted == BasketGame_SceneVariables.VALUE_FOR_PRE_LEVEL_COMPLETE;
    }
}
