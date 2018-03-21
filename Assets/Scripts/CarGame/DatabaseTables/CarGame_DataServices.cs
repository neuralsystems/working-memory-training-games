using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class CarGame_DataService  {

	private SQLiteConnection _connection;

	public CarGame_DataService(string DatabaseName){

		#if UNITY_EDITOR
		var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
		#else
		// check if file exists in Application.persistentDataPath
		var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

		if (!File.Exists(filepath))
		{
		Debug.Log("Database not in Persistent path");
		// if it doesn't ->
		// open StreamingAssets directory and load the db ->

		#if UNITY_ANDROID 
		var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
		while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
		// then save to Application.persistentDataPath
		File.WriteAllBytes(filepath, loadDb.bytes);
		#elif UNITY_IOS
		var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		#elif UNITY_WP8
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);

		#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		#else
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);

		#endif

		Debug.Log("Database written");
		}

		var dbPath = filepath;
		#endif
		_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
//		_connection.CreateTable<Images>();
		Debug.Log("Final PATH: " + dbPath);     

		}

		public IEnumerable<Images> GetImages(){
			return _connection.Table<Images>();
		}

		public IEnumerable<Images> GetImage( int ID){
			return _connection.Table<Images>().Where(x => x.Id == ID);
		}

		public IEnumerable<Category> GetCategory( string name){
			return _connection.Table<Category>().Where(x => x.CategoryName == name);
		}

		public IEnumerable<Category> GetRandomCategory(){
			const string command = "SELECT * FROM Category ORDER BY RANDOM() LIMIT 1";
			return _connection.Query<Category>(command);
		}

		public IEnumerable<Images> GetnImages(int n){
			const string command = "SELECT FileUrl FROM Images ORDER BY RANDOM() LIMIT ?";
			return _connection.Query<Images>(command, n);
		}

		public IEnumerable<Images> GetnImagesFromCategory(int n, int categoryId){
//			const string command = "Select FileUrl from Images join Category where Images.Category = Category.Id and Category.CategoryName = ? ORDER BY RANDOM() LIMIT ?";
		const string command = "Select FileUrl from Images where category = ? ORDER BY RANDOM() LIMIT ?";
		return _connection.Query<Images>(command,categoryId, n);
		}

//		public IEnumerable<Images> GetnImagesOfCategory(int n, int category){
//		const string command = "SELECT FileUrl FROM Images ORDER BY RANDOM() LIMIT ?";
//		return _connection.Query<Images>(command, n);
//		}

		public IEnumerable<ScoreMatrix> GetScoreValue( int Level, float Time, int forCorrect ){
			const string command = "SELECT scoreValue FROM ScoreMatrix where Level = ? and Time >=? and ForCorrect = ? ORDER BY Time ASC LIMIT 1";
			return _connection.Query<ScoreMatrix>(command, Level,Time,forCorrect);
//		return _connection.Table<ScoreMatrix>().Where(x => x.Level == Level && x.Time == Time && x.ForCorrect == isCorrect);
		}

//		public IEnumerable<NextLevelToLoad> GetNextLevelToLoad(int currentLevel, int score){
//		const string command = "SELECT * FROM NextLevelToLoad WHERE CurrentLevel = ? and UpperLimit >= ? and LoverLimit <= ? ORDER BY";
//		return _connection.Query<NextLevelToLoad> (command, currentLevel, score, score);
//		}

		public IEnumerable<NextLevelToLoad> GetNextLevelToLoad(string currentLevel){
			const string command = "SELECT * FROM NextLevelToLoad WHERE CurrentLevel = ? ";
			return _connection.Query<NextLevelToLoad> (command, currentLevel);
		}

	}
