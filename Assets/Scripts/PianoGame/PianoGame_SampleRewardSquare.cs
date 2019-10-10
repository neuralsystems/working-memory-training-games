using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PianoGame_SampleRewardSquare : MonoBehaviour {

	public Image note;

	private Reward reward_go;
	private PG_RewardSquareScrollList scrollList;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetUp(Reward reward_object, PG_RewardSquareScrollList current_scrolllist){
		reward_go = reward_object;
		scrollList = current_scrolllist;
	}
}
