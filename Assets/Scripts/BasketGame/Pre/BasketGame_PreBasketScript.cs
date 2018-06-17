using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketGame_PreBasketScript : MonoBehaviour {

    public GameObject sound_manager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(OnCollision(collision));
    }

    IEnumerator OnCollision(Collision2D collision)
    {
        Debug.Log("called collision ");
        if (collision.gameObject.tag == BasketGame_SceneVariables.baloonTag)
        {
            Destroy(collision.gameObject);
            yield return new WaitForSeconds(sound_manager.GetComponent<SoundManager_Script>().PlayHappySound());
            Camera.main.GetComponent<BasketGame_PreGameManager>().Next();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("called OnTriggerEnter2D ");
        if (collision.tag == BasketGame_SceneVariables.baloonTag)
        {
            collision.GetComponent<Scalling>().Flip();
            collision.GetComponent<BasketGame_PreBaloonScript>().CheckForTouch();
            StartCoroutine(collision.GetComponent<BasketGame_PreBaloonScript>().CheckForStop());
        }
    }
}
