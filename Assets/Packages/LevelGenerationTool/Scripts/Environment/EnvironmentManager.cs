using UnityEngine;
using UnityEditor;

namespace LevelEditorPlugin.Runtime
{
    public class EnvironmentManager
    {
        private readonly EnvironmentModel environmentModel;

        public EnvironmentManager(EnvironmentModel model)
        {
            environmentModel = model;
        }

        public void InstantiateEnvironment(EnvironmentData environmentData, LevelData levelData)
        {
            Vector3 position = environmentModel.GetSceneCenterInWorld();
            position.z = 0;  // Ensure the z position is set to 0

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(environmentData.prefab);
            if (instance != null)
            {
                instance.transform.position = position;
                instance.transform.SetParent(environmentModel.GetEnvironmentContainer().transform);
                Selection.activeGameObject = instance;

                levelData.placedEnvironments.Add(new LevelData.PlacedEnvironment
                {
                    environmentData = environmentData,
                    position = position,
                    rotation = instance.transform.rotation
                });
            }
        }


        public void ReplaceEnvironment(GameObject oldEnvironment, EnvironmentData newEnvironmentData, LevelData levelData)
        {
            Vector3 position = oldEnvironment.transform.position;
            Quaternion rotation = oldEnvironment.transform.rotation;
            Object.DestroyImmediate(oldEnvironment);

            GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(newEnvironmentData.prefab);
            if (newInstance != null)
            {
                newInstance.transform.position = position;
                newInstance.transform.rotation = rotation;
                newInstance.transform.SetParent(environmentModel.GetEnvironmentContainer().transform);
                Selection.activeGameObject = newInstance;

                var existingEnvironment = levelData.placedEnvironments.Find(e => e.position == position);
                if (existingEnvironment != null)
                {
                    existingEnvironment.environmentData = newEnvironmentData;
                    existingEnvironment.rotation = rotation;
                }
                else
                {
                    levelData.placedEnvironments.Add(new LevelData.PlacedEnvironment
                    {
                        environmentData = newEnvironmentData,
                        position = position,
                        rotation = rotation
                    });
                }
            }
        }

        public void DeleteEnvironment(GameObject environment, LevelData levelData)
        {
            Vector3 position = environment.transform.position;
            Object.DestroyImmediate(environment);
            levelData.placedEnvironments.RemoveAll(e => e.position == position);
        }

        public void DeleteAllEnvironments(LevelData levelData)
        {
            var environmentContainer = environmentModel.GetEnvironmentContainer();
            if (environmentContainer != null)
            {
                Object.DestroyImmediate(environmentContainer);
            }
            levelData.placedEnvironments.Clear();
        }
    }
}
