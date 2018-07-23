using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketGame_PreBaloonScript : MonoBehaviour {

    bool ShouldStop = false;
    bool isClickable = false;
    // Use this for initialization
	void Start () {
        AddFruit();
        StartCoroutine(OutOfBoundError());
    }
	
	
    public void Move(int path_speed, string fruitPath, bool should_stop)
    {
        //transform.position = iTweenPath.GetPath(fruitPath).
        isClickable = GetComponent<BasketGame_DetectTouch>().shouldTouch;
        ShouldStop = should_stop;
        transform.position = Camera.main.GetComponent<iTweenPath>().nodes[0];
       iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(fruitPath), "speed", path_speed, "easetype", "linear", "oncomplete", "OnCompletingMotion"));
    }

    void OnCompletingMotion()
    {
        Camera.main.GetComponent<BasketGame_PreGameManager>().Previous();
        Destroy(gameObject);
    }


    public void ConvertToFruit()
    {
        //gameObject.AddComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite; 
    }

    void AddFruit()
    {
        var basket_obj = GameObject.Find("Basket");
        var fruitName = Camera.main.GetComponent<BasketGame_SceneVariables>().GetColoredFruit(basket_obj.GetComponent<SpriteRenderer>().sprite.name);
        Debug.Log("fruitName = " + fruitName);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load(BasketGame_SceneVariables.Game_Name + "/" + Camera.main.GetComponent<BasketGame_SceneVariables>().GetFruitFolderName() + fruitName, typeof(Sprite)) as Sprite;
    }

    IEnumerator OutOfBoundError()
    {
        var lower_limit = Shared_ScriptForGeneralFunctions.GetPointOnScreen(-.1f, -.1f);
        if (transform.position.x < lower_limit.x || transform.position.y < lower_limit.y)
        {
            OnCompletingMotion();
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(OutOfBoundError());
    }
    
    public IEnumerator CheckForStop()
    {
        if (ShouldStop)
        {
            var basket_obj = GameObject.Find("Basket");
            while (transform.position.x < basket_obj.transform.position.x)
            {
                yield return null;
            }
            iTween.Stop(gameObject);
        }
    }

    public void CheckForTouch()
    {
        if (!isClickable)
        {
            GetComponent<BasketGame_DetectTouch>().Flip();
        }
    }
}
