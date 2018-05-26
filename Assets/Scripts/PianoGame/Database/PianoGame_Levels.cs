using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
public class PianoGame_Levels : MonoBehaviour {


	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string LevelName { get; set; }
	public int InitialLength { get; set; }
	public int Gradient { get; set; }
	public int Threshold { get; set; }

	public override string ToString ()
	{
		return string.Format ("[Level: Id={0}, Name={1}, InitialLength={2}, Gradient={3}, Threshold={4}  ]", Id, LevelName, InitialLength, Gradient, Threshold);
	}

	public string GetLevelName(){
		return this.LevelName;
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

}
