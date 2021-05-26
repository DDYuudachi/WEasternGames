using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
   [SerializeField]
   private Vector3 movement = new Vector3();

   [Client]
   private void Update()
   {
      //Will exit update if the client hasn't authority to run
      if (!hasAuthority)
      {
         return;
      }

      if (!Input.GetKeyDown(KeyCode.Space))
      {
         return;
      }
      transform.Translate(movement);
   }

   [Command]
   private void CmdMove()
   {
      //Validation would happen here
      //This would cause jerky movement rather than smooth synced movement
      RpcMove();
   }

   [ClientRpc]
   private void RpcMove() => transform.Translate(movement);
}
