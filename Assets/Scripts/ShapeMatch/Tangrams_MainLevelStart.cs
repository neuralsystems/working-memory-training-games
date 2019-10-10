using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tangrams_MainLevelStart : MonoBehaviour
{
    public static int level_number;
    public static string username00;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void OnMouseDown()
    {
        username00 = Camera.main.GetComponent<ShapeMatch_LevelScreen>().username02;
        level_number = Camera.main.GetComponent<ShapeMatch_LevelScreen>().LevelNumber;
        NextLevel();
    }
    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
