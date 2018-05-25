using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGame_CircleMaskScript : MonoBehaviour {

	void ChangeColor(string hexcode){
		Color myColor = new Color ();
		ColorUtility.TryParseHtmlString (hexcode, out myColor);
		GetComponent<SpriteRenderer> ().color = myColor;
	}

	void ChangeVisibility(bool value){
		GetComponent<SpriteRenderer> ().enabled = value;
	}

	public void SetColorAndVisibility(string hexCode, bool Value){
		ChangeColor (hexCode);
		ChangeVisibility (Value);
	}
}
