using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System;
[System.Serializable]
public class UserLevel
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
	public string UserObj { get; set; }
    public int LevelObj { get; set; }
	public int Progress { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime DateModified { get; set; }
}
