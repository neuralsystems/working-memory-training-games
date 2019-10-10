using System;
using SQLite4Unity3d;


public class TrainGame_Levels
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public int NumOfBogie { get; set; }
	public int ShouldBlock { get; set; }		// set this value to 1 in the database to show block train, and otherwise to doesn't show block train
	public int LevelNumber { get; set; }
    public float WaitTime { get; set; }
	public TrainGame_Levels ()
	{
	}

	public override string ToString (){
		return String.Format("Id:= {4} Level:=  {0}, num of bogies:= {1}, Should Block:= {2}, wait time:= {3}", LevelNumber, NumOfBogie, ShouldBlock, WaitTime, Id);
	}

	public int GetLevel(){
		return this.LevelNumber;
	}

	public bool ShouldBlockTrain(){
		return this.ShouldBlock == 1;
	}

	public int GetNumofBogie(){
		return this.NumOfBogie;
	}
}



