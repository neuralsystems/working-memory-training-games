using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class PlaceBasket : MonoBehaviour {

	public Transform Basket_Prefab;

	// Use this for initialization
	void Start () {
		var numOfBaskets = 4;
		PlaceNBaskets (numOfBaskets);

		int i = 0;
//		GameObject[] baskets = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag);
//		var isOutlined = "WithoutOutline/";
//		foreach (GameObject basket in baskets) {
//			basket.GetComponent<SpriteRenderer> ().sprite = Resources.Load (BasketGame_SceneVariables.Game_Name+ "/" + Camera.main.GetComponent<BasketGame_SceneVariables>().GetBasketFolderName() + basketArray [i], typeof(Sprite)) as Sprite;
//			basket.GetComponent<BasketBehavior> ().basketName = basketArray[i];
//			basket.GetComponent<BasketBehavior>().lowerBound -= basket.GetComponent<SpriteRenderer> ().bounds.size.y;
//			i++;
//		}


	}
		
	// Update is called once per frame
	void Update () {
		
	}



	public void PlaceNBaskets(int n){
//		n += 1;
		string[] basketArray = BasketGame_SceneVariables.GetBaskets (n);
		float height_percentage = .00f, width_percentage = 1f, left_margin = .2f, right_margin = 0.05f;
		var percent_for_one_object = (width_percentage - left_margin - right_margin) / (n +1);
		var max_allowed_size = 1f;
		var position_for_parking_slot = Shared_ScriptForGeneralFunctions.GetPointOnScreen (width_percentage, height_percentage);
		var length_span = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width * percent_for_one_object , 0f, Camera.main.nearClipPlane)).x - Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, Camera.main.nearClipPlane)).x;
		for (int i = 0; i < n; i++) {
			var basket_gameobject = Instantiate (Basket_Prefab, position_for_parking_slot, Quaternion.identity);
//			option_tile_gameobject.transform.parent = PARKING_SLOT_PARENT_GAMEOBJECT.transform;
			basket_gameobject.GetComponent<SpriteRenderer> ().sprite = Resources.Load (BasketGame_SceneVariables.Game_Name+ "/" + Camera.main.GetComponent<BasketGame_SceneVariables>().GetBasketFolderName() + basketArray [i], typeof(Sprite)) as Sprite;;
			basket_gameobject.GetComponent<BasketBehavior> ().basketName = basketArray [i];
			//			var local_scale_for_one_object = (Screen.width * percent_for_one_object * option_tile_gameobject.transform.localScale.x)/ (option_tile_gameobject.GetComponent<SpriteRenderer>().bounds.size.x * option_tile_gameobject.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
			var local_scale_for_one_object = (length_span * basket_gameobject.transform.localScale.x )/ (basket_gameobject.GetComponent<SpriteRenderer>().bounds.size.x  );
			local_scale_for_one_object = Mathf.Min (local_scale_for_one_object, max_allowed_size);
//			Debug.Log(local_scale_for_one_object + "local_scale_for_one_object");
			basket_gameobject.transform.localScale = new Vector3 (local_scale_for_one_object,local_scale_for_one_object,local_scale_for_one_object);
			basket_gameobject.tag = BasketGame_SceneVariables.basketTag;
			Debug.Log ("basket color set");
			basket_gameobject.GetComponent<BasketBehavior> ().enabled = true;
			position_for_parking_slot.x -= basket_gameobject.GetComponent<SpriteRenderer>().sprite.bounds.size.x * basket_gameobject.transform.localScale.x;
			if (i == 0) {
//				Debug.Log ("option_tile_gameobject.GetComponent<SpriteRenderer> ().sprite.bounds.size.y " + basket_gameobject.GetComponent<SpriteRenderer> ().sprite.bounds.size.y);
				position_for_parking_slot.y += basket_gameobject.GetComponent<SpriteRenderer> ().sprite.bounds.size.y/2;
			}
			basket_gameobject.transform.position = position_for_parking_slot;
			position_for_parking_slot.x -= length_span * .1f;
		}
//		StartCoroutine(MoveObjectsOutOfParking(1));
		StartCoroutine(Camera.main.GetComponent<BasketGame_GameManager>().MakeFruit());
	}
}
