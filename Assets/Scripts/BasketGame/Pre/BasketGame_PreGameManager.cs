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
    int max_levels = 10;
    bool should_move = false;
    int movement_Speed = 4;
    public string MainGameSceneName;
    bool[] Movement_choice = new bool[] { false,false,false,true,true,true,true,true,true,true};
    int[] Speed_choices = new int[] {0,0,0,2,4,5,8,10,12,15};
    bool[] shouldStop = new bool[] { false, false, false, true, true, true, true, false, false, false};
    public GameObject basket_go;
    // Use this for initialization
    void Start () {
        
        max_levels = Movement_choice.Length;
        var folder = BasketGame_SceneVariables.Game_Name + "/" + "Pre/Baloons";
        var all_sprites = Resources.LoadAll(folder, typeof(Sprite));
        foreach(var _sprite in all_sprites)
        {
            all_baloons.Add(_sprite as Sprite);
        }
        basket_go.GetComponent<SpriteRenderer>().sprite = Resources.Load(BasketGame_SceneVariables.Game_Name + "/" + Camera.main.GetComponent<BasketGame_SceneVariables>().GetBasketFolderName() + BasketGame_SceneVariables.GetBaskets(1)[0], typeof(Sprite)) as Sprite;
        SpawnBaloon();

    }
	
	

    void SpawnBaloon()
    {
        
        if (level_number < max_levels)
        {
            var baloon_go = simpleGameObjectPool.GetComponent<SimpleObjectPool>().GetObject();
            Debug.Log(all_baloons.Count);
            var target = basket_go.transform.position;
            baloon_go.transform.position = new Vector3 (target.x,Shared_ScriptForGeneralFunctions.GetNPointsAtHeight(.8f, 1)[0].y,target.z);
            should_move = Movement_choice[level_number];
            movement_Speed = Speed_choices[level_number];
            var path = GetComponent<iTweenPath>().pathName;
            if (should_move)
            {
                baloon_go.GetComponent<BasketGame_PreBaloonScript>().Move(movement_Speed, path);
            }
            int n = Random.Range(0, all_baloons.Count);
            baloon_go.GetComponent<SpriteRenderer>().sprite = all_baloons[n];
        }
        else
        {
            var m_obj = GameObject.Find(BasketGame_SceneVariables.masterGO);
            var current_user = m_obj.GetComponent<Shared_PersistentScript>().GetCurrentPlayer();
            var ds = new BasketGame_DataService(BasketGame_SceneVariables.DATABASE_NAME);
            ds.MarkPreLevelCompleted(current_user.Username);
            SceneManager.LoadScene(MainGameSceneName);
        }
    }

    public void Next()
    {
        IncreaseLevel();
        SpawnBaloon();
    }

    public void Previous()
    {
        ReduceLevel();
        SpawnBaloon();
    }

    public void ReduceLevel()
    {
        level_number = Mathf.Max(0, level_number - 1);
    }

    public void IncreaseLevel()
    {
        level_number = Mathf.Min(level_number + 1, max_levels);

    }
}
