using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {
    moveU = 0,
    moveR,
    moveD,
    moveL,
    explore,
    teleport,
    vent,
    flipTimer,
}

public struct PlayerData {
    public PlayerData(ushort id) {
        Id = id;
        Username = "";
    }
    public ushort Id { get; private set; }
    public string Username { get; set; }
}

public class PlayerController {
    public static Dictionary<ushort, PlayerData> playerList = new Dictionary<ushort, PlayerData>();
    public static Dictionary<ushort, PlayerData> connectedPlayerList = new Dictionary<ushort, PlayerData>();

    public static void PlayerConnected(ushort id) {
        connectedPlayerList.Add(id, new PlayerData(id));
    }

    public static void PlayerDisconnected(ushort id) {
        connectedPlayerList.Remove(id);
        if (playerList.ContainsKey(id)) {
            SendPlayerDisconnected(id);
            playerList.Remove(id);
        }
        if (connectedPlayerList.ContainsKey(id)) {
            connectedPlayerList.Remove(id);
        }
        if(playerList.Count == 0) {
            GameController.RenderWaitingScene();
        }
    }

    public static void AddPlayer(ushort id, string username) {
        foreach (PlayerData oldPlayer in playerList.Values) {
            SendPlayerConnectedTo(id, oldPlayer.Id, oldPlayer.Username);
        }
        SendCurrentGameStateTo(id);

        PlayerData p = connectedPlayerList[id];
        p.Username = username;
        playerList[id] = p;

        SendPlayerConnected(id, username);

        GameController.playerJoined(id);
    }

    public static void splitActions() {
        int playerCount = playerList.Count;
        ActionType[][] actions = Collections.GetActionTypes(playerCount);

        int i = 0;
        foreach (PlayerData player in playerList.Values) {
            GameController.addPlayerActions(player.Id, actions[i]);
            SendPlayerActions(player.Id, actions[i]);

            i++;
        }
    }

    #region Messages

    public static void SendPlayerConnected(ushort id, string username) {
        Message message = Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.playerConnected);

        message.AddUShort(id);
        message.AddString(username);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void SendPlayerConnectedTo(ushort sendToId, ushort id, string username) {
        Message message = Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.playerConnected);

        message.AddUShort(id);
        message.AddString(username);

        NetworkManager.Singleton.Server.Send(message, sendToId);
    }

    public static void SendPlayerDisconnected(ushort id) {
        Message message = Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.playerDisconnected);

        message.AddUShort(id);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void SendCurrentGameStateTo(ushort id) {
        switch (GameController.currentScene) {
            case GameScene.waiting: 
                GameController.sendGamestateWaitingTo(id);
                break;
            case GameScene.explore:
                GameController.sendGamestateExploreTo(id);
                GameController.SendGameBoardTo(id);
                break;
            case GameScene.escape:
                GameController.sendGamestateEscapeTo(id);
                GameController.SendGameBoardTo(id);
                break;
        }
    }

    public static void SendPlayerActions(ushort id, ActionType[] actions) {
        Message message = Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.playerActions);

        ushort[] actionList = new ushort[actions.Length];
        for(int i = 0; i < actions.Length; i++) {
            actionList[i] = (ushort)actions[i];
        }

        message.AddUShort(id);
        message.AddUShorts(actionList);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    [MessageHandler((ushort)ClientToServerId.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        AddPlayer(fromClientId, message.GetString());
    }

    #endregion
}
