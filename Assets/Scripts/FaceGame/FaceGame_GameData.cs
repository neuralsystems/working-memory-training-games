using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

[System.Serializable] 
public class FaceGame_GameData {

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int NumOfComponents { get; set; }
    public int LevelNumber { get; set; }
    public int FaceLevel { get; set; }
    public int NumOfCompletions { get; set; }
}
