using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
public class Level : MonoBehaviour {


	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string LevelName { get; set; }


	public override string ToString ()
	{
		return string.Format ("[Level: Id={0}, Name={1} ]", Id, LevelName);
	}

	public string GetLevelName(){
		return this.LevelName;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
