using UnityEngine;
using Mirror;

namespace Multiplayer
{
    public class CustomNetworkManager : NetworkManager
    {
        private static CustomNetworkManager instace;
        public static CustomNetworkManager Instace
        {
            get
            {
                if (instace == null)
                {
                    instace = FindObjectOfType<CustomNetworkManager>();
                }
                return instace;
            }
        }

        //public static event Action OnClientConnected;
    }
}
