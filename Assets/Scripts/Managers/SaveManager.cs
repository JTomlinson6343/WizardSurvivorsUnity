using UnityEditor;
using UnityEngine;

public class SaveManager
{
    static SkillTree[] m_SkillTrees;

    public static void PopulateSkillTreesArray(SkillTree[] trees)
    {
        m_SkillTrees = trees;
    }
    public static void SaveToFile() // Called every time skill menu is closed
    {
        // Save all current skills to json
        // Save all skill points to json
    }
    public static void LoadFromFile() // Called on game start
    {
        // Load json
        // Set values in json to files in skill tree class
    }

    // Adds skill points to the specified tree. Called at the end of a run
    public static void AddSkillPoints(int points, SkillTree tree)
    {

    }
}
