using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BasketGame_PlayerIconManager : MonoBehaviour {

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

    public void OnMouseDown()
    {
        levelcanvas.gameObject.SetActive(false);
        Camera.main.GetComponent<PlaceBasket>().SetUpGame();
    }

    public void SetTouch(bool val)
    {
        GetComponent<Button>().interactable = val;
    }
}
