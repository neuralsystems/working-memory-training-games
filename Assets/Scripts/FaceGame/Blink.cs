using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
	private IEnumerator blink;

	void Start()
    {
        float blinkTime = Database.constants_blinkTime;
		blink = BlinkEffect(blinkTime);
    }

	public void StartBlink()
	{
		StartCoroutine(blink);
	}

	public void StopBlink()
	{
		StopCoroutine(blink);
		GetComponent<SpriteRenderer>().enabled = true;
	}

    IEnumerator BlinkEffect(float blinkTime)
    {
		while (true)
        {
			transform.GetComponent<SpriteRenderer>().enabled = false;
			yield return new WaitForSeconds(blinkTime);
            transform.GetComponent<SpriteRenderer>().enabled = true;
			yield return new WaitForSeconds(blinkTime);
		}
    }

}
