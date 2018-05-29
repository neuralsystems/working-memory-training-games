using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGame_KeyLockScript : MonoBehaviour {

	public float maxSize = 1f;
	public float minSize = .9f;
	public float growFactor = .5f;
	public float waitTime = .1f;
	public Vector3 original_scale;

	// Use this for initialization
	void Start () {
		original_scale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Scale()
	{
		// we scale all axis, so they will have the same value, 
		// so we can work with a float instead of comparing vectors

		while(maxSize > transform.localScale.x)
		{
			transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
			yield return null;
		}
		while(minSize < transform.localScale.x)
		{
			transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
			yield return null;
		}

		yield return new WaitForSeconds(waitTime);
//		if (shouldScale) {
//			scaling = StartCoroutine (Scale ());
//		} else {
//			transform.localScale = new Vector3(1,1,1) * maxSize;
//		}
	}

	public void ZoomInOut(){
		Debug.Log ("Called me?");
		StartCoroutine (Scale ());
	}
}
