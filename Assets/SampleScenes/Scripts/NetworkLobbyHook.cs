using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook 
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        RobotInit robotinit = gamePlayer.GetComponent<RobotInit>();
        robotinit.playername = lobby.playerName;
        robotinit.robotinitcolor = lobby.playerColor;
        robotinit.robotname = lobby.playerRobot;

    }
}
