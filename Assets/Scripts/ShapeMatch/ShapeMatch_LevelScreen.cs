using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeMatch_LevelScreen : MonoBehaviour
{
    public Transform panel;
    public GameObject Object1;
    [HideInInspector] public int count;
    public int LevelNumber;
    [HideInInspector]public GameObject temp_GO;
    [HideInInspector]public string username02;
    [HideInInspector]public static int PreviousLevel=0;
    public Canvas canvas;
    public ScrollRect ScrollRect1;
    public const string masterGO = "MasterGameObject";
    private void Start()
    {
        canvas.gameObject.SetActive(true);
        username02 = GetuserInformation();
        Debug.Log(username02);
        GetLevelInformation(username02);
        Debug.Log(LevelNumber);
        ScrollRect1.verticalNormalizedPosition = 1 - (0.02f * LevelNumber);
        StartCoroutine(Instantiate_obj(LevelNumber));
        PreviousLevel = LevelNumber;
        ChangeColor(Color.HSVToRGB(210, 62, 77));
    }
    public void NewStart()
    {
        ScrollRect1.verticalNormalizedPosition = 1 - (0.02f * LevelNumber);
        canvas.gameObject.SetActive(true);
        username02 = GetuserInformation();
        Debug.Log(username02);
        GetLevelInformation(username02);
        //Debug.Log(LevelNumber);
        ChangeColor(Color.HSVToRGB(210, 62, 77));
        Debug.Log("previous level  " + PreviousLevel + "level   " + LevelNumber);
        StartCoroutine(MoveIcon());
    }
    
    IEnumerator MoveIcon()
    {
        yield return new WaitForSeconds(0.5f);
        if (PreviousLevel > LevelNumber)
        {
           if(PreviousLevel- LevelNumber==2)
            {
                int temp = LevelNumber + 1;
                Vector3 playerpos1 = Object1.transform.GetChild(temp).position;
                StartCoroutine(backAgain(playerpos1, temp_GO));
                Object1.transform.GetChild(PreviousLevel).GetComponent<Image>().color = Color.white;
                PreviousLevel--;
            }
            Debug.Log("previous level  " + PreviousLevel+ "level   " + LevelNumber);
            Vector3 playerpos2 = Object1.transform.GetChild(LevelNumber).position;
            yield return new WaitForSeconds(2f);
            StartCoroutine(backAgain(playerpos2, temp_GO));
            Object1.transform.GetChild(PreviousLevel).GetComponent<Image>().color = Color.white;

        }
        else
        {
            Object1.transform.GetChild(PreviousLevel).GetComponent<Image>().color = Color.HSVToRGB(210, 62, 77);
            Vector3 playerpos = Object1.transform.GetChild(LevelNumber).position;
            StartCoroutine(backAgain(playerpos, temp_GO));
            yield return new WaitForSeconds(2f);
            
        }
        PreviousLevel = LevelNumber;
        temp_GO.GetComponent<Scalling>().SetScaleForLevelScreen(true);

    }
    IEnumerator Instantiate_obj(int AtLevel)
    {
        count = Object1.transform.childCount;
        temp_GO = Object1.GetComponent<SimpleObjectPool>().GetObject();
        temp_GO.transform.SetParent(panel.transform, false);
        temp_GO.transform.position = Object1.transform.GetChild(AtLevel).position;
        yield return null;
        temp_GO.GetComponent<Scalling>().SetScaleForLevelScreen(true);
    }

    public string GetuserInformation()
    {
        var master_go = GameObject.Find(masterGO);
        //User User_info = ShapeMatch_detectTapOnscollList.GO;
        User User_info = master_go.GetComponent<Shared_PersistentScript>().GetCurrentPlayer();
        string Username = User_info.GetUserName();
        return Username;
    }

    public void GetLevelInformation(string username02)                       // for getting Details about the previously completed levels by that user
    {
        var ds = new ShapeMatch_DataService(MainScript.DATABASE_NAME);
        var Level_completed_Details = ds.GetCompletedLevel(username02);
        LevelNumber = GetLevelDetail(Level_completed_Details);

    }
    string name01;
    //private string GetUserName(User User_info)   // for getting the last played level
    //{
    //    //foreach (var level_obj in User_info)
    //    //{
    //    //    name01 = level_obj.GetUserName();
    //    //}
    //    return User_info.GetUserName();
    //}
    int d;
    private int GetLevelDetail(UserProgress_ShapeMatch Level_completed_Details)   // for getting the last played level
    {
        //foreach (var level_obj in Level_completed_Details)
        //{
        //    d = level_obj.GetCompletedLevel();
        //}
        return Level_completed_Details.GetCompletedLevel();
    }
    IEnumerator backAgain(Vector3 playerpos, GameObject temp_Go)        // for moving the object to a particular position with some speed
    {
        float speed = Camera.main.pixelWidth * 0.8f;
        var step = speed * Time.deltaTime;
        while ((Vector3.Distance(temp_Go.transform.position, playerpos)) >= 0.5)
        {
            temp_Go.transform.position = Vector3.MoveTowards(temp_Go.transform.position, playerpos, step);
            yield return null;
            
        }
        //if (Vector3.Distance(temp_Go.transform.position, playerpos) <= 0.5)
        //{
            temp_Go.transform.position = playerpos;
        //}


    }
    public void ChangeColor( Color Color_reqired)
    {
        for (int i = 0; i < PreviousLevel; i++)
        {
            Object1.transform.GetChild(i).GetComponent<Image>().color = Color_reqired;
        }
    }
}
