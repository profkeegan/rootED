using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public Vector3Int Position;

    private bool _isFacingRight = true;

    private void Update()
    {
        var direction = Vector3Int.zero;
        var movement = 0f;

        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector3Int.right;
            movement = 1f;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector3Int.left;
            movement = -1f;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector3Int.up;
            movement = -1f;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector3Int.down;
            movement = 1f;
        }

        var canMove = Cache.Instance.CanPlayerMove(direction);

        if (canMove)
        {
            if((direction == Vector3Int.right && !_isFacingRight) || (direction == Vector3Int.left && _isFacingRight))
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
