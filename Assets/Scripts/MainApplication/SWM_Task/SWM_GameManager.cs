using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SWM_GameManager : MonoBehaviour {

    public int number_of_blocks = 3;
    public bool shouldRandomize = false;
    private List<float> x_position_list = new List<float>();
    private List<float> y_position_list = new List<float>();
    public  GameObject block_parent, tower;
    public int _with_in_search_error, _between_search_error;
    public Transform _flower;
    System.Random rand;
    int numSpawn = 0;
    public GameObject previous_gameobject;
    List<int> token_order = new List<int>();

    // Use this for initialization
    void Start() {
        //rand = new System.Random();
        var _temp = new List<int>();
        for (int i = 0; i < number_of_blocks; i++)
        {
            _temp.Add(i);
        }
        if (shouldRandomize)
        {
            token_order = RandomizingArray.RandomizeInt(_temp);
        }
        else
        {
            token_order = _temp;
        }
        SetScene();
        ResetErrorValues(0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void GetAllPosition()
    {
        var screenHeight = Screen.height;
        var screenWidth = Screen.width;
        foreach(Transform block_box in block_parent.transform)
        {
            var _pixels = Camera.main.WorldToScreenPoint(block_box.position);
            //Debug.Log((_pixels.x/screenWidth ) + " " + (_pixels.y/screenHeight)+ " " + block_box.gameObject.name);
        }
    }


    public void SetScene()
    {
        //GetAllPosition();
        if (numSpawn < number_of_blocks)
        {
            ResetErrorValues(_with_in_search_error, _between_search_error);
            for (int i = 0; i < number_of_blocks; i++)
            {
                //block_parent.transform.GetChild(i).GetComponent<SWM_BlockScript>().SetVisible(true);
                block_parent.transform.GetChild(i).GetComponent<SWM_BlockScript>().Reset();
                tower.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            }
            //string token = "Flower";
            var flower_go = Instantiate(_flower, new Vector3(0, 0, 0), Quaternion.identity);
            rand = new System.Random();
            var child_number = token_order[numSpawn];
            Debug.Log("CN: " + child_number);
            flower_go.transform.position = block_parent.transform.GetChild(child_number).transform.position;
            flower_go.transform.parent = block_parent.transform.GetChild(child_number).transform;
            block_parent.transform.GetChild(child_number).GetComponent<SWM_BlockScript>().SetTokenBool(true);
            numSpawn++;
        }
        else
        {
            for (int i = 0; i < number_of_blocks; i++)
            {
                //block_parent.transform.GetChild(i).GetComponent<SWM_BlockScript>().SetVisible(false);
                block_parent.transform.GetChild(i).GetComponent<SWM_BlockScript>().Reset();
                tower.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            }
            MApp_DataServices ds = new MApp_DataServices(MApp_UserInforFormScript.database_Name);
            var persistent_go = GameObject.Find(Shared_Scenevariables.masterGO);
            var user = persistent_go.GetComponent<Shared_PersistentScript>().GetCurrentPlayer();
            var _timeoftest = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ds.RegisterSWMScores(user.Username, _with_in_search_error, _between_search_error, _timeoftest, number_of_blocks);
            Debug.Log("Test Completed Scores: " + _with_in_search_error + "(Withh in search errors), " + _between_search_error + " (Between search error count)");
        }
    }

    public void WithInSearchError()
    {
        _with_in_search_error++;
    }

    public void BetweenSearchError()
    {
        _between_search_error++;
    }

    public void ResetErrorValues(int _within, int _between)
    {
        _with_in_search_error = _within;
        _between_search_error = _between;
    }

    public void TrashFlower(GameObject flower)
    {
        var target = tower.GetComponent<SWM_TowerScript>().GetNextChild().gameObject;
        StartCoroutine(flower.GetComponent<SWM_FlowerScript>().MoveToTarget(target));
    }

}
