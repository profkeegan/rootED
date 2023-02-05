using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public Cache Cache;
    
    public Grid Grid;
    public Tilemap TileMap;

    public GameObject CarriedItem;
    private bool _isFacingRight = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        Instance.Cache.CreateInteractableObjects();
    }

    private void Update()
    {
        var direction = Vector3Int.zero;
        var movement = 0f;

        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector3Int.right;
            movement = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector3Int.left;
            movement = -1f;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector3Int.up;
            movement = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector3Int.down;
            movement = -1f;
        }

        if (direction == Vector3Int.zero)
            return;

        if ((direction == Vector3Int.up || direction == Vector3Int.down) && CarriedItem != null)
        {
            Debug.Log("Up/Down restricted, carrying item..");
            return;
        }

        var info = GetNeighbouringTile(direction);
        var canMove = !info.IsBlocked;
        Debug.Log($"CanPlayerMove Tile: Type: {info.Type}, Row: {info.Row}, Column: {info.Column}, Blocked: {info.IsBlocked}");
       
        if (canMove)
        {
            if (direction == Vector3Int.right && !_isFacingRight || direction == Vector3Int.left && _isFacingRight)
                Flip();

            if (direction == Vector3Int.left || direction == Vector3Int.right)
                this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x + movement, this.gameObject.transform.position.y);
            else
                this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + movement);
        }
    }

    public TileInfo GetNeighbouringTile(Vector3Int direction)
    {
        var gridPosition = Grid.WorldToCell(gameObject.transform.position);
        var position = gridPosition + direction;

        if (!Cache.Instance.TileMap.HasTile(gridPosition))
            throw new Exception($"The position {gridPosition} does not exist on the map!");

        if(!Cache.Instance.TileInfos.ContainsKey(position))
            throw new Exception($"The position {position} does not exist in TileInfos!");

        return Cache.Instance.TileInfos[position];
    }

    public TileInfo GetNextTile()
    {
        return GetNeighbouringTile(_isFacingRight ? Vector3Int.right : Vector3Int.left);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;

        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public TileInfo GetCurrentTile()
    {
        var gridPosition = Grid.WorldToCell(gameObject.transform.position);
        if (Cache.Instance.TileInfos.ContainsKey(gridPosition))
            return Cache.Instance.TileInfos[gridPosition];

        throw new Exception("Cannot find current TileInfo!");
    }
}