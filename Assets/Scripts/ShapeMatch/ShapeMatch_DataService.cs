using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System;
using System.Collections.Generic;

public class ShapeMatch_DataService
{

    private SQLiteConnection _connection;

    public ShapeMatch_DataService(string DatabaseName)
    {

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
        //Debug.Log("Final PATH: " + dbPath);

    }
    public IEnumerable<User> GetPersons()
    {
        return _connection.Table<User>();
    }

    public User GetPersonsWithUserName(string username)
    {
        var user_list = _connection.Table<User>().Where(x => x.Username == username);
        foreach(var user in user_list)
        {
            return user;
        }
        return new User();
    }

    public UserProgress_ShapeMatch GetCompletedLevel(string username)
    {
        var user_level = _connection.Table<UserProgress_ShapeMatch>().Where(x => x.Username == username);
        foreach (var user_level_obj in user_level)
        {
            return user_level_obj;
        }
        return AddUserProgress(username);
    }

    //public UserProgress_ShapeMatch AddProgress()
    //{
    //    var p = new UserProgress_ShapeMatch
    //    {
    //        Id = 05,
    //        Username = "user05",
    //        LevelCompleted = 0,
    //        DateCreated = DateTime.Now.ToString("yyyy-MM-dd / h:mm:ss tt"),
    //        LastModified= DateTime.Now.ToString("yyyy-MM-dd / h:mm:ss tt")
    //    };
    //    _connection.Insert(p);
    //    return p;
    //}

    public IEnumerable<ShapeMatch_levels> GetLevelsInfo(int level_number)
    {
        return _connection.Table<ShapeMatch_levels>().Where(c => c.LevelNumber == level_number);
    }
    public void UpdateUserProgress(string username, int level_number)
    {
        var user_level_obj = GetUserProgress(username);
        user_level_obj.LevelCompleted = level_number;
        user_level_obj.LastModified = DateTime.Now;
        _connection.Update(user_level_obj);

    }

    public UserProgress_ShapeMatch GetUserProgress(string username)
    {
        //var _game_name = GetGameName();
        var user_level = _connection.Table<UserProgress_ShapeMatch>().Where(x => x.Username == username);
        foreach (var user_level_obj in user_level)
        {
            return user_level_obj;
        }
        return AddUserProgress(username);
    }

    public UserProgress_ShapeMatch AddUserProgress(string username)
    {
        var default_level = 0;
        var max_ids = _connection.Query<UserProgress_ShapeMatch>("SELECT *, max(Id) FROM UserProgress_ShapeMatch LIMIT 1");
        int id = 0;
        foreach (var max_id in max_ids)
        {
            id = max_id.Id;
        }
        _connection.Insert(new UserProgress_ShapeMatch() { Username = username, LevelCompleted = default_level, LastModified = DateTime.Now, DateCreated = DateTime.Now });
        return GetCompletedLevel(username);
    }
    public void AddLevels(List<ShapeMatch_levels> levels_list)
    {
        _connection.InsertAll(levels_list);
    }

    public void AddLevel(ShapeMatch_levels level_obj)
    {
        _connection.Insert(level_obj);
    }

    public void DeleteLevel(ShapeMatch_levels level_obj)
    {
        _connection.Delete(level_obj);

    }
    public List<string> ListAllLevels()
    {

        var result = _connection.Table<ShapeMatch_levels>();
        List<string> result_string = new List<string>();
        foreach (var r in result)
        {
            result_string.Add(r.ToString());
        }
        return result_string;
    }
}