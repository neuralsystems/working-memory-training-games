using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class MApp_DataServices
{

    private SQLiteConnection _connection;
    int Default_IQ = 80;
    public MApp_DataServices(string DatabaseName)
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
        Debug.Log("Final PATH: " + dbPath);

    }

    public IEnumerable<User> GetAllUsers()
    {
        return _connection.Table<User>();
    }
    //
    public IEnumerable<User> GetUser(int id)
    {
        return _connection.Table<User>().Where(x => x.Id == id);
    }

    public IEnumerable<User> GetUser(string username)
    {
        return _connection.Table<User>().Where(x => x.Username == username);
    }

    public void CreateUser(string Username, string dob, string diagnosis, int IQ = 80, string First_name = "Rohit", string Last_name = "Shetty")
    {
         _connection.Insert( new User() { Id = 1, Username = Username, Dob = dob, Diagnosis = diagnosis, IQ = IQ, First_Name = First_name, Last_Name = Last_name });
    }

    public IEnumerable<PianoGame_TonesForLevels> GetRandomToneForLevel(int level)
    {
        const string command = "SELECT * FROM PianoGame_TonesForLevels where Level = ? ORDER BY RANDOM() LIMIT 1";
        return _connection.Query<PianoGame_TonesForLevels>(command, level);
    }

    public IEnumerable<PianoGame_TonesForLevels> GetRandomTone()
    {
        const string command = "SELECT * FROM PianoGame_TonesForLevels ORDER BY RANDOM() LIMIT 1";
        return _connection.Query<PianoGame_TonesForLevels>(command);
    }
    
}
