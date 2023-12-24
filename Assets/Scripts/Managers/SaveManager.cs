using System;
using System.Xml;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct SaveData
{
    public SkillTreeData[] skillTrees;
}

[System.Serializable]
public struct SkillTreeData
{
    public int[] skills;
    public int points;
}

public class SaveManager
{
    static readonly string m_Path = Application.dataPath + "/Saves/save.json";

    static SkillTree[] m_SkillTrees;
    static SaveData m_SaveData;

    public static void PopulateSkillTreesArray(SkillTree[] trees)
    {
        m_SkillTrees = trees;
    }
    public static void SaveToFile() // Called every time skill menu is closed
    {
        // Save all current skills to json
        // Save all skill points to json

        if (m_SkillTrees == null) { return; }

        m_SaveData.skillTrees = new SkillTreeData[m_SkillTrees.Length];

        for (int i = 0; i < m_SkillTrees.Length; i++)
        {
            SkillTree tree = m_SkillTrees[i];
            SkillIcon[] skills = tree.GetComponentsInChildren<SkillIcon>();
            m_SaveData.skillTrees[i].points = tree.m_TotalSkillPoints;
            m_SaveData.skillTrees[i].skills = new int[skills.Length];

            for (int j = 0; j < skills.Length; j++)
            {
                m_SaveData.skillTrees[i].skills[j] = skills[j].m_Data.level;
            }
        }

        string json = JsonUtility.ToJson(m_SaveData, true);
        File.WriteAllText(m_Path, json);
        Debug.Log(json);
    }
    public static void LoadFromFile() // Called on game start
    {
        // Load json
        // Set values in json to files in skill tree class
    }

    // Adds skill points to the specified tree. Called at the end of a run
    public static void AddSkillPoints(int points, SkillTree tree)
    {
        tree.m_TotalSkillPoints += points;
    }
}
