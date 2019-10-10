using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System;
public class UserProgress_ShapeMatch 
{
    [PrimaryKey, AutoIncrement]
    public int Id{ get; set; }
    public string Username { get; set; }
    public int LevelCompleted { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastModified { get; set; }

    public override string ToString()
    {
        return string.Format("[Id={0}, Username={1}, Level completed= {2}]", Id, Username, LevelCompleted);
    }
    public int GetCompletedLevel()
    {
        return LevelCompleted;
    }
    public void UpdateProgressLevel(int level_completed)
    {
        Debug.Log(level_completed);
        LevelCompleted = level_completed;
    }
}

