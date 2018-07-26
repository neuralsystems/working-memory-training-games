using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{

	public Vector3 pos;
    public Vector3 optionPos;
    public bool correctKey = false;
    public bool selectedKey = false;
    private Database database;

    void Start()
    {
		database = FindObjectOfType<Database>();
    }

    void OnMouseDown()
    {

        //FaceGame_GameManager gameManager = FindObjectOfType<FaceGame_GameManager>();
        GameObject optionBG = transform.GetChild(0).gameObject;

        //fix this option as selected option : for deleting not-selected options
        selectedKey = true;

        //set wrong or right tags for the options : all wrong options blink at end of game 
        if (!correctKey)
        {
			database.ifRight = false; //flag for noting unsuccessful game plays
            gameObject.tag = database.tagsAndNames_wrongChoice;
        }
        else
        {
            gameObject.tag = database.tagsAndNames_rightChoice;
        }

        //fade away option background
		optionBG.GetComponent<FadeAway>().StartFadeOut();

		//stop wobble effect of option
		GetComponent<WobbleEffect>().StopWobble();

        //trigger to proceed with next face component
		database.ifOptionSelected = true;

        //transition onto facebase
		gameObject.GetComponent<SmoothTransition>().enabled = true;
		gameObject.GetComponent<SmoothTransition>().SetTarget(pos, new Vector3(database.constants_faceComponentScale, database.constants_faceComponentScale)) ;

    }

    void Update()
	{

        //Replace OnMouseDown with Touch
        RaycastHit hit = new RaycastHit();
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out hit))
                {
                    hit.transform.gameObject.SendMessage("OnMouseDown");
                }
            }
        }

    }

}
