using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using Unity.Collections;

public class TankPlayer : NetworkBehaviour
{
   [Header("Refferences")] [SerializeField]
   private CinemachineVirtualCamera virtualCamera;

   [field: SerializeField] public Health Health { get; private set; }

   [Header("Settings")] [SerializeField] private int ownerPriority = 15;

   public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
   public NetworkVariable<int> PlayerColorIndex = new NetworkVariable<int>();

   public static event Action<TankPlayer> OnPlayerSpawned;
   public static event Action<TankPlayer> OnPlayerDespawned;
   public override void OnNetworkSpawn()
   {
      if (IsServer)
      {
         UserData userData =
            HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);

         PlayerName.Value = userData.userName;
         PlayerColorIndex.Value = userData.userColorIndex;
         
         OnPlayerSpawned?.Invoke(this);
      }
      if (IsOwner)
      {
         virtualCamera.Priority = ownerPriority;
      }
   }
   
   public override void OnNetworkDespawn()
   {
      if (IsServer)
      {
         OnPlayerDespawned.Invoke(this);
      }
   }

   private void HandlePlayerSpawned(TankPlayer player)
   {
     // player.Health.OnDie += (health == HandlePlayerDie(player));
   }
}
