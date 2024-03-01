using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.NetworkRoom;

public class CustomNetworkRoomManager : NetworkRoomManagerExt
{
    public bool showBackRoomBtm = false;

    public GameObject[] playerPrefabs;
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        // get start position from base class
        Transform startPos = GetStartPosition();
        int index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        GameObject playerObj = startPos != null
                    ? Instantiate(playerPrefabs[index], startPos.position, startPos.rotation)
                    : Instantiate(playerPrefabs[index], Vector3.zero, Quaternion.identity);
        return playerObj;
    }
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {

        PlayerState playerstate = gamePlayer.GetComponent<PlayerState>();
        playerstate.playerId = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        return true;
    }

    public override void OnGUI()
    {
        base.OnGUI();

        if (NetworkServer.active && Utils.IsSceneActive(GameplayScene) && showBackRoomBtm)
        {
            GUILayout.BeginArea(new Rect((Screen.width / 2) - 75f, 750f, 150f, 100f));
            if (GUILayout.Button("¥ë©`¥à¤Ø‘ø¤ë"))
            {
                ServerChangeScene(RoomScene);
                showBackRoomBtm = false;
            }
            GUILayout.EndArea();
        }
    }
}
