using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TrainGame_PreGameManager : MonoBehaviour {

	public GameObject SampleShape;
	// Use this for initialization
	const string Folder_location = TrainGame_SceneVariables.Game_Name + "/Pre/";
//	string shape_name;
	string[] shapes = new string[]{"Shape_5", "Shape_1","Shape_2", "Shape_3", "Shape_4"};
	int first_level = 0;
	int consecutive_correct =0, threshold = 5;
	public string mainScene;
	void Start () {
		SetShape ();
		
	}

	public void IncrementOnCorrectMatch(){
		consecutive_correct++;
	}

	public bool HasReachedThreshold(){
		return consecutive_correct >= threshold;
	}

	public void ResetCorrect(){
		consecutive_correct = 0;
	}
	public void SetShape(){
//		int total_shapes = shapes.Length;
//		int x = Random.Range (0, total_shapes);
		var shape_name = shapes [first_level];
		SampleShape.GetComponent<SpriteRenderer> ().sprite = Resources.Load (Folder_location + "Shapes/" +shape_name, typeof(Sprite)) as Sprite;
		SetOptions (shape_name);
		first_level++;
		first_level %= shapes.Length;
	}

	public void LoadNext(){
		if (HasReachedThreshold()) {
			SceneManager.LoadScene (mainScene);
		} else {
			SetShape ();
		}
	}

	void SetOptions(string shape_name){
		Debug.Log ("sneaked under: " + Folder_location + shape_name + "/");
		var all_objects = Resources.LoadAll (Folder_location + shape_name + "/", typeof(Sprite));
//		
		int all_sprites_len = all_objects.Length;
		Sprite[] all_sprites =  new Sprite[all_sprites_len];
		for (int j =0;j< all_sprites_len;j++ ) {
			all_sprites[j] = all_objects[j] as Sprite;
			Debug.Log (all_sprites [j].name + " "+ all_sprites_len);
		}
		var counter_shape_sprites = RandomizingArray.RandomizeSprite (all_sprites).ToArray ();
//		Debug.Log ("sneaked under: " counter_shape_sprites.coun);
//		counter_shape_sprites = RandomizingArray.RandomizeSprite (counter_shape_sprites).ToArray();
		var counter_shape_go = GameObject.FindGameObjectsWithTag (TrainGame_SceneVariables.COUNTER_SHAPE_OPTION_TAG);
		int i = 0;
		foreach (var counter_shape in counter_shape_go) {
			counter_shape.GetComponent<TrainGame_CounterShapeScript> ().SetUp(counter_shape_sprites [i] as Sprite);
			Debug.Log (counter_shape_sprites [i]);
			i++;

		}
	}


}
