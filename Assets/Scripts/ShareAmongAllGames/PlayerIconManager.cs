using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerIconManager : MonoBehaviour {

    Vector3 velocity = Vector3.zero;
    float smoothTime = .5f;
    public Canvas levelcanvas;

    public IEnumerator Transition(Transform destination, bool rotate)
    {
        Debug.Log("called to move icon to " + destination);
        //var look_for = transform.position;
        //look_for.z = destination.position.z;
        if (rotate)
        {
            Vector3 diff = destination.position - transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
        while (Vector3.Distance(transform.position, destination.position) > 0.5f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, destination.position, ref velocity, smoothTime);
            //transform.LookAt(look_for);
            yield return null;
        }
        transform.position = destination.position;
    }


    public void OnMouseDown( int gameCode)
    {
        levelcanvas.gameObject.SetActive(false);
        Debug.Log("clicked the player icon");
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
        //GetComponent<Scalling>().SetScale(val);
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
        Camera.main.GetComponent<TrainGame_GameManager>().StartGame();
    }
}
