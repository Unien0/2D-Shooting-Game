using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.NetworkRoom;

public class CustomNetworkRoomManager : NetworkRoomManagerExt
{
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {

        PlayerState playerstate = gamePlayer.GetComponent<PlayerState>();
        playerstate.playerId = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        return true;
    }
}
