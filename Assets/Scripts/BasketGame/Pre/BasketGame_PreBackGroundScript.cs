using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketGame_PreBackGroundScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == BasketGame_SceneVariables.baloonTag)
        {
            collision.GetComponent<Scalling>().Flip();
        }
    }
}
