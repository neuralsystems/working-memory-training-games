using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWM_BlockScript : MonoBehaviour {

    int TouchCount = 0;
    public bool hasToken = false;
    public bool hadToken;
    bool Touchable = true;
    bool clicked = false;
    //public GameObject _tower;
    // Use this for initialization
    void Start() {
        SetHadToken(false);
        //Reset();
        //StartCoroutine(GetComponentInChildren<SWM_CoverScript>().OpeningClosingAnimation());

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetVisible(bool val)
    {
        GetComponent<SpriteRenderer>().enabled = val;
        Touchable = val;
        
        //StartCoroutine(GetComponentInChildren<SWM_CoverScript>().OpeningClosingAnimation());

    }


    private void OnMouseDown()
    {
        if (Touchable)
        {
            TouchCount++;
            StartCoroutine(Show());
            clicked = true;


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

    public void CheckForToken(){
        if (clicked)
        {
            Debug.Log("Checking for token " + name);
            if (hadToken && (!hasToken))
            {
                Camera.main.GetComponent<SWM_GameManager>().BetweenSearchError();
            }
            else if (hasToken)
            {
                GameObject _flower = transform.GetChild(1).gameObject;
                hadToken = true;
                hasToken = false;
                Camera.main.GetComponent<SWM_GameManager>().TrashFlower(_flower);
            }
            if (TouchCount > 1)
            {
                Camera.main.GetComponent<SWM_GameManager>().WithInSearchError();
            }
        }
    }
    IEnumerator Show()
    {
        //GetComponent<SpriteRenderer>().enabled = false;
        //yield return new WaitForSeconds(0.2f);
        //GetComponent<SpriteRenderer>().enabled = true;
        yield return StartCoroutine(GetComponentInChildren<SWM_CoverScript>().OpenBox());
    }

    public void Reset()
    {
        TouchCount = 0;
        clicked = false;
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
