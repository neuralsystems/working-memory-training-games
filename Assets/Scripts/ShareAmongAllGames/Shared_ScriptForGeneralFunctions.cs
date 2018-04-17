using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shared_ScriptForGeneralFunctions : MonoBehaviour {

	Vector3 velocity = Vector3.zero; 
	float smoothTime = .5f, minDistance = 0.01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static Vector3 GetPointOnScreen(float width_percentage, float height_percentage){
		var screen_height = Camera.main.pixelHeight;
		var screen_width = Camera.main.pixelWidth;
		return Camera.main.ScreenToWorldPoint (new Vector3 (screen_width * width_percentage, screen_height * height_percentage, Camera.main.nearClipPlane + 100f));

	}


	public IEnumerator MoveToTarget (GameObject traveller, Vector3 target)
	{
		if (Vector3.Distance (traveller.transform.position, target) > Mathf.Min(minDistance,0.0001f)) {
			//			Debug.Log ("first if");
			traveller.transform.position = Vector3.SmoothDamp (traveller.transform.position, target, ref velocity, smoothTime);
			yield return null;
//			yield return new WaitForSeconds(1f);
			StartCoroutine (MoveToTarget (traveller,target));
		} else {
			traveller.transform.position = target;

		}


	}


	public static IEnumerator ScaleUp(GameObject go, float maxSize, float growFactor)
	{
		float timer = 0;


		// we scale all axis, so they will have the same value, 
		// so we can work with a float instead of comparing vectors
		Debug.Log("Scalling up");
		while (maxSize > go.transform.localScale.x) {
			//				timer += Time.deltaTime;
			go.transform.localScale += new Vector3 (1, 1, 1) * Time.deltaTime * growFactor;
			yield return null;
		}
	}

	public static IEnumerator ScaleDown(GameObject go, float minSize, float growFactor)
	{
		float timer = 0;


		// we scale all axis, so they will have the same value, 
		// so we can work with a float instead of comparing vectors
		Debug.Log("Scalling down");
		while (minSize < go.transform.localScale.x) {
			//				timer += Time.deltaTime;
			go.transform.localScale -= new Vector3 (1, 1, 1) * Time.deltaTime * growFactor;
			yield return null;
		}
	}
}
