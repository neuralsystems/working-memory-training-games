using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

	string outline ="outlined", withoutoutline ="withououtline";
	// Use this for initialization
	public string ImageFolder = "GameImages/";
	string ImageList;
	int categoryId;
	void Start () {
		
		string imagename = GetImage ();
		string category = GetCategory ();
		GameObject outl = GameObject.Find (outline);
		GameObject woutl = GameObject.Find (withoutoutline);
		Debug.Log (ImageFolder + category + "/" + "WithOutline/" + imagename);
		outl.GetComponent<SpriteRenderer>().sprite = Resources.Load (ImageFolder + category + "/" + "WithOutline/" + imagename, typeof(Sprite)) as Sprite;
		woutl.GetComponent<SpriteRenderer>().sprite = Resources.Load (ImageFolder + category + "/" + "WithoutOutline/" + imagename, typeof(Sprite)) as Sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	string GetCategory(){
		var ds = new CarGame_DataService (CarGame_SceneVariables.databaseName);

		var image = ds.GetRandomCategory();
		//		Debug.Log (image);
		foreach (var i in image) {
			categoryId = i.Id;
			Debug.Log ("Category id is: "+categoryId);
			return i.CategoryName;
		}
		return "Car";
	}
	string GetImage(){
		
			var ds = new CarGame_DataService (CarGame_SceneVariables.databaseName);
			var image = ds.GetnImagesFromCategory (2, categoryId);
			//			Debug.Log(image);
					Debug.Log (image);
			foreach (var i in image) {

				ImageList = i.GetImageName ();
				Debug.Log ("ImageList "+ImageList);
				return ImageList;
			}
		return "red";
			//		Debug.Log(ds.GetImages());

	}
}
