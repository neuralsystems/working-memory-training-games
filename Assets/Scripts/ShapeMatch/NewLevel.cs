using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevel : MonoBehaviour
{
    public void New_Level(bool levelComplete)
    {
        StartCoroutine(New_level(levelComplete));                                                // calling function for starting new level
    }
    public int level; 
    IEnumerator New_level(bool levelComplete)                                                         // IEnumerator for starting new function
    {
        level= Camera.main.GetComponent<MainScript>().LevelNumber;
        yield return new WaitForSeconds(1.5f);
        int GO1 = Camera.main.GetComponent<MainScript>().GameObjectsInLevel;
        int tap_count = Camera.main.GetComponent<MainScript>().Tap_Count;
        int difference = tap_count - GO1;
        //Debug.Log(difference);
        if (Camera.main.GetComponent<MainScript>().RepeatLevel == 0 && difference==0 && levelComplete)       // upgrading level
        {
            level += 1;
            //Debug.Log(level);
            UpdateLevel();
            Debug.Log("level upgrade");
        }
        else if (Camera.main.GetComponent<MainScript>().RepeatLevel == 0 && (difference>1 && difference<=3) && levelComplete)        // downgrading level by 1
        {
            level -= 1;
            if (level < 0)
                level = 0;
            UpdateLevel();
            Debug.Log("one level downgrade");
        }
        else if (Camera.main.GetComponent<MainScript>().RepeatLevel == 0 && difference> 3 && levelComplete)               // downgrading level by 2
        {
            level -= 2;
            if (level < 0)
                level = 0;
            UpdateLevel();
            Debug.Log("two level downgrade");
        }
        else if (Camera.main.GetComponent<MainScript>().RepeatLevel == 0 && !levelComplete)
        {
            DestroyObjectsLeft();
            level -= 1;
            if (level<0)
                level = 0;
            UpdateLevel();
            Debug.Log("one level downgrade, level not completed");
        }
        else
        {                                                                                                  // for remaining in the same level
            UpdateLevel();
            Debug.Log("level same");
        }
        Camera.main.GetComponent<MainScript>().MoveAgainList.Clear();
        Camera.main.GetComponent<MainScript>().Tap_Count = 0;
        Camera.main.GetComponent<MainScript>().Wrong_Tap_Count = 0;
        yield return new WaitForSeconds(2f);
        NextLevel();
    }
    public void UpdateLevel()                                                                               // for getting the object of the user having the last played levels
    {
        string user_name = ShapeMatch_mainLevelStart.username00;
        var ds = new ShapeMatch_DataService(MainScript.DATABASE_NAME);
        ds.UpdateUserProgress(user_name, level);
    }
    void NextLevel()
    {
        Camera.main.GetComponent<ShapeMatch_LevelScreen>().NewStart();
    }
    void DestroyObjectsLeft()
    {
        GameObject[] target = GameObject.FindGameObjectsWithTag("TargetObject");
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Object");
        foreach (var obj in target)
        {
            Destroy(obj);
        }
        foreach (var obj in objs)
        {
            Destroy(obj);
        }
    }
}
