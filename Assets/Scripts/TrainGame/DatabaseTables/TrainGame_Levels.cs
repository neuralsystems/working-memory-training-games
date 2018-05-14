using System;
using SQLite4Unity3d;


public class TrainGame_Levels
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public int NumOfBogie { get; set; }
	public int ShouldBlock { get; set; }		// set this value to 1 in the database to show block train, and otherwise to doesn't show block train
	public int LevelNumber { get; set; }

	public TrainGame_Levels ()
	{
	}

	public override string ToString (){
		return String.Format("Level:=  {0}, numbaskets:= {1}, Capacity:= {2}", LevelNumber, NumOfBogie, ShouldBlock);
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



