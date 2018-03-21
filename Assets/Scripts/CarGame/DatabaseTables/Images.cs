using SQLite4Unity3d;

public class Images  {

	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string FileUrl { get; set; }
	public int category { get; set; }

	public override string ToString ()
	{
		return string.Format ("[Person: Id={0}, LeftImage={1},  Category={2}]", Id, FileUrl, category);
	}

	public string GetImageName()
	{
		return this.FileUrl;
	}

	public int GetCategory()
	{
		return this.category;
	}
}
