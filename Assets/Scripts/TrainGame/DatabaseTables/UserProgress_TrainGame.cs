using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System;
public class UserProgress_TrainGame {

    [PrimaryKey, AutoIncrement]
    public int Id { set; get; }
    public string User_Obj { get; set; }
    public int Level_Obj { get; set; }
    public int PreLevelCompleted { get; set; }                  // set to 1 if pre level is complete any other value means pre level isn't complete
    public DateTime LastModified { get; set; }
    public DateTime DateCreated { get; set; }

    public bool HasCompletedPreLevel()
    {
        return this.PreLevelCompleted == TrainGame_SceneVariables.VALUE_FOR_PRE_LEVEL_COMPLETE;
    }

}
