using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableObjectController : MonoBehaviour
{
   public InteractableObjectTypes Type;
   public GameObject InteractBalloon;
   public SpriteRenderer SpriteRenderer;

   private bool _canPickUp = false;
   private bool _isBeingCarried = false;

   private void OnTriggerEnter2D(Collider2D col)
   {
       if (col.gameObject.GetComponent<PlayerController>() != null)
       {
           _canPickUp = true;
           Debug.Log("Press E to pick up!");
       }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
       if (other.gameObject.GetComponent<PlayerController>() != null)
       {
           _canPickUp = false;
           Debug.Log("Stop pick up!");
       }
   }

   private void OnEnable()
   {
       SpriteRenderer.sprite = Cache.Instance.GetInteractableObjectScprite(Type);
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.E))
      {
         if (_canPickUp && !_isBeingCarried)
         {
             var o = this.gameObject;
             o.transform.parent = PlayerController.Instance.transform;
             o.transform.localPosition = new Vector3(0.5f, 0);
             o.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
             o.transform.Rotate(0, 0, 25);
             _isBeingCarried = true;
         }
         else if (_isBeingCarried)
         {
             this.gameObject.transform.position = PlayerController.Instance.GetCurrentTile().Center;
             this.gameObject.transform.parent = PlayerController.Instance.TileMap.transform;
         }
      }
   }
}
