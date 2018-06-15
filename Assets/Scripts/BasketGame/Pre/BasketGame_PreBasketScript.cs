using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketGame_PreBasketScript : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("called collision ");
        if (collision.gameObject.tag == BasketGame_SceneVariables.baloonTag)
        {
            Destroy(collision.gameObject);
            Camera.main.GetComponent<BasketGame_PreGameManager>().Next();
        }
    }
}
