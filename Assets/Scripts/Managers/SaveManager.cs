using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System;

[System.Serializable]
public struct SaveData
{
    public string version;
    public SkillTreeData[] skillTrees;

    public OptionsData options;

    public TrackedStat[] stats;

    public Unlockable[] unlockables;

    public string[] viewedTutorials;
}

[System.Serializable]
public struct SkillTreeData
{
    public int[] skills;
    public int   totalPoints;
    public int   currentPoints;
}

[System.Serializable]
public class OptionsData
{
    public float musicVolume = 1f;
    public float soundVolume = 1f;
    public bool  autoFire = true;
}

public class SaveManager
{
    static string versionNumber = "1.2";

    static readonly string m_Filename = "save_.json";
    static readonly string m_Path = Application.persistentDataPath + "/" + m_Filename;

    static SkillTree[] m_SkillTrees;
    static SaveData m_SaveData;

    private static void PopulateSkillTreesArray(SkillTree[] trees)
    {
        m_SkillTrees = trees;
    }

    public static void SaveToFile() // Called every time skill menu is closed
    {
        m_SaveData.version = versionNumber;
        SaveSkills();

        SaveOptions();

        SaveUnlocks();

        SaveViewedTutorials();

        // Save data as JSON
        string json = JsonUtility.ToJson(m_SaveData, true);
        if (json == null) return;
        File.WriteAllText(m_Path, json);
        if (!SteamworksManager.failed && Steamworks.SteamRemoteStorage.IsCloudEnabled)
            Steamworks.SteamRemoteStorage.FileWrite(m_Filename, File.ReadAllBytes(m_Path));
        Debug.Log(json);
    }

    private static void SaveSkills()
    {
        // Save all current skills to json
        // Save all skill points to json

        if (m_SkillTrees == null || m_SkillTrees.Length == 0) {
            LoadSkills();
            return;
        }

        m_SaveData.skillTrees = new SkillTreeData[m_SkillTrees.Length];

        for (int i = 0; i < m_SkillTrees.Length; i++)
        {
            SkillTree tree = m_SkillTrees[i];
            SkillIcon[] skills = tree.GetComponentsInChildren<SkillIcon>();
            // Serialise data in skill tree into save data format
            m_SaveData.skillTrees[i].totalPoints = tree.m_TotalSkillPoints;
            m_SaveData.skillTrees[i].currentPoints = tree.m_CurrentSkillPoints;
            m_SaveData.skillTrees[i].skills = new int[skills.Length];

            for (int j = 0; j < skills.Length; j++)
            {
                m_SaveData.skillTrees[i].skills[j] = skills[j].m_Data.level;
            }
        }
    }
    private static void SaveOptions()
    {
        m_SaveData.options = new OptionsData();
        m_SaveData.options.musicVolume = AudioManager.m_MusicVolume;
        m_SaveData.options.soundVolume = AudioManager.m_SoundVolume;
        m_SaveData.options.autoFire    = Player.m_AutoFire;
    }
    private static void SaveUnlocks()
    {
        m_SaveData.stats = UnlockManager.m_TrackedStats.ToArray();

        m_SaveData.unlockables = UnlockManager.m_Unlockables.ToArray();
    }
    private static void SaveViewedTutorials()
    {
        m_SaveData.viewedTutorials = TutorialManager.m_ViewedTutorials.ToArray();
    }
    public static void LoadFromFile(SkillTree[] skillTrees) // Called on game start
    {
        // Load json
        // Set values in json to files in skill tree class
        bool cloudExists = false;
        if (!SteamworksManager.failed) cloudExists = Steamworks.SteamRemoteStorage.FileExists(m_Filename);

        if (!File.Exists(m_Path) && !cloudExists)
        {
            Debug.LogWarning("Save file not found: " + m_Path);
            m_SaveData = new SaveData(); // or null, depending on your needs
            PopulateSkillTreesArray(skillTrees);
            SaveToFile();
            UnlockManager.PopulateTrackedStats();
            UnlockManager.PopulateUnlockables();
            return;
        }

        string json;
        if (!SteamworksManager.failed && Steamworks.SteamRemoteStorage.IsCloudEnabled && Steamworks.SteamRemoteStorage.FileExists(m_Filename) && !Debug.isDebugBuild)
        {
            byte[] buffer = Steamworks.SteamRemoteStorage.FileRead(m_Filename);
            json = System.Text.Encoding.UTF8.GetString(buffer);

            m_SaveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            // Convert json into save data format
            json = File.ReadAllText(m_Path);
            m_SaveData = JsonUtility.FromJson<SaveData>(json);
        }
        PopulateSkillTreesArray(skillTrees);

        LoadSkills();
        LoadOptions();
        LoadUnlocks();
        LoadViewedTutorials();
        NewVersion();
    }

    private static void LoadSkills()
    {
        if (m_SaveData.skillTrees == null) return;
        for (int i = 0; i < m_SaveData.skillTrees.Length; i++)
        {
            // De-serialise data from save data into skill tree data
            SkillTreeData treeData = m_SaveData.skillTrees[i];
            int[] skillData = m_SaveData.skillTrees[i].skills;
            SkillTree tree = m_SkillTrees[i];

            tree.m_TotalSkillPoints = treeData.totalPoints;
            tree.m_CurrentSkillPoints = treeData.currentPoints;

            for (int j = 0; j < skillData.Length; j++)
            {
                tree.GetComponentsInChildren<SkillIcon>()[j].InitFromFile(skillData[j]);
            }
            tree.TryUnlockAchievement();
        }
    }

    private static void LoadOptions()
    {
        AudioManager.m_MusicVolume = m_SaveData.options.musicVolume;
        AudioManager.m_SoundVolume = m_SaveData.options.soundVolume;
        Player.m_AutoFire          = m_SaveData.options.autoFire;
    }

    public static void LoadUnlocks()
    {
        if (m_SaveData.stats.Length == 0) UnlockManager.PopulateTrackedStats();
        else UnlockManager.m_TrackedStats = m_SaveData.stats?.ToList();

        if (m_SaveData.unlockables.Length == 0) UnlockManager.PopulateUnlockables();
        else UnlockManager.m_Unlockables = m_SaveData.unlockables?.ToList();

        UnlockManager.CheckUnlockConditions();
    }

    public static void LoadViewedTutorials()
    {
        if (m_SaveData.viewedTutorials.Length == 0) return;
        TutorialManager.m_ViewedTutorials = m_SaveData.viewedTutorials?.ToList();
    }

    private static void NewVersion()
    {
        // Things to do if the game version changes.
        if (m_SaveData.version == versionNumber) return;
    }

    // Adds skill points to the specified tree. Called at the end of a run
    public static void AddSkillPoints(int points, SkillTree tree)
    {
        tree.m_TotalSkillPoints += points;
    }
}
