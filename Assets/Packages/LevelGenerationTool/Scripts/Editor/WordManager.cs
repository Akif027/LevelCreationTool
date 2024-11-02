using TMPro;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class WordManager
{
    public void DrawWordManagement(LevelData levelData)
    {
        GUILayout.Label("Manage Words", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Word"))
        {
            levelData.words.Add(string.Empty);
        }

        for (int i = 0; i < levelData.words.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            levelData.words[i] = EditorGUILayout.TextField("Word", levelData.words[i]);

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                levelData.words.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }


    }
}