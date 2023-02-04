using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public Tilemap Tilemap;
    public GridLayout Grid;

    private void Awake()
    {
       
    }

    private readonly Vector3Int[] Directions = 
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left,
    
        // if you also wanted to get diagonal neighbours
        //Vector3Int.up + Vector3Int.right,
        //Vector3Int.up + Vector3Int.left,
        //Vector3Int.down + Vector3Int.right,
        //Vector3Int.down + Vector3Int.left
    };

    public List<TileBase> FindAllTileNeighbors(Vector2 gameOjectPosition)
    {  
        var grid = Tilemap.GetComponentInParent<GridLayout>();
        var gridPosition = grid.WorldToCell(gameOjectPosition);
    
        if(!Tilemap.HasTile(gridPosition))
        {
            Debug.LogWarning($"The position {gridPosition} does not exist in the map!");
            return new List<TileBase>();
        }

        var tiles = new List<TileBase>();
        foreach(var d in Directions)
        {
            var position = gridPosition + d;

            if(Tilemap.HasTile(position))
            {
                var neighbour = Tilemap.GetTile(position);
                tiles.Add(neighbour);
            }
        }
        return tiles;
        //  return (neighbourPositions.Select(neighbourPosition => gridPosition + neighbourPosition).Where(position => Tilemap.HasTile(position)).Select(position => Tilemap.GetTile(position))).ToList();
    } 
}
