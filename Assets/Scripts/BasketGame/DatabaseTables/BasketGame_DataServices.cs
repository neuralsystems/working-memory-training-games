using SQLite4Unity3d;
using UnityEngine;
using System;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class BasketGame_DataService  {

	private SQLiteConnection _connection;

	public BasketGame_DataService(string DatabaseName){

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

		public IEnumerable<BasketGame_Levels> GetLevels(){
		return _connection.Table<BasketGame_Levels>();
		}

		public IEnumerable<BasketGame_Levels> GetLevelsObject( int currentLevel){
		return _connection.Table<BasketGame_Levels>().Where(x => x.LevelNumber == currentLevel);
		}

		public IEnumerable<BasketGame_Levels> GetRandomLevel(){
		const string command = "SELECT * FROM BasketGame_Levels ORDER BY RANDOM() LIMIT 1";
		return _connection.Query<BasketGame_Levels>(command);
		}
        
        public void UpdateUserProgress(string username, int level_number)
        {
            var user_level_obj = GetUserProgress(username);
            user_level_obj.Level_Obj = level_number;
            _connection.Update(user_level_obj);
        
        }

        public void MarkPreLevelCompleted(string username)
        {
            var user_level_obj = GetUserProgress(username);
            user_level_obj.PreLevelCompleted = BasketGame_SceneVariables.VALUE_FOR_PRE_LEVEL_COMPLETE;
            user_level_obj.LastModified = DateTime.Now;
            _connection.Update(user_level_obj);

        }

        public UserProgress_BasketGame GetUserProgress(string username)
        {
            //var _game_name = GetGameName();
            var user_level = _connection.Table<UserProgress_BasketGame>().Where(x => x.User_Obj == username);/*.Where(x => x.Game_name == _game_name);*/
            foreach (var user_level_obj in user_level)
            {
               return user_level_obj;
            }
            return AddUserProgress(username);
        }

        public UserProgress_BasketGame AddUserProgress(string username)
        {
            var default_level = 1;
            var max_ids =_connection.Query< UserProgress_BasketGame>("SELECT *, max(Id) FROM UserProgress_BasketGame LIMIT 1");
            int id = 0;
            foreach(var max_id in max_ids)
            {
                id = max_id.Id;
            }
            _connection.Insert(new UserProgress_BasketGame() {  User_Obj = username, Level_Obj = default_level, DateCreated = DateTime.Now,LastModified = DateTime.Now  });
            return GetUserProgress(username);
        }

        //public int GetGameName()
        //{
        //    var gn = BasketGame_SceneVariables.Game_Name;
        //    var gns = _connection.Table<Games>().Where(x => x.GameName == gn);
        //    var l = 1;
        //    foreach(var g in gns)
        //    {
        //        return g.Id;
        //    }
        //    return l;
        //}
}
