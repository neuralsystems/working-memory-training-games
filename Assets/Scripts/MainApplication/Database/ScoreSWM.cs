using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
public class ScoreSWM {

    [PrimaryKey, AutoIncrement]
	public int Id { get; set; }
    public string TestedUser { get; set; }
    public int WithInErrorCount { get; set; }
    public int BetweenErrorCount { get; set; }
    public string DateOfTest { get; set; }
    public int NumOfBlocks { get; set; }

    public override string ToString()
    {
        MApp_DataServices ds = new MApp_DataServices(MApp_UserInforFormScript.database_Name);
        var _current_users = ds.GetUser(this.Id);
        var Username = "";
        foreach(var user in _current_users)
        {
            Username = user.GetUserName();
        }
        return string.Format("[SWMScore: Id={0}, tested_user = {1}, WithInErrorCount = {2},  BetweeenErrorCount = {3}, administered date= {4}, # of Blocks = {5} ]", Id, Username, WithInErrorCount, BetweenErrorCount, DateOfTest, NumOfBlocks);
    }


}
