using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleEffect : MonoBehaviour {
	
	public IEnumerator wobble;
	private float targetScale;
	private float deltaScale;
	private float growFactor;
	private float maxScale;
	private float minScale;

	void SetValues()
	{
		targetScale = Database.constants_faceComponentScale;
        deltaScale = Database.constants_wobbleDeltaScale;
        growFactor = Database.constants_wobbleGrowFactor;
        maxScale = targetScale + deltaScale;
        minScale = targetScale - deltaScale;

	}

	public void StartWobble()
	{
		SetValues();

		wobble = Wobble(maxScale, minScale, targetScale, growFactor);
		StartCoroutine(wobble);
	}

	public void StopWobble()
	{
		StopCoroutine(wobble);
		transform.localScale = new Vector3(targetScale, targetScale);
	}

    IEnumerator Wobble(float maxScale, float minScale, float targetScale, float growFactor) {

        //Unstable effect of option when not selected
		while(true) {
			while(transform.localScale.x < maxScale)
			{
                transform.localScale += new Vector3(targetScale,targetScale) * Time.deltaTime * growFactor;
				yield return null;
			}

			while(transform.localScale.x > minScale)
			{
                transform.localScale -= new Vector3(targetScale, targetScale) * Time.deltaTime * growFactor;
				yield return null;
			}
		}
	}

}
