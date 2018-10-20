using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour {

	private float fadePerSecond = 2.5f;
	private bool fadeIn;
	private bool fadeOut;
    void SetValues()
    {
        fadePerSecond = Database.constants_fadePerSecond;

    }

	public void StartFadeIn()
	{
		SetValues();
		StartCoroutine(Fade("In"));
	}

	public void StartFadeOut()
    {
        SetValues();
		StartCoroutine(Fade("Out"));
    }

	IEnumerator Fade(string str)
	{
		var material = GetComponent<Renderer>().material;

		if (str == "In")
		{
			while (material.color.a <= 1f)
			{
				material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a + (fadePerSecond * Time.deltaTime));
				yield return null;
			}
		}
		else if(str == "Out")
		{
			while (material.color.a >= 0f)
			{
				material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a - (fadePerSecond * Time.deltaTime));
				yield return null;
			}
		}
	}
 
}
