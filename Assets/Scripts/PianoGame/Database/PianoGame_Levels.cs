using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
public class PianoGame_Levels : MonoBehaviour {


	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public int LevelNumber { get; set; }
	public int InitialLength { get; set; }
	public int Gradient { get; set; }
	public int Threshold { get; set; }
    public int KeepVisible { get; set; }
    public float PlayTime { get; set; }
    public float WaitTime { get; set; }


	public override string ToString ()
	{
		return string.Format ("[Level: Id={0}, Name={1}, InitialLength={2}, Gradient={3}, Threshold={4}  ]", Id, LevelNumber, InitialLength, Gradient, Threshold);
	}

	public int GetLevelName(){
		return this.LevelNumber;
	}

	public int GetInitialLength(){
		return this.InitialLength;
	}
	public int GetThreshold(){
		return this.Threshold;
	}
	public int GetGradient(){
		return this.Gradient;
	}

    public bool GetKeepVisible()
    {
        return this.KeepVisible == SceneVariables.DEFAULT_VALUE_KEEP_VISIBLE;
    }

    public int GetDifference()
    {
        return Mathf.Max(this.InitialLength - this.Gradient, 0);
    }
}
