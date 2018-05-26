using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
public class PianoGame_TonesForLevels : MonoBehaviour {

	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string Tone { get; set; }
	public string ToneName { get; set; }
	public string Level { get; set; }

	public override string ToString ()
	{
		return string.Format ("[Tone Details: Id={0}, ToneName = {1},Tone={2}, Level ={3} ]", Id, ToneName ,Tone.Substring(0,10), Level);
	}

	public string GetTone(){
		return this.Tone;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
