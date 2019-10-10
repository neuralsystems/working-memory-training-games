using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class ShapeMatch_levels
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int LevelNumber { get; set; }
    public int IsHide { get; set;}
    public int IsMove { get; set; }
    public int GameObjects { get; set; }
    //public int Id { get; set; }


    public override string ToString()
    {
        return string.Format("[LevelNumber={0},   GameObjects={1},   IsHide= {2},   IsMove={3} ]",LevelNumber, GameObjects, IsHide, IsMove);
    }
    public int GetgameObjects()
    {
        return GameObjects;
    }
    public int GetIsMove()
    {
        return IsMove;
    }
    public int GetIsHide()
    {
        return IsHide;
    }
}
