using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

namespace Multiplayer
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        
        private static List<Transform> spawnPoints = new List<Transform>();

        private int nextIndex = 0;

        public static void AddSpawnPoint(Transform transform)
        {
            spawnPoints.Add(transform);

            spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        }

        public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    }
}