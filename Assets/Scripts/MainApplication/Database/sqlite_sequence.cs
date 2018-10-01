using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
public class sqlite_sequence  {

   
    public string name { get; set; }
    public string  seq { get; set; }
   
    public string ToString()
    {
        return string.Format("{0}:{1}", name, seq);
    }
    public string GetName()
    {
        return this.name;
    }
    public string GetSeq()
    {
        return this.seq;
    }
}
