using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketGame_PreBasketScript : MonoBehaviour {

    public GameObject sound_manager;
    int currentIns = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(OnCollision(collision));
    }

    IEnumerator OnCollision(Collision2D collision)
    {
        Debug.Log("called collision ");
        if (collision.gameObject.tag == BasketGame_SceneVariables.baloonTag)
        {
            var ch = collision.gameObject.transform.GetChild(0);
            ch.transform.parent = null;
            Destroy(collision.gameObject);
           
            currentIns %= 10;
            ch.transform.position = transform.GetChild(currentIns).transform.position;
            ch.transform.parent = transform.GetChild(currentIns).transform;
            currentIns++;
            ch.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            Destroy(collision.gameObject);
            GetComponent<ParticleSystem>().Play();
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
