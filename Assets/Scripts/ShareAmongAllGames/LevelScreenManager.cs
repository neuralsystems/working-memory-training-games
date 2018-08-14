using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelScreenManager : MonoBehaviour
{

    int previousLevel = -1, currentLevel;
    public GameObject levelIconParent, playerIcon;
    public string game_name;
    public Sprite level_complete_icon, level_icon;
    LevelScreenManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(ShowTransition());
    }


    public IEnumerator ShowTransition()
    {
        Debug.Log("called transition");
        yield return null;
        var master_go = GameObject.Find(Shared_Scenevariables.masterGO);
        var should_rotate = false;
        if (game_name == BasketGame_SceneVariables.Game_Name)
        {
            currentLevel = master_go.GetComponent<Shared_PersistentScript>().GetNewBasketGameLevelDetails().LevelNumber;
        }
        else if (game_name == SceneVariables.Game_Name)
        {
            currentLevel = master_go.GetComponent<Shared_PersistentScript>().GetNewPianoGameLevelDetails().LevelNumber;
        }
        else if (game_name == TrainGame_SceneVariables.Game_Name)
        {
            should_rotate = true;
            currentLevel = master_go.GetComponent<Shared_PersistentScript>().GetNewTrainGameLevelDetails().LevelNumber;
        }
        else
        {
            Debug.Log("No such game as " + game_name);
        }


        //if (previousLevel == -1)
        //{
        //    playerIcon.transform.position = destination;
        //}
        //else
        //{
        playerIcon.GetComponent<PlayerIconManager>().SetTouch(false);
        //playerIcon.GetComponent<Scalling>().SetScaleForLevelScreen(false);
        if (previousLevel == -1)
        {
            //playerIcon.transform.position = levelIconParent.transform.GetChild(currentLevel - 1).transform.position;
            //var previous_level = Mathf.Max(currentLevel - 2, 0);
            //if (rotate)
            //{
            //    //LookAtGameObject(levelIconParent.transform.GetChild(currentLevel - 1).transform);
            //    playerIcon.GetComponent<PlayerIconManager>().LookAtGameObject(levelIconParent.transform.GetChild(previous_level - 1).transform)
            //}
            previousLevel = Mathf.Max(1, currentLevel - 1);
        }
        //playerIcon.GetComponent<PlayerIconManager>().SetTouch(false);
        //else
        //{
        for(int j = 1; j <= previousLevel; j++)
        {
            levelIconParent.transform.GetChild(j - 1).transform.gameObject.GetComponent<Image>().sprite = level_complete_icon;
        }
        var val = previousLevel <= currentLevel ? 1 : -1;
        Debug.Log("previous level n current level: " + previousLevel + " " + currentLevel);
        int i = previousLevel;
        playerIcon.transform.position = levelIconParent.transform.GetChild(previousLevel - 1).transform.position;
        while (i != (currentLevel + val))
        {
            Debug.Log("called transition to " + levelIconParent.transform.GetChild(i - 1).name);
            var destination = levelIconParent.transform.GetChild(i - 1).transform;
            yield return StartCoroutine(playerIcon.GetComponent<PlayerIconManager>().Transition(destination, should_rotate));
            if( val > 0)
            {
                destination.gameObject.GetComponent<Image>().sprite = level_complete_icon;
            }
            else
            {
                destination.gameObject.GetComponent<Image>().sprite = level_icon;
            }
            
            i += val;
        }
        //}
        levelIconParent.transform.GetChild(currentLevel - 1).transform.gameObject.GetComponent<Image>().sprite = level_icon;
        previousLevel = currentLevel;
        playerIcon.GetComponent<PlayerIconManager>().SetTouch(true);
        //previousLevel = currentLevel;
        //}

    }


}
