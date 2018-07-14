using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapDetectOnCover : MonoBehaviour
{
    private GameObject target;
    [HideInInspector]public Vector3 targetPosition;
    private Vector3 playerPosition;
    private int GO;
    private int i, GO1;
    public void OnMouseDown()
    {
        Camera.main.GetComponent<MainScript>().Tap_Count++;                         // counting no. of taps done for matching the game object for a particular level
        GO1 = Camera.main.GetComponent<MainScript>().GameObjectsInLevel;
        target = GameObject.FindGameObjectWithTag("TargetObject");                  // this will be the target game object which should be matched
        targetPosition = target.transform.position;                                 // position of the target game object
        this.GetComponent<Renderer>().enabled = false;  //disabling the cover of the touched game object
        float scale_size = Scale_size();
        this.transform.parent.localScale = new Vector3(scale_size, scale_size, 1);
        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Object");  // making an array of game objects tagged with object to disable their colliders and pumping
        GO = gameObjectArray.Length + 1;
        foreach (GameObject go in gameObjectArray)
        {
            go.GetComponent<Scalling>().SetScale(false, GO1);
            go.GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
        playerPosition = transform.parent.position;                                     // getting the position of the touched object
        StartCoroutine(Move());                                                         // moving the touched object to target object position
    }
    private SpriteRenderer targetsprite;                                                // storing the sprite of the target object
    private SpriteRenderer playersprite;                                                // storing the sprite of teh player object 
    
    IEnumerator Move()                                                                  // fuction for moving the game object
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(backAgain(targetPosition));                                         // calling function to move
        yield return new WaitForSeconds(1.2f);
        targetsprite = target.GetComponent<SpriteRenderer>();
        playersprite = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        if (targetsprite.sprite == playersprite.sprite)                                     // comparing the sprites of both the objects
        {
            
            Camera.main.GetComponent<Audio_effects>().PlayNeutralSound();                   // playing fitting sound
            yield return new WaitForSeconds(0.4f);
            Camera.main.GetComponent<Audio_effects>().PlayInstructionSound();               // playing appreciating sounds
            Debug.Log(" both objects matches");
            ParticleSystem particle=transform.parent.gameObject.GetComponent<ParticleSystem>();
            particle.Play();                                                                    // playing attached particle system
            transform.parent.GetComponent<Renderer>().enabled = false;
            target.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            yield return new WaitForSeconds(0.1f);
            MoveToGameObjectList();                                                             // calling fn to move the game object to the box
            yield return new WaitForSeconds(2f);
            GO = GO - 2;
            if(GO==0)                                                                           // checking if gameobjects=0
            {
                Camera.main.GetComponent<Audio_effects>().PlayHappySound();
                yield return new WaitForSeconds(1f);
                Destroy(transform.parent.gameObject);
                Camera.main.GetComponent<NewLevel>().New_Level();                               // calling script for starting new level
            }
            else
            {
                Destroy(transform.parent.gameObject);
                transform.parent.tag = "TouchedObject";
                Camera.main.GetComponent<ShapeMatch_GameManager>().Run(GO);                                  // calling script for resetting when gameobjects ae remaining 
            }
        }
        else
        {
            Camera.main.GetComponent<Audio_effects>().PlaySadSound();                               // when the sprites dont match
            Debug.Log(" both objects do not match");
            StartCoroutine(backAgain(playerPosition));
            ResetWhenNotMatch();                                                                    // calling reset function when not matching
        }
    }
    IEnumerator backAgain(Vector3 playerpos)                                                        // fuction to move the game object to some position
    {
        Debug.Log("called back again");
        float speed= Camera.main.pixelWidth*0.02f;
        var step = speed * Time.deltaTime;
        while ((Vector3.Distance(transform.parent.position, playerpos)) >= 0.5)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, playerpos, step);
            yield return null;
            if (Vector3.Distance(transform.parent.position, playerpos) <=0.5)
            {
                transform.parent.position = playerpos;
            }
        }
    }
    private void ResetWhenNotMatch()                                                                // fuction for resetting when the sprites do not match
    {
        GameObject[] gameObjectarray = GameObject.FindGameObjectsWithTag("Object");
        if (Camera.main.GetComponent<MainScript>().ShouldHide == 0)                                 // when the value of should hide ==0
        {
            foreach (GameObject go in gameObjectarray)
            {
                go.GetComponent<Scalling>().SetScale(true, GO1);
                go.GetComponentInChildren<BoxCollider2D>().enabled = true;
            }
        }
        else if (Camera.main.GetComponent<MainScript>().ShouldHide == 1)                            // when the value of should hide =1 i.e, when covered
        {
            foreach (GameObject go in gameObjectarray)
            {
                Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    r.enabled = true;
                }
                go.GetComponentInChildren<Scalling>().SetScale(true, GO1);
                go.GetComponentInChildren<BoxCollider2D>().enabled = true;
            }
        }
    }
    
    void MoveToGameObjectList()
    {
        Camera.main.GetComponent<MainScript>().Box.GetComponent<Renderer>().enabled = true;
        target.GetComponent<SpriteRenderer>().color = Color.white;
        float size = Camera.main.pixelWidth;
        Vector3 screenPos = new Vector3(0, 0, 0);
        screenPos.x = 7f;
        screenPos.y = 0f;
        StartCoroutine(MoveToList(screenPos));
    }
    IEnumerator MoveToList(Vector3 screenPos)                                                                   // fuction to move the game object to the box
    {
        var step= Camera.main.pixelWidth * 0.02f * Time.deltaTime;
        while ((Vector3.Distance(target.transform.position, screenPos)) >= 0.5)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, screenPos, step);

            yield return null;
            if (Vector3.Distance(target.transform.position, screenPos) <= 0.5)
            {
                target.transform.position = screenPos;
            }
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(target);                                                                                // destroying the target game object
        Camera.main.GetComponent<MainScript>().Box.GetComponent<Renderer>().enabled = false;
    }
    float Scale_size()
    {
        int go= Camera.main.GetComponent<MainScript>().GameObjectsInLevel;
        float maxSize;
        if (go <= 3)
        {
            maxSize = 5 - 0.7f * go;
        }
        else if (go > 3 && go <= 5)
        {
            maxSize = 5 - 0.6f * go;
        }
        else
        {
            maxSize = 5 - 0.5f * go;
        }
        return maxSize;
    }
}
