using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class PlaceBasket : MonoBehaviour {

	public Transform Basket_Prefab;
	public GameObject persistent_go;
	int capacity;
    AndroidJavaObject currentActivity;
    string toastString;
    public Text t;
    // Use this for initialization
    void Start () {
        persistent_go = GameObject.Find (BasketGame_SceneVariables.masterGO);
		var level_details = persistent_go.GetComponent<Shared_PersistentScript> ().GetNewBasketGameLevelDetails ();
		capacity = level_details.GetCapacity ();
		PlaceNBaskets (level_details.GetNumofBaskets());
		Debug.Log(capacity);
	}

    void ShowToast(string toast_msg)
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currenActivity");
        this.toastString = toast_msg;
        currentActivity.Call("runOnUiThreads", new AndroidJavaRunnable(DisplayToast));
    }

    void DisplayToast()
    {
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
        toast.Call("show");
    }
    // Update is called once per frame
    void Update () {
		
	}



	public void PlaceNBaskets(int n){
//		n += 1;
		string[] basketArray = BasketGame_SceneVariables.GetBaskets (n);
		Debug.Log ("basketArray[0] "+ basketArray.Length+ " " + n);
		float height_percentage = .00f, width_percentage = 1f, left_margin = .2f, right_margin = 0.05f;
		var percent_for_one_object = (width_percentage - left_margin - right_margin) / (n +1);
		var max_allowed_size = 1f;
		var position_for_parking_slot = Shared_ScriptForGeneralFunctions.GetPointOnScreen (width_percentage, height_percentage);
		var length_span = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width * percent_for_one_object , 0f, Camera.main.nearClipPlane)).x - Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, Camera.main.nearClipPlane)).x;
		for (int i = 0; i < n; i++) {
			var basket_gameobject = Instantiate (Basket_Prefab, position_for_parking_slot, Quaternion.identity);
//			option_tile_gameobject.transform.parent = PARKING_SLOT_PARENT_GAMEOBJECT.transform;
			basket_gameobject.GetComponent<SpriteRenderer> ().sprite = Resources.Load (BasketGame_SceneVariables.Game_Name+ "/" + Camera.main.GetComponent<BasketGame_SceneVariables>().GetBasketFolderName() + basketArray [i], typeof(Sprite)) as Sprite;;
			var basketName = basketArray [i];
			var local_scale_for_one_object = (length_span * basket_gameobject.transform.localScale.x )/ (basket_gameobject.GetComponent<SpriteRenderer>().bounds.size.x  );
			local_scale_for_one_object = Mathf.Min (local_scale_for_one_object, max_allowed_size);
			var _scale = new Vector3 (local_scale_for_one_object,local_scale_for_one_object,local_scale_for_one_object);
			var _tag = BasketGame_SceneVariables.basketTag;
			basket_gameobject.GetComponent<BasketBehavior> ().enabled = true;
//			var _position = position_for_parking_slot;
//			basket_gameobject.GetComponent<BasketBehavior> ().SetUpBasketProperties (basketName, _scale, _position, _tag);
			position_for_parking_slot.x -= basket_gameobject.GetComponent<SpriteRenderer>().sprite.bounds.size.x * _scale.x;
			if (i == 0) {
				position_for_parking_slot.y += basket_gameobject.GetComponent<SpriteRenderer> ().sprite.bounds.size.y/2;
			}
			var _position = position_for_parking_slot;
			basket_gameobject.GetComponent<BasketBehavior> ().SetUpBasketProperties (basketName, _scale, _position, _tag, capacity);
			position_for_parking_slot.x -= length_span * .1f;
		}
//		StartCoroutine(MoveObjectsOutOfParking(1));
//		StartCoroutine(Camera.main.GetComponent<BasketGame_GameManager>().MakeFruit());
		StartCoroutine(Camera.main.GetComponent<BasketGame_GameManager>().HangFruitOnTree());
	}
}
