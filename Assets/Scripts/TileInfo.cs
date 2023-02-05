using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInfo
{
    public Vector3Int Position;
    public Vector3 Center;
    public TileTypes Type => Cache.Instance.GetTileType(Position);

    public int Row;
    public int Column;
    
    public List<GameObject> InteractableObjects;
    public bool IsBlocked => Type == TileTypes.Granite;

    public float MovementSpeed
    {
        get
        {
            switch (Type)
            {
                case TileTypes.None:
                case TileTypes.Granite:
                    return 0f;

                case TileTypes.Clouds:
                    return 80f;
                case TileTypes.Dirt:
                    return 100f;
                case TileTypes.HeavyDirt:
                    return 70f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public TileInfo(Vector3Int position, Vector3 center, int row, int column, Tile tile)
    {
        Position = position;
        Center = center;
        Row = row;
        Column = column;

        InteractableObjects = new List<GameObject>();

        this.tile = tile;
    }

    private Tile tile;

}
