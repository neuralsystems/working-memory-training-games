using SQLite4Unity3d;

public class Category  {

	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string CategoryName { get; set; }


	public override string ToString ()
	{
		return string.Format ("[Person: Id={0}, Name={1} ]", Id, CategoryName);
	}

	public string GetName(){
		return this.CategoryName;
	}
}
