using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward
{
	public Image rewardNote;
}

public class PG_RewardSquareScrollList : MonoBehaviour {


	public List<Reward> rewardList;
	public Transform contentPanel;
//	public PG_RewardSquareScrollList 
	public SimpleObjectPool RewardSquareObjectPool;
	// Use this for initialization
	void Start () {
		RefreshDisplay ();
	}

	private void RefreshDisplay(){
		AddNotes ();
	}

	private void AddNotes(){
		for (int i = 0; i < rewardList.Count; i++) {
			Reward reward_object = rewardList [i];
			var newReward = RewardSquareObjectPool.GetObject ();
			newReward.transform.SetParent (contentPanel);

			PianoGame_SampleRewardSquare new_sampleRewardSquare = newReward.GetComponent<PianoGame_SampleRewardSquare> ();
			new_sampleRewardSquare.SetUp (reward_object, this);
		}
	}
		
}
