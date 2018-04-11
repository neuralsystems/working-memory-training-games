using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scalling : MonoBehaviour {


	public float maxSize = 1f;
	public float minSize = .9f;
	float growFactor = .3f;
	public float waitTime = .1f;
	public bool shouldScale = false;
	public Vector3 original_scale;
	void Start()
	{
		original_scale = transform.localScale;
		transform.localScale = new Vector3 (1, 1, 1) * maxSize;
		minSize = .5f * maxSize;
//		growFactor = .5f * (maxSize - minSize);
		if (shouldScale) {
			StartCoroutine (Scale ());
		}
	}


	IEnumerator Scale()
	{
		float timer = 0;


			// we scale all axis, so they will have the same value, 
			// so we can work with a float instead of comparing vectors

			while(maxSize > transform.localScale.x)
			{
//				timer += Time.deltaTime;
				transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
				yield return null;
			}
			// reset the timer

//			yield return new WaitForSeconds(waitTime);

			timer = 0;
			while(minSize < transform.localScale.x)
			{
//				timer += Time.deltaTime;
				transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
				yield return null;
			}

			timer = 0;
			yield return new WaitForSeconds(waitTime);
		if (shouldScale) {
			StartCoroutine (Scale ());
		} else {
			transform.localScale = new Vector3(1,1,1) * maxSize;
		}
	}


	public void SetScale(bool value){
//		StopCoroutine(Scale());
		transform.localScale = new Vector3(1,1,1) * maxSize;
		shouldScale = value;
		if (shouldScale) {
			StartCoroutine (Scale ());
		}
	}
}
