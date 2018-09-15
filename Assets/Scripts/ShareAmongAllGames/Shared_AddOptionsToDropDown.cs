using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shared_AddOptionsToDropDown : MonoBehaviour {

    public int code;
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
            for (int i = 1990; i < 2018; i++)
            {
                l.Add(i.ToString());
            }
        }
        drop_down.AddOptions(l);
    }
}
