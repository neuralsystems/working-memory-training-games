using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
public class User : MonoBehaviour {

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Username { get; set; }
    public float Age { get; set; }
    //public string DoB { get; set; }
    public int IQ { get; set; }
    public string Diagnosis { get; set;}
    public string First_Name { get; set; }
    public string Last_Name { get; set; }


    public override string ToString()
    {
        return string.Format("[Person: Id={0}, Username={1}, name = {2} {3} ]", Id, Username, First_Name, Last_Name);
    }


    public string GetUserName()
    {
        return this.Username;
    }
}
