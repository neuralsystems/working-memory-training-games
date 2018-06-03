using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BasketGame_PreGameManager : MonoBehaviour {

    public GameObject simpleGameObjectPool;
	List<Sprite> all_baloons = new List<Sprite>();
    int tapCount = 0;
    int tap_threshold = 5;
    int level_number = 0;
    int max_levels = 7;
    bool should_move = false;
    int movement_Speed = 4;
    public string MainGameSceneName;
    bool[] Movement_choice = new bool[] { false,false,false,true,true,true,true};
    int[] Speed_choices = new int[] {0,0,0,1,2,4,5};
    // Use this for initialization
    void Start () {
        var folder = BasketGame_SceneVariables.Game_Name + "/" + "Pre/Baloons";
        var all_sprites = Resources.LoadAll(folder, typeof(Sprite));
        foreach(var _sprite in all_sprites)
        {
            all_baloons.Add(_sprite as Sprite);
        }
        SpawnBaloon();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnBaloon()
    {
        if (level_number < max_levels)
        {
            var baloon_go = simpleGameObjectPool.GetComponent<SimpleObjectPool>().GetObject();
            Debug.Log(all_baloons.Count);
            baloon_go.transform.position = Shared_ScriptForGeneralFunctions.GetRandomPointOnScreen();
            should_move = Movement_choice[level_number];
            movement_Speed = Speed_choices[level_number];
            var path = "Zigzagpath";
            if (should_move)
            {
                baloon_go.GetComponent<BasketGame_PreBaloonScript>().Move(movement_Speed, path);
            }
            int n = Random.Range(0, all_baloons.Count);
            baloon_go.GetComponent<SpriteRenderer>().sprite = all_baloons[n];
        }
        else
        {
            SceneManager.LoadScene(MainGameSceneName);
        }
    }

    public void Next()
    {
        IncreaseLevel();
        SpawnBaloon();
    }

    public void ReduceLevel()
    {
        level_number--;
    }

    public void IncreaseLevel()
    {
        level_number++;
        
    }
}
