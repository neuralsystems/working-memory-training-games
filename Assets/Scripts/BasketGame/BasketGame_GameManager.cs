using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BasketGame_GameManager : MonoBehaviour {

	public Transform fruit, alignedBasket;
	public bool replaceBasket = false;
	Vector3 positionForNewBasket;

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
		var fruits = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.fruitTag).ToList();
		if (fruits.Count <= 1) {
			Instantiate (fruit, BasketGame_SceneVariables.initVector, Quaternion.identity);
		}
		yield return new WaitForSeconds (BasketGame_SceneVariables.waitTime);

//		StartCoroutine (MakeFruit());
	}

	// function called when a basket is completely filled.
	// if all the baskets are filled Show some animation
	public void BasketFilled(){
		Debug.Log (GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag).Length);
		if (GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.basketTag).Length == 0) {
			GameObject[] filledBaskets = GameObject.FindGameObjectsWithTag (BasketGame_SceneVariables.fullBasket);
			foreach (GameObject g in filledBaskets) {
				g.GetComponent<BasketBehavior> ().BaksetAnimation ();
				GameObject bubble_rain_particle_system = GameObject.FindGameObjectWithTag(GetComponent<BasketGame_SceneVariables> ().RAIN_PARTICLE_SYSTEM_TAG);
				bubble_rain_particle_system.GetComponent<ParticleSystem> ().Play ();
			}
		} else {
			StartCoroutine(MakeFruit ());
		}
	}
}
