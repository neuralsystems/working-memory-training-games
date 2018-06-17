using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scalling : MonoBehaviour {


	public float maxSize = 1f;
	public float minSize = .9f;
	public float growFactor = .5f;
	public float waitTime = .1f;
	public bool shouldScale ;
	public Vector3 original_scale;
	Coroutine scaling;
	void Start()
	{
		original_scale = transform.localScale;
		transform.localScale = new Vector3 (1, 1, 1) * maxSize;
//		minSize = .5f * maxSize;
//		growFactor = .5f * (maxSize - minSize);
		if (shouldScale) {
			scaling = StartCoroutine (Scale ());
		}
        Debug.Log("Should Scale = " + shouldScale);
	}


	IEnumerator Scale()
	{
        // we scale all axis, so they will have the same value, 
        // so we can work with a float instead of comparing vectors
        while (shouldScale)
        {
            while (maxSize > transform.localScale.x)
            {
                transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
                yield return null;
                //Debug.Log("increasing size" + transform.localScale)  ;
            }
            while (minSize < transform.localScale.x)
            {
                transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
                yield return null;
                //Debug.Log("decreasing size" + transform.localScale);
            }
            yield return new WaitForSeconds(waitTime);
        }
		//yield return new WaitForSeconds(waitTime);
		//if (shouldScale) {
		//	scaling = StartCoroutine (Scale ());
		//} else {
			transform.localScale = new Vector3(1,1,1) * maxSize;
		//}
	}


	public void SetScale(bool value){
//		StopCoroutine(Scale());

		shouldScale = value;
		if (shouldScale) {
			//StartCoroutine (Scale ());
		} else if(scaling != null && (!shouldScale)){
			Debug.Log ("Stopping to scale");
			StopCoroutine (scaling);
		}
//		transform.localScale = new Vector3(1,1,1) * maxSize;
	}

    public void Flip()
    {
        Debug.Log("Scalling flipped");
        SetScale(!shouldScale);
    }
}
