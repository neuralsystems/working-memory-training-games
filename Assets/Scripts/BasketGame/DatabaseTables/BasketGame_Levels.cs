using System;
using SQLite4Unity3d;


public class BasketGame_Levels
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public int NumBasket { get; set; }
	public int Capacity { get; set; }
	public int LevelNumber { get; set; }

	public BasketGame_Levels ()
	{
	}

	public override string ToString (){
		return String.Format("Level:=  {0}, numbaskets:= {1}, Capacity:= {2}", LevelNumber, NumBasket, Capacity);
	}

	public int GetLevel(){
		return this.LevelNumber;
	}

	public int GetCapacity(){
		return this.Capacity;
	}

	public int GetNumofBaskets(){
		return this.NumBasket;
	}
}


