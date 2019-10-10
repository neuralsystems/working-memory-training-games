using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMatch_GameManager : MonoBehaviour
{
    public void Run(int GO)
    {
        StartCoroutine(initiate(GO));                                                 // called when the sprites for target and touched particle matches
    }
    public IEnumerator initiate(int GO)
    {
        int GO1 = Camera.main.GetComponent<MainScript>().GameObjectsInLevel;
        if (2 * GO1 - GO == 2 && Camera.main.GetComponent<MainScript>().ShouldMove == 1)        // for randomizing the objects if IsMove is 1 and 1 object match has been done
        {
            List<Vector2> Shuffle = Camera.main.GetComponent<MainScript>().MoveAgainList;
            ShuffleList(Shuffle);
            GameObject[] gameObjectarray = GameObject.FindGameObjectsWithTag("Object");
            for (int k = 0; k < gameObjectarray.Length; k++)
            {
                Vector3 playerpos = new Vector3(Shuffle[k].x, Shuffle[k].y, 0);
                StartCoroutine(MoveAgainObject(playerpos, gameObjectarray[k]));

            }
            StartCoroutine(Show(true));                                          // showing the objects after randomizing
            yield return new WaitForSeconds(0.3f * gameObjectarray.Length);    // showing for the time is dependent upon the no. of game objects preseent
            StartCoroutine(Show(false));                                        // hidding objects again
        }
        ResetWhenMatch();                                                       // resetting the objects when match is done
    }
    void ShuffleList(List<Vector2> alpha)                                       // shuffling the position list
    {
        for (int i = 0; i < alpha.Count; i++)
        {
            Vector2 temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }
    IEnumerator MoveAgainObject(Vector3 playerpos, GameObject go)                   // for moving the game objects to a particular position
    {
        float speed = Camera.main.pixelWidth * 2f;
        var step = speed * Time.deltaTime;
        while ((Vector3.Distance(go.transform.position, playerpos)) >= 0.5)
        {
            go.transform.position = Vector3.MoveTowards(go.transform.position, playerpos, step);
            yield return null;
            if (Vector3.Distance(go.transform.position, playerpos) <= 0.5)
            {
                go.transform.position = playerpos;
            }
        }

    }
    IEnumerator Show(bool value)                                                        // for showing and hidding the gameobjects and their child; value==truee when showing
    {
        GameObject[] gameObjectarray = GameObject.FindGameObjectsWithTag("Object");
        foreach (GameObject go in gameObjectarray)
        {
            Renderer[] renderersc = go.GetComponentsInChildren<Renderer>();             // renderer of child
            Renderer[] renderers = go.GetComponents<Renderer>();                        // renderer of gameobject
            foreach (Renderer rc in renderersc)
            {
                rc.enabled = !value;
            }
            foreach (Renderer r in renderers)
            {
                r.enabled = value;
            }
        }
        yield return null;
    }
    private void ResetWhenMatch()                                                           // resetting when the target sprite matches the player sprite
    {
        int GO1 = Camera.main.GetComponent<MainScript>().GameObjectsInLevel;
        if (Camera.main.GetComponent<MainScript>().ShouldHide == 0)                         // checking if the objects are to be hidden or not
        {
            GameObject[] gameObjectarray = GameObject.FindGameObjectsWithTag("Object");    // finding the objects with tag i.e., the remaining no. of objects
            foreach (GameObject go in gameObjectarray)
            {
                go.GetComponent<Scalling>().SetScale(true, GO1);
                go.GetComponent<Collider2D>().enabled = true;
                go.GetComponentInChildren<BoxCollider2D>().enabled = false;
            }
        }
        else if (Camera.main.GetComponent<MainScript>().ShouldHide == 1)                    // when objects are to be temporarily hidden
        {
            GameObject[] gameObjectarray = GameObject.FindGameObjectsWithTag("Object");
            StartCoroutine(Show(true));
            foreach (GameObject go in gameObjectarray)
            {
                go.GetComponent<Scalling>().SetScale(true, GO1);
                go.GetComponent<Collider2D>().enabled = true;
                go.GetComponentInChildren<BoxCollider2D>().enabled = false;
            }
        }
        else if (Camera.main.GetComponent<MainScript>().ShouldHide == 2)                    // when the objects are always hidden
        {
            GameObject[] gameObjectarray1 = GameObject.FindGameObjectsWithTag("Object");
            Debug.Log(gameObjectarray1.Length);
            GameObject obj = gameObjectarray1[Random.Range(0, gameObjectarray1.Length)];      // choosing a random object to be shown to be matchd from the remaining no. of objects
            obj.tag = "TargetObject";                                                           // making that chosen object as a random object
            foreach (GameObject go in gameObjectarray1)
            {
                Renderer[] renderersc = go.GetComponentsInChildren<Renderer>();
                foreach (Renderer rc in renderersc)
                {
                    rc.enabled = false;
                }
                Renderer[] renderer = go.GetComponents<Renderer>();
                foreach (Renderer rc in renderer)
                {
                    rc.enabled = true;
                }
            }
            GameObject[] gameObjectarray = GameObject.FindGameObjectsWithTag("Object");             // showing the cover of the objecs 
            foreach (GameObject go in gameObjectarray)
            {
                Renderer[] renderersc = go.GetComponentsInChildren<Renderer>();
                foreach (Renderer rc in renderersc)
                {
                    rc.enabled = true;
                }
                go.GetComponentInChildren<Scalling>().SetScale(true, GO1);                          // for pumping the objects
                go.GetComponentInChildren<BoxCollider2D>().enabled = true;                          // for enabling the colliders
            }
        }
    }
}
