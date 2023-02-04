using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    public Vector3Int Position;
    public Grid Grid;

    private bool _isFacingRight = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        Position = new Vector3Int(1, 1);
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

        var gridPosition = Grid.WorldToCell(gameObject.transform.position);

        if (!Cache.Instance.TileMap.HasTile(gridPosition))
        {
            Debug.LogWarning($"The position {gridPosition} does not exist in the map!");
            return;
        }

        var position = gridPosition + direction;
        bool canMove;
        if (Cache.Instance.TileInfos.ContainsKey(position))
        {
            var info = Cache.Instance.TileInfos[position];
            Debug.Log($"CanPlayerMove Tile: Type: {info.Type}, Row: {info.Row}, Column: {info.Column}, Blocked: {info.IsBlocked}");
            canMove = !info.IsBlocked;
        }
        else
            canMove = false;

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

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;

        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}