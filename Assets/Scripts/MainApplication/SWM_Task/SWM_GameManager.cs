using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SWM_GameManager : MonoBehaviour {

    private int number_of_blocks = 4;
    private List<float> x_position_list = new List<float>();
    private List<float> y_position_list = new List<float>();
    public  GameObject block_parent, tower;
    public int _with_in_search_error, _between_search_error;
    public Transform _flower;
    System.Random rand;
    int numSpawn = 0;
    // Use this for initialization
	void Start () {
        rand = new System.Random();
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
            numSpawn++;
            ResetErrorValues(0, _between_search_error);
            for (int i = 0; i < number_of_blocks; i++)
            {
                block_parent.transform.GetChild(i).GetComponent<SWM_BlockScript>().SetVisible(true);
                tower.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            }
            //string token = "Flower";
            var flower_go = Instantiate(_flower, new Vector3(0, 0, 0), Quaternion.identity);
            var child_number = rand.Next(0, number_of_blocks - 1);
            flower_go.transform.position = block_parent.transform.GetChild(child_number).transform.position;
            flower_go.transform.parent = block_parent.transform.GetChild(child_number).transform;
            block_parent.transform.GetChild(child_number).GetComponent<SWM_BlockScript>().SetTokenBool(true);
        }
        else
        {
            Debug.Log(_between_search_error + " " + _with_in_search_error );
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
