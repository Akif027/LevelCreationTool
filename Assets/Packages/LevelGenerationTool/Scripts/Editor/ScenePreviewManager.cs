using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ScenePreviewManager : MonoBehaviour
{
    // public LevelData levelData;

    // private void OnValidate()
    // {
    //     ClearGrid();
    //     GeneratePreviewGrid();
    // }

    // private void ClearGrid()
    // {
    //     foreach (Transform child in transform)
    //     {
    //         DestroyImmediate(child.gameObject);
    //     }
    // }

    // private void GeneratePreviewGrid()
    // {
    //     if (levelData == null) return;

    //     for (int x = 0; x < levelData.gridSizeX; x++)
    //     {
    //         for (int y = 0; y < levelData.gridSizeY; y++)
    //         {
    //             TileData tileData = levelData.GetTileAt(x, y);
    //             if (tileData != null && tileData.prefab != null)
    //             {
    //                 Vector3 position = new Vector3(x, 0, y);
    //                 var instance = Instantiate(tileData.prefab, position, Quaternion.identity, transform);
    //                 instance.name = $"{tileData.tileName}_Preview";
    //             }
    //         }
    //     }
    // }
}
