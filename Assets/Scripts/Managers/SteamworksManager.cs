using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamworksManager : MonoBehaviour
{
    static SteamworksManager instance;

    public static bool failed = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance) Destroy(gameObject);
        try
        {
            Steamworks.SteamClient.Init(3146730);
            PrintName();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            failed = true;
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
        if (failed) return false;
        var ach = new Steamworks.Data.Achievement(id);
        return ach.State;
    }

    public static void UnlockAchievement(string id)
    {
        if (failed) return;
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();
        Steamworks.SteamUserStats.StoreStats();
    }
}
