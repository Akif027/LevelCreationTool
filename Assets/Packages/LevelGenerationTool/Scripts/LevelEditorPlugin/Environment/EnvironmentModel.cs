// Make sure this is at the top of the script
using System.Collections.Generic;
using LevelEditorPlugin.Runtime;
using UnityEngine;

public class EnvironmentModel
{
    public List<LevelData.PlacedEnvironment> PlacedEnvironments { get; private set; } = new List<LevelData.PlacedEnvironment>();

    public void AddEnvironment(LevelData.PlacedEnvironment environment)
    {
        PlacedEnvironments.Add(environment);
    }

    public void RemoveEnvironment(UnityEngine.Vector3 position) // Explicitly using UnityEngine.Vector3
    {
        PlacedEnvironments.RemoveAll(e => e.position == position);
    }

    public void ClearAllEnvironments()
    {
        PlacedEnvironments.Clear();
    }
}
