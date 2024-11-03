namespace LevelEditorPlugin.Runtime
{
    using UnityEngine;
    using UnityEditor;

    public class EnvironmentManager
    {
        private EnvironmentModel environmentModel;

        public EnvironmentManager(EnvironmentModel model)
        {
            environmentModel = model;
        }

        private GameObject GetEnvironmentContainer()
        {
            GameObject container = GameObject.Find("EnvironmentContainer");
            if (container == null)
            {
                container = new GameObject("EnvironmentContainer");
            }
            return container;
        }

        public GameObject InstantiateEnvironment(EnvironmentData environmentData, Vector3 position)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(environmentData.prefab);
            if (instance != null)
            {
                instance.transform.position = position;
                instance.transform.SetParent(GetEnvironmentContainer().transform);

                var placedEnvironment = new LevelData.PlacedEnvironment
                {
                    environmentData = environmentData,
                    position = instance.transform.position,
                    rotation = instance.transform.rotation
                };

                environmentModel.AddEnvironment(placedEnvironment);
            }
            return instance;
        }

        public void ReplaceEnvironment(GameObject oldEnvironment, EnvironmentData newEnvironmentData)
        {
            Vector3 position = oldEnvironment.transform.position;
            Quaternion rotation = oldEnvironment.transform.rotation;
            Object.DestroyImmediate(oldEnvironment);

            GameObject newInstance = InstantiateEnvironment(newEnvironmentData, position);
            if (newInstance != null)
            {
                newInstance.transform.rotation = rotation;
            }
        }

        public void DeleteEnvironment(GameObject environment)
        {
            Vector3 position = environment.transform.position;
            Object.DestroyImmediate(environment);
            environmentModel.RemoveEnvironment(position);
        }

        public void DeleteAllEnvironments()
        {
            GameObject container = GameObject.Find("EnvironmentContainer");
            if (container != null)
            {
                Object.DestroyImmediate(container);
            }
            environmentModel.ClearAllEnvironments();
        }
    }
}
