using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShapeMatch_mainLevelStart : MonoBehaviour
{
    public static int level_number;
    public static string username00;
    
    public void OnMouseDown()
    {
        //Camera.main.GetComponent<ShapeMatch_LevelScreen>().ChangeColor(Color.white);
        username00 = Camera.main.GetComponent<ShapeMatch_LevelScreen>().username02;
        level_number = Camera.main.GetComponent<ShapeMatch_LevelScreen>().LevelNumber;
        NextLevel();
    }
    void NextLevel()
    {
        Camera.main.GetComponent<ShapeMatch_LevelScreen>().temp_GO.GetComponent<Scalling>().SetScaleForLevelScreen(false);
        Camera.main.GetComponent<ShapeMatch_LevelScreen>().canvas.gameObject.SetActive(false);
        Camera.main.GetComponent<MainScript>().StartMainLevel();
    }
}
