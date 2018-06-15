using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketGame_PreBaloonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(OutOfBoundError());
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Move(int path_speed, string fruitPath)
    {
        //transform.position = iTweenPath.GetPath(fruitPath).
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
        gameObject.AddComponent<Rigidbody2D>();
        var basket_obj = GameObject.Find("Basket");
        var fruitName = Camera.main.GetComponent<BasketGame_SceneVariables>().GetColoredFruit(basket_obj.GetComponent<SpriteRenderer>().sprite.name);
        Debug.Log("fruitName = "+ fruitName);
        GetComponent<SpriteRenderer>().sprite = Resources.Load(BasketGame_SceneVariables.Game_Name + "/" + Camera.main.GetComponent<BasketGame_SceneVariables>().GetFruitFolderName() + fruitName, typeof(Sprite)) as Sprite;

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
}
