using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tapDetect : MonoBehaviour
{
    public void OnMouseDown()
    {
        float scale_size = Scale_size();
        this.transform.localScale = new Vector3(scale_size, scale_size, 1);
        int GO= Camera.main.GetComponent<MainScript>().GameObjectsInLevel;          // getting no. of game objects
        Debug.Log("in tap script  "+GO);
        this.GetComponent<SpriteRenderer>().color = Color.yellow;                    // changing the color on tap
        this.GetComponent<Scalling>().SetScale(false, GO);                          // setting the pumping off
        Debug.Log("touched  " + gameObject.transform.name);                         // printing name of the touched object
         tag = "TargetObject";                                                      // changing the tag of the touched object
         GetComponent<Collider2D>().enabled = false;                                // disabling the collider of the touched object
          
        GameObject[] gameObjectarray = GameObject.FindGameObjectsWithTag("Object"); // making an array og objects having tag "object"
        foreach (GameObject go in gameObjectarray)
        {
            go.GetComponent<Collider2D>().enabled = false;                          // disabling the collider of the game objects
            go.GetComponentInChildren<BoxCollider2D>().enabled = true;              // enabling the collider of the game bjects

            if (Camera.main.GetComponent<MainScript>().ShouldHide == 1|| Camera.main.GetComponent<MainScript>().ShouldHide == 2)  // checking for shouldHide value for that level
            {
                go.GetComponent<Scalling>().SetScale(false, GO);                        // if hidden the turning of the pumping for the gameobjects and turning hon for the cover
                go.GetComponentInChildren<Scalling>().SetScale(true, GO);
                Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    r.enabled = true;                                                   // enabling the cover renderer
                }

            }
        }
            
    }
    float Scale_size()
    {
        int GO = Camera.main.GetComponent<MainScript>().GameObjectsInLevel;
        float maxSize;
        if (GO <= 3)
        {
            maxSize = 5 - 0.7f * GO;
        }
        else if (GO > 3 && GO <= 5)
        {
            maxSize = 5 - 0.6f * GO;
        }
        else
        {
            maxSize = 5 - 0.5f * GO;
        }
        return maxSize;
    }


}
