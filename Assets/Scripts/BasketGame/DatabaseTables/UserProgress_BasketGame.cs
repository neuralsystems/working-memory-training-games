﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
public class UserProgress_BasketGame  {

	[PrimaryKey, AutoIncrement]
    public int Id { set; get; }
    public string User_Obj { get; set; }
    public int Level_Obj { get; set; }

    int default_level = 1;
    //public Set UserProgress_BasketGame(string username)
    //{
    //    this.User_Obj = username;
    //    this.Level_Obj = default_level;
    //}
}