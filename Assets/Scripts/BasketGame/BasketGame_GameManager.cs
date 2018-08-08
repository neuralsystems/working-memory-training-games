using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BasketGame_GameManager : MonoBehaviour {

	public Transform fruitprefab, alignedBasket, bubbleprefab;
	public bool replaceBasket = false;
	Vector3 positionForNewBasket;
	public ParticleSystem rain_particlesystem_object;
	public GameObject Girl, persistent_go;
	int Error_Count = 0, num_of_fruits;
    public Canvas level_canvas;
    public Transform level_content;
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

	public void IncreaseErrorCount(){
		Error_Count++;
	}

	public int SetLevel(){
		var error_rate_for_increment  = .2f;
        var error_rate_for_decrement_1 = .7f;
        var error_rate_for_decrement_2 = .9f;
        int change = 0;
        // increase level by 1 if error rate is 20%, decrease level by 1 if error rate is 70% or above and by 2 if error rate is 90% or above
        if (Error_Count < num_of_fruits * error_rate_for_increment) {
			change = 1;
		} else if (Error_Count > num_of_fruits * error_rate_for_decrement_2)
        {
            change = -2;
        } else if (Error_Count > num_of_fruits * error_rate_for_decrement_1)
        {
            change = -1;
        }
		return change;
	}
	public IEnumerator MakeFruit(){
//		yield return new WaitForSeconds (2f);
//		Debug.Log("called me?");
		var fruits = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.fruitTag);
//		var fallingfruits = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables);
//		var offset = 1f;
		if (fruits.Length < 1 ) {
//			var spawn_position = Shared_ScriptForGeneralFunctions.GetPointOnScreen (.2f, 0f);
//			var basket_gameobject = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag) [0];
//			spawn_position.y = basket_gameobject.transform.position.y+ basket_gameobject.GetComponent<SpriteRenderer>().bounds.size.y + offset;
//			Instantiate (fruit,spawn_position , Quaternion.identity);
//		}
//			Debug.Log("called me to fall ");
			var random_fruit_array = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.hangingFruitTag);
			if (random_fruit_array.Length > 0) {
				int x = Random.Range (0, random_fruit_array.Length);
				var random_fruit = random_fruit_array [x];
				random_fruit.tag = BasketGame_SceneVariables.fruitTag;
				random_fruit.GetComponent<FruitBehavior> ().SetUp();
				var init_position = Girl.gameObject.transform.position;
				var bubble_gameobject = Instantiate (bubbleprefab, init_position, Quaternion.identity);
				//		bubble_gameobject.GetComponent<BasketGame_BubbleBehavior> ().MoveBubble (random_fruit.transform.position);
				random_fruit.GetComponent<FruitBehavior> ().StartFall ();
				yield return null;
			} else {
				rain_particlesystem_object.Play ();
				yield return new WaitForSeconds(rain_particlesystem_object.main.duration);
				persistent_go = GameObject.Find (Shared_Scenevariables.masterGO);
				persistent_go.GetComponent<Shared_PersistentScript>().IncreaseLevelBasketGame(SetLevel());
                //SceneManager.LoadScene (SceneManager.GetActiveScene().name);
                ChangeLevel();

            }

	}
//		StartCoroutine (MakeFruit());
//		fruitTag
	}

    void ChangeLevel()
    {
        var all_baskets = GameObject.FindGameObjectsWithTag(BasketGame_SceneVariables.fullBasket);
        foreach(var b in all_baskets)
        {
            Destroy(b);
        }
        level_canvas.gameObject.SetActive(true);
        var value_for_stand_from_eat = 6;
        var value_for_stand_from_Clap = 2;
        var value_for_pause = 6;
        var state_eat = "Eat";
        var state_clap = "Clap";
        var variable_name = "StateValue";
        if (Girl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(state_eat))
        {
            Debug.Log("State is " + state_eat);
            Girl.GetComponent<Animator>().SetInteger(variable_name, value_for_stand_from_eat);
        }
        else if (Girl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(state_clap))
        {
            Debug.Log("State is " + state_clap);
            Girl.GetComponent<Animator>().SetInteger(variable_name, value_for_stand_from_Clap);
        }
        //Girl.GetComponent<Animator>().SetInteger("StateValue", 6);
        StartCoroutine(level_content.GetComponent<LevelScreenManager>().ShowTransition());
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

	public IEnumerator HangFruitOnTree(){
//		Debug.Log ("yo wass up?");
		num_of_fruits = 0;
		float max_height_percentage = .98f, min_height_percentage = 0.5f;
		float max_width_percentage = .25f, min_width_percentage = 0.10f;
		var all_baskets = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag);
		foreach (var basket in all_baskets) {
			var basket_capacity = basket.GetComponent<BasketBehavior> ().capacity;
			num_of_fruits += basket_capacity;
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
		yield return new WaitForSeconds (3f);
		StartCoroutine(MakeFruit());
	}

	public IEnumerator PlayGirlAnimation(){
		var value_for_play = 1; 
		var value_for_pause = 2;
		Girl.GetComponent<Animator>().SetInteger("StateValue", value_for_play);
		yield return StartCoroutine(Camera.main.GetComponent<Shared_ScriptForGeneralFunctions>().GetRandomClapping());
		Girl.GetComponent<Animator>().SetInteger("StateValue", value_for_pause);
		yield return new WaitForSeconds (.5f);
		StartCoroutine (MakeFruit ());
	}

	public IEnumerator EmptyBasket(){
		yield return null;
		var value_for_play_from_Stand = 5; 
		var value_for_play_from_Clap = 4; 
		var value_for_pause = 6;
		var state_stand = "Stand";
		var state_clap = "Clap";
		var variable_name = "StateValue";
		if (Girl.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName (state_stand)) {
			Debug.Log ("State is "+ state_stand);
			Girl.GetComponent<Animator> ().SetInteger (variable_name, value_for_play_from_Stand);
		} else if (Girl.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName (state_clap)) {
			Debug.Log ("State is "+ state_clap);
			Girl.GetComponent<Animator> ().SetInteger (variable_name, value_for_play_from_Clap);
		}
		var all_fruits = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.inBasketFruitTag);
//		foreach (Transform slot in transform) {
//			foreach (Transform fruit in slot.transform) {
//				StartCoroutine(fruit.GetComponent<FruitBehavior> ().EatFruit ());
//				yield return new WaitForSeconds (.1f);
//			}
//		}
		foreach (var fruit in all_fruits) {
			StartCoroutine(fruit.GetComponent<FruitBehavior>().EatFruit());
			StartCoroutine(Shared_ScriptForGeneralFunctions.ScaleDown (fruit, .1f, 0.1f));
		}

//		while
	}
}
