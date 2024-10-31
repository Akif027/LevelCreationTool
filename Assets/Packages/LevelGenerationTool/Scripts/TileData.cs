using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTileData", menuName = "Tile/Tile Data")]
public class TileData : ScriptableObject
{
    public string prefabName;     // Name of the prefab for identification
    public Sprite icon;           // Icon to display in the Level Editor window
    public GameObject prefab;     // Reference to the prefab itself
    public bool isCorrectPrefab;  // Indicates if this prefab is part of a correct solution (optional)
}
