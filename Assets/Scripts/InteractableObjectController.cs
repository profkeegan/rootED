using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectController : MonoBehaviour
{
   public InteractableObjectTypes Type;
   public GameObject InteractBalloon;

   private bool _canPickUp = false;
   private bool _isBeingCarried = false;

   private void OnCollisionEnter2D(Collision2D col)
   {
      if (col.gameObject.GetComponent<PlayerController>() != null)
      {
         _canPickUp = true;
         Debug.Log("Press E to pick up!");
      }
   }

   private void OnCollisionExit2D(Collision2D other)
   {
      if (other.gameObject.GetComponent<PlayerController>() != null)
      {
         _canPickUp = false;
         Debug.Log("Stop pick up!");
      }
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.E))
      {
         if (_canPickUp && !_isBeingCarried)
         {
            
         }
         else if (_isBeingCarried)
         {
            
         }
      }
   }
}
