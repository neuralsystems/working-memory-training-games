using SQLite4Unity3d;

public class NextLevelToLoad  {

	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string CurrentLevel { get; set; }
	public string NextLevel { get; set; }
	public string PreviousLevel { get; set; }

	// both the limits are inclusive Score in [LowerLimit, UpperLimit]
	public int UpperLimit { get; set; }		
	public int LowerLimit { get; set; }


	public override string ToString ()
	{
		return string.Format ("[NextLevelToLoad: Id={0}, CurrentLevel={1}, NextLevel={2}, UpperLimit={3}, LowerLimit={4}]", Id, CurrentLevel, NextLevel, UpperLimit, LowerLimit);
	}

	public string GetNextLevel()
	{
		return this.NextLevel;
	}


	public string GetCurrentLevel()
	{
		return this.CurrentLevel;
	}


	public string GetPreviousLevel()
	{
		return this.PreviousLevel;
	}


	public int GetUpperLimit()
	{
		return this.UpperLimit;
	}


	public int GetLowerLimit()
	{
		return this.LowerLimit;
	}
}
