using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService  {

	private SQLiteConnection _connection;

	public DataService(string DatabaseName){

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

		public IEnumerable<PianoGame_Levels> GetLevels(){
			return _connection.Table<PianoGame_Levels>();
		}
//
		public IEnumerable<PianoGame_Levels> GetLevel( int id){
		return _connection.Table<PianoGame_Levels>().Where(x => x.Id == id);
		}
//
		public IEnumerable<PianoGame_TonesForLevels> GetToneForLevel ( int id){
		return _connection.Table<PianoGame_TonesForLevels>().Where(x => x.Id == id);
		}
//
		public IEnumerable<PianoGame_TonesForLevels> GetRandomToneForLevel(int level){
		const string command = "SELECT * FROM PianoGame_TonesForLevels where Level = ? ORDER BY RANDOM() LIMIT 1";
		return _connection.Query<PianoGame_TonesForLevels>(command, level);
		}

		public IEnumerable<PianoGame_TonesForLevels> GetRandomTone(){
		const string command = "SELECT * FROM PianoGame_TonesForLevels ORDER BY RANDOM() LIMIT 1";
		return _connection.Query<PianoGame_TonesForLevels>(command);
		}
//
//		public IEnumerable<Images> GetImage( string imageName){
//		return _connection.Table<Images>().Where(x => x.FileUrl == imageName);
//		}
//
//		public IEnumerable<ScoreMatrix> GetScoreValue( int Level, float Time, int forCorrect ){
//		const string command = "SELECT scoreValue FROM ScoreMatrix where Level = ? and Time >=? and ForCorrect = ? ORDER BY Time ASC LIMIT 1";
//		return _connection.Query<ScoreMatrix>(command, Level,Time,forCorrect);
//		//		return _connection.Table<ScoreMatrix>().Where(x => x.Level == Level && x.Time == Time && x.ForCorrect == isCorrect);
//		}
//
//		//		public IEnumerable<NextLevelToLoad> GetNextLevelToLoad(int currentLevel, int score){
//		//		const string command = "SELECT * FROM NextLevelToLoad WHERE CurrentLevel = ? and UpperLimit >= ? and LoverLimit <= ? ORDER BY";
//		//		return _connection.Query<NextLevelToLoad> (command, currentLevel, score, score);
//		//		}
//
//		public IEnumerable<NextLevelToLoad> GetNextLevelToLoad(int currentLevel){
//		const string command = "SELECT * FROM NextLevelToLoad WHERE CurrentLevel = ? ";
//		return _connection.Query<NextLevelToLoad> (command, currentLevel);
//		}

	}
