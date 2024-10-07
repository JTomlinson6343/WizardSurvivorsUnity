using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamworksManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            Steamworks.SteamClient.Init(3146730);
            PrintName();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    void PrintName()
    {
        Debug.Log(Steamworks.SteamClient.Name);
    }

    private void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

    public static bool isAchievementUnlocked(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        return ach.State;
    }

    public static void UnlockAchievement(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();
    }
}
