using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWM_BlockScript : MonoBehaviour {

    int TouchCount = 0;
    bool hasToken = false;
    bool hadToken;
    bool Touchable;
    // Use this for initialization
	void Start () {
        SetHadToken(false);
        //Reset();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetVisible(bool val)
    {
        GetComponent<SpriteRenderer>().enabled = val;
        Touchable = val;
    }

    private void OnMouseDown()
    {
        if (Touchable)
        {
            TouchCount++;
            StartCoroutine(Show());
            //if (TouchCount > 1)
            //{
            //    Camera.main.GetComponent<SWM_GameManager>().WithInSearchError();
            //    if (hadToken && !hasToken)
            //    {
            //        Camera.main.GetComponent<SWM_GameManager>().BetweenSearchError();
            //    }
            //}
            //else
            //{
            //    if (hasToken)
            //    {
            //        hadToken = true;
            //        transform.GetChild(0).transform.parent = null;
            //        Camera.main.GetComponent<SWM_GameManager>().SetScene();
            //    }
            //}
        }
    }

    IEnumerator Show()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void Reset()
    {
        TouchCount = 0;
        //Touchable = false;
        //SetHasToken(false);
    }

    private void SetHasToken(bool val)
    {
        hasToken = val;
    }

    private void SetHadToken(bool val)
    {
        hadToken = val;
    }

    public void SetTokenBool(bool has)
    {
        SetHasToken(has);
    }
}
