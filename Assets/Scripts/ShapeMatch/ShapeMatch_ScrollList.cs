using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeMatch_ScrollList : MonoBehaviour
{
    public Transform contentPanel;
    public List<string> Userdetails = new List<string>();
    public SimpleObjectPool SampleObject;


    void Start()
    {
        GetUserDetails();
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        AddButtons();
    }
    
    private void AddButtons()
    {
        for (int i = 0; i< Userdetails.Count; i++) 
        {
            string username = Userdetails[i];
            GameObject newButton = SampleObject.GetObject();
            newButton.transform.SetParent(contentPanel);

            ShapeMatch_SampleButton sampleButton = newButton.GetComponent<ShapeMatch_SampleButton>();
            sampleButton.Setup(username);
        }
    }
    void GetUserDetails()
    {
        var ds = new ShapeMatch_DataService(MainScript.DATABASE_NAME);
        var userDetails = ds.GetPersons();
        ToConsole(userDetails);

    }
    private void ToConsole(IEnumerable<User> userDetails)
    {
        foreach (var person in userDetails)
        {
            string str = person.GetUserName();
            Userdetails.Add(str);
        }
    }
}
