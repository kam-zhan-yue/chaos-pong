using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkManagerChaosPong : NetworkManager
{
    public Player prefab;
    public Team redTeam;
    public Team blueTeam;
    public List<Player> players = new List<Player>();

    public static new NetworkManagerChaosPong singleton { get; private set; }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        singleton = this;
    }
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // add player at correct spawn position
        Transform start = numPlayers == 0 ? redTeam.transform : blueTeam.transform;
        Player player = Instantiate(prefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player.gameObject);
        
        players.Add(player);
        if (numPlayers == 1)
        {
            GrantServe(player);
        }
    }


    /// <summary>
    /// Called on the server when a client disconnects.
    /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        if(players.Contains())
    }

    public void GrantServe(Player player)
    {
        
    }

    public void ServeBall()
    {
        
    }
}
