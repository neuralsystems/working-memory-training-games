using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShapeMatch_detectTapOnscollList : MonoBehaviour
{
    public static User GO;
    public static string name01;
    public string main_scene = "ShapeMatch_Scene1";
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void OnMouseDown()
    {
        GameObject button_obj = EventSystem.current.currentSelectedGameObject;
        name01 = button_obj.GetComponentInChildren<Text>().text;
        var ds = new ShapeMatch_DataService(MainScript.DATABASE_NAME);
        var Seleted_User = ds.GetPersonsWithUserName(name01);
        GO = Seleted_User;
        NextLevel();
    }
    void NextLevel()
    {
        SceneManager.LoadScene("ShapeMatch_Scene1");
    }
}
