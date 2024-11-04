using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace LevelEditorPlugin.Runtime
{
    public class EnvironmentModel
    {
        private const string EnvironmentContainerName = "EnvironmentContainer";

        public List<LevelData.PlacedEnvironment> PlacedEnvironments { get; private set; } = new List<LevelData.PlacedEnvironment>();

        public void AddEnvironment(LevelData.PlacedEnvironment environment)
        {
            PlacedEnvironments.Add(environment);
        }

        public void RemoveEnvironment(Vector3 position)
        {
            PlacedEnvironments.RemoveAll(e => e.position == position);
        }

        public void ClearAllEnvironments()
        {
            PlacedEnvironments.Clear();
        }

        public GameObject GetEnvironmentContainer()
        {
            GameObject container = GameObject.Find(EnvironmentContainerName);
            if (container == null)
            {
                container = new GameObject(EnvironmentContainerName);
            }
            return container;
        }

        public Vector3 GetSceneCenterInWorld()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            return sceneView != null ? sceneView.camera.transform.position + sceneView.camera.transform.forward * 10f : Vector3.zero;
        }
    }
}
