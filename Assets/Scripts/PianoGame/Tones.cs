using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Tones : MonoBehaviour {

	// it stores all the tones in the game. Keept this array sorted on the length of the sequence as in number of notes. 
	string[] all_tones = new string[]{
		"e d e d  b a g b  e d e d  b a g a",
		"G G A G C B  G G A G D C  G G G E C B A  F F E C D C ",
		"a a a  e d e  c d f e  a a a  e d e  c d c b  a b g  a b g  a c b  a b g  a b g  c b a",
		"e e d c d  g a c f e d  e e d c d  g a c f e d  g a b c c c  c c b a a  a a g  f f e f g",

	};
	int lengthAllTones;
	string delimeter=" ";

	// Use this for initialization
	void Start () {
		lengthAllTones = all_tones.Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string GetToneAtRandom(){
		var sc = Camera.main.GetComponent<SceneVariables> ();
		DataService ds = new DataService (sc.DATABASE_NAME);
		var tones = ds.GetRandomTone();
		foreach (var tone in tones) {
			Debug.Log (tone.ToneName);
			return tone.GetTone ();
		}
		return "Do";
	}
	public string GetDelimeter(){
		return this.delimeter;
	}
}
