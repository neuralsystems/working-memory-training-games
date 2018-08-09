using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerIconManager : MonoBehaviour {

    Vector3 velocity = Vector3.zero;
    float smoothTime = .5f;
    public Canvas levelcanvas;
    
    public IEnumerator Transition(Vector3 destination)
    {
        Debug.Log("called to move icon to " + destination);
        while (Vector3.Distance(transform.position, destination) > 0.5f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothTime);
            yield return null;
        }
        transform.position = destination;
    }


    public void OnMouseDown( int gameCode)
    {
        levelcanvas.gameObject.SetActive(false);
        // game code = 1,2,3 for basket game, piano game,  train game respectively
        if (gameCode == 1)
        {
            StartBasketGame();
        } else if(gameCode == 2)
        {
            StartPianoGame();
        } else if( gameCode == 3)
        {
            StartTrainGame();
        }
        else
        {
            //error 
            Debug.Log("game with code " + gameCode + "not found");
        }
    }

    public void SetTouch(bool val)
    {
        GetComponent<Button>().interactable = val;
    }

    void StartBasketGame()
    {
        
        Camera.main.GetComponent<PlaceBasket>().SetUpGame();
    }

    void StartPianoGame()
    {
        Camera.main.GetComponent<PlayTone>().StartGame();
    } 

    void StartTrainGame()
    {

    }
}
