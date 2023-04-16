using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public enum GameScene {
    notStarted,
    waiting,
    explore,
    escape,
    end,
}

public class GameController {
    public static ushort minNumPlayers = 1;
    public static ushort maxNumPlayers = 8;
    public static GameScene currentScene = GameScene.notStarted;
    private static GameBoard gameBoard;

    public static void RenderWaitingScene() {
        currentScene = GameScene.waiting;
        sendGamestateWaiting();
    }

    public static void RenderExploreScene() {
        currentScene = GameScene.explore;
        gameBoard = new GameBoard();

        PlayerController.splitActions();

        Room room = Collections.getRoom(0, 0, 0, 0);
        addRoom(room);

        addCharacter(1, 1, CharacterClass.Alchemist);
        addCharacter(2, 1, CharacterClass.Archer);
        addCharacter(1, 2, CharacterClass.Swordsman);
        addCharacter(2, 2, CharacterClass.Warrior);

        sendGamestateExplore();

        gameBoard.startTimer();
    }

    public static void RenderEscapeScene() {
        currentScene = GameScene.escape;
        sendGamestateEscape();
    }

    public static void RenderEndScene(bool won) {
        currentScene = GameScene.end;
        gameBoard.pauseTimer();
        sendGamestateEnded(won);
    }

    public static void playerJoined(ushort id) {
        
    }

    public static void addPlayerActions(ushort id, ActionType[] actions) {
        gameBoard.addPlayerActions(id, actions);
    }

    private static void addRoom(Room room) {
        gameBoard.addRoom(room);
        sendRoomPlaced(room);
    }

    private static void addCharacter(short x, short y, CharacterClass c) {
        ushort ind = gameBoard.addCharacter(x, y, c);
        sendCharacterSpawned(ind, x, y, c);
    }

    public static void fixedUpdate() {
        gameBoard.fixedUpdate();
    }

    #region Messages

    [MessageHandler((ushort)ClientToServerId.moveCharacter)]
    private static void getMoveCharacter(ushort fromClientId, Message message) {
        ushort ind = message.GetUShort();
        short startX = message.GetShort();
        short startY = message.GetShort();
        short endX = message.GetShort();
        short endY = message.GetShort();

        gameBoard.moveCharacter(fromClientId, ind, startX, startY, endX, endY);
    }

    [MessageHandler((ushort)ClientToServerId.explore)]
    private static void getExplore(ushort fromClientId, Message message) {
        short x = message.GetShort();
        short y = message.GetShort();

        gameBoard.explore(fromClientId, x, y);
    }

    [MessageHandler((ushort)ClientToServerId.teleport)]
    private static void getTeleport(ushort fromClientId, Message message) {
        ushort ind = message.GetUShort();
        short startX = message.GetShort();
        short startY = message.GetShort();
        short endX = message.GetShort();
        short endY = message.GetShort();

        gameBoard.teleportCharacter(fromClientId, ind, startX, startY, endX, endY);
    }

    [MessageHandler((ushort)ClientToServerId.vent)]
    private static void getVent(ushort fromClientId, Message message) {
        ushort ind = message.GetUShort();
        short startX = message.GetShort();
        short startY = message.GetShort();
        short endX = message.GetShort();
        short endY = message.GetShort();

        gameBoard.ventCharacter(fromClientId, ind, startX, startY, endX, endY);
    }

    [MessageHandler((ushort)ClientToServerId.flipTimer)]
    private static void getFlipTimer(ushort fromClientId, Message message) {
        ushort ind = message.GetUShort();
        short X = message.GetShort();
        short Y = message.GetShort();

        if (gameBoard.tryFlipTimer(ind, X, Y)) {
            SendTimerFlipped(X, Y, (short)gameBoard.timerTick);
        }
    }

    [MessageHandler((ushort)ClientToServerId.startGame)]
    private static void getStartGame(ushort fromClientId, Message message) {
        if (currentScene == GameScene.waiting && minNumPlayers <= PlayerController.playerList.Count) {
            RenderExploreScene();
        }
    }

    [MessageHandler((ushort)ClientToServerId.ping)]
    private static void getPing(ushort fromClientId, Message message) {
        SendPinged(message.GetUShort());
    }

    public static void sendRoomPlaced(Room room) {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.roomPlaced);

        message.AddRoom(room);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void sendCharacterSpawned(ushort ind, short x, short y, CharacterClass c) {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.characterSpawned);

        message.AddUShort(ind);
        message.AddShort(x);
        message.AddShort(y);
        message.AddUShort((ushort)c);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void sendCharacterMoved(ushort ind, short x, short y) {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.characterMoved);

        message.AddUShort(ind);
        message.AddShort(x);
        message.AddShort(y);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void sendGateRemoved(short x, short y) {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.gateRemoved);

        message.AddShort(x);
        message.AddShort(y);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void sendTileChanged(short x, short y, ushort t) {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.tileChanged);

        message.AddShort(x);
        message.AddShort(y);
        message.AddUShort(t);

        NetworkManager.Singleton.Server.SendToAll(message);
    }
    public static void sendGamestateWaiting() {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.gamestateWaiting);

        message.AddInt(PlayerController.playerList.Count);
        message.AddInt(minNumPlayers);
        message.AddInt(maxNumPlayers);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void sendGamestateExplore() {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.gamestateExplore);

        message.AddInt(GameBoard.gameLengthInTicks);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void sendGamestateEscape() {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.gamestateEscape);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void sendGamestateEnded(bool won) {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.gamestateEnded);

        message.AddBool(won);

        NetworkManager.Singleton.Server.SendToAll(message);
    }
    public static void sendGamestateWaitingTo(ushort id) {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.gamestateWaiting);

        message.AddInt(PlayerController.playerList.Count);
        message.AddInt(minNumPlayers);
        message.AddInt(maxNumPlayers);

        NetworkManager.Singleton.Server.Send(message, id);
    }

    public static void sendGamestateExploreTo(ushort id) {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.gamestateExplore);

        message.AddInt(GameBoard.gameLengthInTicks);

        NetworkManager.Singleton.Server.Send(message, id);
    }

    public static void sendGamestateEscapeTo(ushort id) {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.gamestateEscape);

        NetworkManager.Singleton.Server.Send(message, id);
    }

    public static void SendSync() {
        Message message = Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.syncClock);

        message.AddFloat(gameBoard.timerTick);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void SendTimerFlipped(short x, short y, short tick) {
        Message message = Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.timerFlipped);

        message.AddShort(x);
        message.AddShort(y);
        message.AddShort(tick);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    public static void SendGameBoardTo(ushort id) {
        foreach (Room room in gameBoard.roomList) {
            Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.roomPlaced);

            message.AddRoom(room);

            NetworkManager.Singleton.Server.Send(message, id);
        }
        foreach(Character c in gameBoard.characterList) {
            Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.characterSpawned);

            message.AddUShort((ushort)c.Ind);
            message.AddShort((short)c.X);
            message.AddShort((short)c.Y);
            message.AddUShort((ushort)c.C);

            NetworkManager.Singleton.Server.Send(message, id);
        }
    }

    public static void SendPinged(ushort id) {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.pinged);

        message.AddUShort(id);

        NetworkManager.Singleton.Server.SendToAll(message);
        
    }

    #endregion
}
