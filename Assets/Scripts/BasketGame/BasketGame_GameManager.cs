using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BasketGame_GameManager : MonoBehaviour {

	public Transform fruitprefab, alignedBasket, bubbleprefab;
	public bool replaceBasket = false;
	Vector3 positionForNewBasket;
	public ParticleSystem rain_particlesystem_object;
	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
//		if (replaceBasket) {
//			GameObject go = GameObject.FindGameObjectWithTag (Scenevariables.fullBasket);
//			positionForNewBasket = go.transform.position;
//			GameObject ab = Instantiate (alignedBasket, go.transform.position, Quaternion.identity).gameObject;
//			ab.GetComponent<SpriteRenderer> ().sprite = go.GetComponent<SpriteRenderer> ().sprite;
//		}
	}

	public IEnumerator MakeFruit(){
//		yield return new WaitForSeconds (2f);
		Debug.Log("called me?");
		var fruits = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.fruitTag);
//		var fallingfruits = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables);
//		var offset = 1f;
		if (fruits.Length < 1 ) {
//			var spawn_position = Shared_ScriptForGeneralFunctions.GetPointOnScreen (.2f, 0f);
//			var basket_gameobject = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag) [0];
//			spawn_position.y = basket_gameobject.transform.position.y+ basket_gameobject.GetComponent<SpriteRenderer>().bounds.size.y + offset;
//			Instantiate (fruit,spawn_position , Quaternion.identity);
//		}
			Debug.Log("called me to fall ");
			var random_fruit_array = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.hangingFruitTag);
			if (random_fruit_array.Length > 0) {
				int x = Random.Range (0, random_fruit_array.Length);
				var random_fruit = random_fruit_array [x];
				random_fruit.tag = BasketGame_SceneVariables.fruitTag;
				random_fruit.GetComponent<FruitBehavior> ().enabled = true;
				var init_position = GameObject.Find ("BubbleBurst").transform.position;
				var bubble_gameobject = Instantiate (bubbleprefab, init_position, Quaternion.identity);
				//		bubble_gameobject.GetComponent<BasketGame_BubbleBehavior> ().MoveBubble (random_fruit.transform.position);
				random_fruit.GetComponent<FruitBehavior> ().StartFall ();
				yield return null;
			} else {
				rain_particlesystem_object.Play ();
				yield return new WaitForSeconds(rain_particlesystem_object.main.duration);
			}

	}
//		StartCoroutine (MakeFruit());
//		fruitTag
	}

	// function called when a basket is completely filled.
	// if all the baskets are filled Show some animation
	public void BasketFilled(){
		Debug.Log (GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag).Length);
		if (GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag).Length == 0) {
			GameObject[] filledBaskets = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.fullBasket);
			foreach (GameObject g in filledBaskets) {
				StartCoroutine(g.GetComponent<BasketBehavior> ().BaksetAnimation ());
			}
			GameObject bubble_rain_particle_system = GameObject.FindGameObjectWithTag(GetComponent<BasketGame_SceneVariables> ().RAIN_PARTICLE_SYSTEM_TAG);
			bubble_rain_particle_system.GetComponent<ParticleSystem> ().Play ();
		} 
//		else {
//			StartCoroutine(MakeFruit ());
//		}
	}


	public void FinalAnimation(){
		GameObject[] filledBaskets = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.fullBasket);
		foreach (GameObject g in filledBaskets) {
			StartCoroutine(g.GetComponent<BasketBehavior> ().BaksetAnimation ());
		}
		
	}

	public void HangFruitOnTree(){
		Debug.Log ("yo wass up?");
		float max_height_percentage = .98f, min_height_percentage = 0.5f;
		float max_width_percentage = .25f, min_width_percentage = 0.10f;
		var all_baskets = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag);
		foreach (var basket in all_baskets) {
			var basket_capacity = basket.GetComponent<BasketBehavior> ().capacity;
			for (int i = 0; i < basket_capacity; i++) {
				var height_percentage = Random.Range (min_height_percentage, max_height_percentage);
				var width_percentage = Random.Range (min_width_percentage, max_width_percentage);
				var spawn_position = Shared_ScriptForGeneralFunctions.GetPointOnScreen (width_percentage, height_percentage);
				var fruit_gameobject = Instantiate (fruitprefab,spawn_position , Quaternion.identity);
				var fruitName = Camera.main.GetComponent<BasketGame_SceneVariables>().GetColoredFruit (basket.GetComponent<BasketBehavior>().GetBasketName());
				fruit_gameobject.GetComponent<SpriteRenderer> ().sprite = Resources.Load (BasketGame_SceneVariables.Game_Name+ "/" + Camera.main.GetComponent<BasketGame_SceneVariables>().GetFruitFolderName () + fruitName, typeof(Sprite)) as Sprite;
				fruit_gameobject.GetComponent<FruitBehavior> ().fruitName = fruitName;
				fruit_gameobject.GetComponent<FruitBehavior> ().original_position = spawn_position;
			}
		}
		StartCoroutine(MakeFruit());
	}

}
