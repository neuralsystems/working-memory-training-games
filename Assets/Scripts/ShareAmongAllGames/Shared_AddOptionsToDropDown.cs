using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shared_AddOptionsToDropDown : MonoBehaviour {

    public int code;
    public SimpleObjectPool simpleGameObjectPool;
    public Transform contentPanel;
    private void Start()
    {
        AddOptionsToMenu();
    }
    void AddOptionsToMenu()
    {
        List<string> l = new List<string>();
        var drop_down = GetComponent<Dropdown>();
        drop_down.ClearOptions();
        if (code == 1)                              // code for adding days
        {
            for(int i = 1; i < 32; i++)
            {
                l.Add(i.ToString());
            }
        }
        if (code == 2)                             // code for adding months
        {
            for (int i = 1; i < 13; i++)
            {
                l.Add(i.ToString());
            }
        }
        if (code == 3)                             // code for adding years
        {
            for (int i = 1970; i < 2019; i++)
            {
                l.Add(i.ToString());
            }
        }
        if(code == 4)                              // code to add list of database tables
        {
            l = GetDatabaseTablesList();
        }
        drop_down.AddOptions(l);
    }

    List<string> GetDatabaseTablesList()
    {
        MApp_DataServices ds = new MApp_DataServices(MApp_UserInforFormScript.database_Name);
        List<string> tables = new List<string>();
        var res = ds.GetTables();
        foreach(var r in res)
        {
            tables.Add(r.ToString());
        }
        return tables;
    }

    public void ListRows()
    {
        var dd = GetComponent<Dropdown>();
        var table_name = dd.options[dd.value].text.Split(':')[0];
        List<string> rows = new List<string>();

        //var selected_table = System.Type.GetType(table_name);
        foreach (Transform list_obj in contentPanel.transform)
        {
            simpleGameObjectPool.ReturnObject(list_obj.gameObject);
        }
        rows.Clear();
        if (table_name == "TrainGame_Levels")
        {
            var ds = new TrainGame_DataServices(MApp_UserInforFormScript.database_Name);
           rows = ds.ListAllLevels();
        }
        else if (table_name == "BasketGame_Levels")
        {
            var ds = new BasketGame_DataService(MApp_UserInforFormScript.database_Name);
            rows = ds.ListAllLevels();
        }
        else if (table_name == "PianoGame_Levels")
        {
            var ds = new DataService(MApp_UserInforFormScript.database_Name);
            rows = ds.ListAllLevels();
        }
        else if (table_name == "ShapeMatch_levels")
        {
            var ds = new ShapeMatch_DataService(MApp_UserInforFormScript.database_Name);
            rows = ds.ListAllLevels();
        }
        else if (table_name == "FaceGame_GameData")
        {
            var ds = new FaceGame_DataService(MApp_UserInforFormScript.database_Name);
            rows = ds.ListAllLevels();
        }
        //else
        //{
        //    var ds = new TrainGame_DataServices(MApp_UserInforFormScript.database_Name);
        //    List<string> rows = ds.ListAllLevels();

        //}

        Debug.Log("selected table " + table_name);
        for (int i = 0; i < rows.Count; i++)
        {
            
            var _rowbutton = simpleGameObjectPool.GetObject();
            //_usericon.GuiButton.onClick.AddListener(() => { Function(param); OtherFunction(param); })
            _rowbutton.transform.SetParent(contentPanel);
            var _row_script = _rowbutton.GetComponent<DatabaseRowScript>();
            //Debug.Log("running the inner loop 1 " + u_icon.user.First_Name);
            _row_script.SetUp(rows[i]);

        }
    }


}
