using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MApp_AcrossSceneStaticVariables {

    private static User CurrentPlayer;

    public static User GetCurrentPlayer()
    {
        return CurrentPlayer;
    }

    public static void SetCurrentPlayer(User _player)
    {
        CurrentPlayer = _player;
    }
}
