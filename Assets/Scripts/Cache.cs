using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class TileTypeList
{
    public TileTypes TileType;
    public List<Tile> Tiles;

    public TileTypeList(TileTypes tileType)
    {
        TileType = tileType;
        Tiles = new List<Tile>();
    }
}

[Serializable]
public class InteractableObjectMapping
{
    public InteractableObjectTypes Type;
    public Sprite Sprite;
}

[CreateAssetMenu(fileName = "Cache", menuName = "ScriptableObjects/Cache", order = 1)]
public class Cache : ScriptableObject
{
    public static Cache Instance;

    public Tilemap TileMap;
    public List<TileTypeList> Tiles;
    public List<InteractableObjectMapping> InteractableObjectMappings;
    public GameObject InteractableObjectPrefab;
    public Dictionary<Vector3Int, TileInfo> TileInfos = new();
    
    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        
        TileMap = FindObjectOfType<Tilemap>();

        if (Tiles.Count == 0)
        {
            Tiles.Add(new TileTypeList(TileTypes.Clouds));
            Tiles.Add(new TileTypeList(TileTypes.Dirt));
            Tiles.Add(new TileTypeList(TileTypes.HeavyDirt));
            Tiles.Add(new TileTypeList(TileTypes.Granite));
        }

        TileMap.CompressBounds();
        var row = -1;
        var column = -1;
        var lastrow = 0;

        for (var y = TileMap.cellBounds.max.y - 1; y >= TileMap.cellBounds.min.y; y--)
        {
            row++;
            for (var x = TileMap.cellBounds.min.x; x < TileMap.cellBounds.max.x; x++)
            {
                column++;
                for (var z = TileMap.cellBounds.min.z; z < TileMap.cellBounds.max.z; z++)
                {
                    if (lastrow < row)
                        column = 0;
                    lastrow = row;

                    var position = new Vector3Int(x, y, z);
                    //Debug.Log($"Processing Tile {i++}, Pos: {position}, Row: {row} Col: {column}");
                    TileMap.SetTileFlags(position, TileFlags.None);

                    var tile = (Tile) TileMap.GetTile(position);
                    TileInfos.Add(position, new TileInfo(position, TileMap.GetCellCenterWorld(position), row, column, tile));

                    //TileMap.SetColor(position, Color.red);
                }
            }
        }
        Debug.Log($"Processed {TileInfos.Count} Tiles.");
    }

    public void CreateInteractableObjects()
    {
        var t = GetTileInfo(1, 3);
        var obj = Instantiate(InteractableObjectPrefab, t.Center, Quaternion.identity, TileMap.transform);
        obj.GetComponent<InteractableObjectController>().Type = InteractableObjectTypes.Miner;
        obj.SetActive(true);
        t.InteractableObjects.Add(obj);

        t = GetTileInfo(1, 4);
        obj = Instantiate(InteractableObjectPrefab, t.Center, Quaternion.identity, TileMap.transform);
        obj.GetComponent<InteractableObjectController>().Type = InteractableObjectTypes.Excavator;
        obj.SetActive(true);
        t.InteractableObjects.Add(obj);
    }

    public TileInfo GetTileInfo(int row, int column)
    {
        return TileInfos.FirstOrDefault(x => x.Value.Row == row && x.Value.Column == column).Value;
    }

    public TileTypes GetTileType(Tile tile)
    {
        foreach (var list in Tiles)
        {
            if (list.Tiles.Contains(tile))
                return list.TileType;
        }

        return TileTypes.None;
    }

    public TileTypes GetTileType(Vector3Int position)
    {
        return GetTileType((Tile)TileMap.GetTile(position));
    }

    public Sprite GetInteractableObjectScprite(InteractableObjectTypes interactableObject)
    {
        return InteractableObjectMappings.FirstOrDefault(x => x.Type == interactableObject)?.Sprite;
    }
}
