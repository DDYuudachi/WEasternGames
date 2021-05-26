using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Mirror.Discovery;
using UnityEngine;

public class CustomNetworkDiscovery : NetworkDiscovery
{
    private static CustomNetworkDiscovery instance;

    public static CustomNetworkDiscovery Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CustomNetworkDiscovery>();
            }

            return instance;
        }
    }

}
