using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
public class Games  {

	[PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public string GameName { get; set; }


}
