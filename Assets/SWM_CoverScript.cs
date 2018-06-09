using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWM_CoverScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public  IEnumerator OpeningClosingAnimation()
    {
        //yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length - .1f);

        Debug.Log("Set to 1.0f");
        GetComponentInParent<SWM_BlockScript>().CheckForToken();
        GetComponent<Animator>().SetFloat("Direction", 1.0f);
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        
        //Debug.Log("Set to 0.0f");
        GetComponent<Animator>().SetFloat("Direction", 0.0f);
        //yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        transform.rotation = Quaternion.identity;
        Debug.Log(Quaternion.identity);
    }

    public IEnumerator OpenBox()
    {
        //GetComponent<Animator>().SetFloat("Direction", -1.0f);
        //yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length - .1f);
        //GetComponent<Animator>().SetFloat("Direction", 2.0f);
        //transform.rotation = Quaternion.identity;
        GetComponent<Animator>().SetFloat("Direction", -1.0f);
        //yield return StartCoroutine(OpeningClosingAnimation());
        yield return null;
    }

    public IEnumerator CloseBox()
    {
        GetComponent<Animator>().SetFloat("Direction", 1.0f);
        yield return null;
    }
}
