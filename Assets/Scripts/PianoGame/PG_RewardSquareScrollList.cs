using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Reward
{
	public Image rewardNote;
}

public class PG_RewardSquareScrollList : MonoBehaviour {


	public List<Reward> rewardList;
	public Transform contentPanel;
//	public PG_RewardSquareScrollList 
	public SimpleObjectPool KeySquareUIPool;
	// Use this for initialization
	void OnEnable() {
        RefreshDisplay();
    }

    private void RefreshDisplay(){
		AddNotes ();
	}

	private void AddNotes(){
		for (int i = 0; i < rewardList.Count; i++) {
			Reward reward_object = rewardList [i];
			var newReward = KeySquareUIPool.GetObject ();
			newReward.transform.SetParent (contentPanel);
			PianoGame_SampleRewardSquare new_sampleRewardSquare = newReward.GetComponent<PianoGame_SampleRewardSquare> ();
			new_sampleRewardSquare.SetUp (reward_object, this);
		}
	}
		
}
