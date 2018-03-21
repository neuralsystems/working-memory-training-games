using SQLite4Unity3d;

public class ScoreMatrix {

	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public int ScoreValue { get; set; }
	public int Level { get; set; }
	public float Time { get; set; }
	public int ForCorrect { get; set; }

	public override string ToString ()
	{
		return string.Format ("[Person: Id={0}, Score={1},  Level={2}, Time={3}]", Id, ScoreValue, Level,Time);
	}

	public float GetScoreValue()
	{
		return this.ScoreValue;
	}

	public int GetLevel()
	{
		return this.Level;
	}

	public float GetTime()
	{
		return this.Time;
	}

	public int GetForCorrect()
	{
		return this.ForCorrect;
	}
}
