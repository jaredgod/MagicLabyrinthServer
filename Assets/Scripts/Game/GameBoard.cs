using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterClass
{
    Alchemist,
    Archer,
    Swordsman,
    Warrior
}
public struct CharacterPortalData {
    public int x;
    public int y;
    public CharacterClass characterClass;
}
public struct CharacterItemData {
    public int x;
    public int y;
    public CharacterClass characterClass;
}
public struct CharacterExitData {
    public int x;
    public int y;
    public CharacterClass characterClass;
}
public struct LinkedPortalData {
    public int x;
    public int y;
    public int lx;
    public int ly;
}

public struct TimerData {
    public int x;
    public int y;
    public bool active;
}

public struct GateData
{
    public ushort r;
    public CharacterClass c;
}

public class GameBoard {

    private Dictionary<Vector2Int, int> tileMap = new Dictionary<Vector2Int, int>();
    private Dictionary<Vector2Int, GateData> gateMap = new Dictionary<Vector2Int, GateData>();
    private Dictionary<Vector2Int, Character> charMap = new Dictionary<Vector2Int, Character>();
    public Dictionary<Vector2Int, CharacterPortalData> portalMap = new Dictionary<Vector2Int, CharacterPortalData>();
    public Dictionary<Vector2Int, CharacterItemData> itemMap = new Dictionary<Vector2Int, CharacterItemData>();
    public Dictionary<Vector2Int, CharacterExitData> exitMap = new Dictionary<Vector2Int, CharacterExitData>();
    public Dictionary<Vector2Int, LinkedPortalData> lportalMap = new Dictionary<Vector2Int, LinkedPortalData>();
    public Dictionary<Vector2Int, TimerData> timerMap = new Dictionary<Vector2Int, TimerData>();

    public List<Character> characterList = new List<Character>();
    public List<Room> roomList = new List<Room>();

    public Dictionary<ushort, List<ActionType>> playerActions = new Dictionary<ushort, List<ActionType>>();


    private static int gameLengthInSeconds = 3 * 60;
    private static int ticksPerSecond = 60;
    public static int gameLengthInTicks = gameLengthInSeconds * ticksPerSecond;
    
    public float timerTick = gameLengthInTicks;
    private bool timePaused = true;
    private float timeFlowScalar = 1;


    ushort roomInd = 1;

    #region Add Components
    public void addRoom(Room room)
    {
        roomList.Add(room);

        for (int i = 0; i < room.RoomSize; i++)
        {
            for (int j = 0; j < room.RoomSize; j++)
            {
                (int x, int y) = rotate(i, j, room.Rotation);
                int t = ((int)room.Tiles[i][j]);
                for(int ind = 0; ind < room.Rotation; ind++)
                {
                    t = t * 2;
                    t = t > 15 ? t + 1 : t;
                    t = t % 16;
                }

                tileMap.Add(new Vector2Int(room.X + x, room.Y + y), t);
            }
        }

        int gateInd = (4 - room.Rotation) % 4;
        if (room.Gates.Length > gateInd && room.Gates[gateInd] >= 0) addGate(room.X, room.Y + 2, 3, (CharacterClass)room.Gates[gateInd]);

        gateInd = (gateInd + 1) % 4;
        if (room.Gates.Length > gateInd && room.Gates[gateInd] >= 0) addGate(room.X + 2, room.Y + 3, 0, (CharacterClass)room.Gates[gateInd]); 

        gateInd = (gateInd + 1) % 4;
        if (room.Gates.Length > gateInd && room.Gates[gateInd] >= 0) addGate(room.X + 3, room.Y + 1, 1, (CharacterClass)room.Gates[gateInd]); 

        gateInd = (gateInd + 1) % 4;
        if (room.Gates.Length > gateInd && room.Gates[gateInd] >= 0) addGate(room.X + 1, room.Y, 2, (CharacterClass)room.Gates[gateInd]);

        placeItems(room.Items, room.X, room.Y, room.Rotation);
    }

    private void placeItems(short[][] portals, int rx, int ry, int rr) {
        bool lpd1Set = false;
        bool lpd2Set = false;

        LinkedPortalData lpd1;
        lpd1.x = 0;
        lpd1.y = 0;
        LinkedPortalData lpd2;
        lpd2.x = 0;
        lpd2.y = 0;

        foreach (short[] itemData in portals) {
            (int x, int y) = rotate(itemData[0], itemData[1], rr);
            x += rx;
            y += ry;

            if ((ItemType)itemData[2] == ItemType.FirstBridge) {
                if(!lpd1Set) {
                    lpd1.x = x;
                    lpd1.y = y;
                    lpd1Set = true;
                }
                else {
                    lpd1.lx = x;
                    lpd1.ly = y;

                    LinkedPortalData temp;
                    temp.x = x;
                    temp.y = y;
                    temp.lx = lpd1.x;
                    temp.ly = lpd1.y;

                    lportalMap.Add(new Vector2Int(lpd1.x, lpd1.y), lpd1);
                    lportalMap.Add(new Vector2Int(temp.x, temp.y), temp);
                }
            }
            else if ((ItemType)itemData[2] == ItemType.SecondBridge) {
                if (!lpd2Set) {
                    lpd2.x = x;
                    lpd2.y = y;
                    lpd2Set = true;
                }
                else {
                    lpd2.lx = x;
                    lpd2.ly = y;

                    LinkedPortalData temp;
                    temp.x = x;
                    temp.y = y;
                    temp.lx = lpd2.x;
                    temp.ly = lpd2.y;

                    lportalMap.Add(new Vector2Int(lpd2.x, lpd2.y), lpd2);
                    lportalMap.Add(new Vector2Int(temp.x, temp.y), temp);
                }
            }
            else if ((ItemType)itemData[2] <= ItemType.WarriorPortal) {
                CharacterPortalData d;
                d.x = x;
                d.y = y;
                d.characterClass = (CharacterClass)(itemData[2] - ItemType.AlchemistPortal);

                portalMap.Add(new Vector2Int(d.x, d.y), d);
            }
            else if ((ItemType)itemData[2] <= ItemType.WarriorItem) {
                CharacterItemData d;
                d.x = x;
                d.y = y;
                d.characterClass = (CharacterClass)(itemData[2] - ItemType.AlchemistItem);

                itemMap.Add(new Vector2Int(d.x, d.y), d);
            }
            else if ((ItemType)itemData[2] <= ItemType.WarriorExit) {
                CharacterExitData d;
                d.x = x;
                d.y = y;
                d.characterClass = (CharacterClass)(itemData[2] - ItemType.AlchemistExit);

                exitMap.Add(new Vector2Int(d.x, d.y), d);
            }
            else if ((ItemType)itemData[2] == ItemType.Timer) {
                TimerData p;
                p.x = x;
                p.y = y;
                p.active = true;

                timerMap.Add(new Vector2Int(p.x, p.y), p);
            }
        }
    }

    public ushort addCharacter(short x, short y, CharacterClass c)
    {
        ushort ind = (ushort)characterList.Count;
        Character character = new Character(ind, x, y, c);
        characterList.Add(character);
        charMap.Add(new Vector2Int(x, y), character);

        return ind;
    }

    private (int, int) rotate(int x, int y, int rotation)
    {
        if (rotation == 0) return (x, y);
        if (rotation == 1) return (y, 3 - x);
        if (rotation == 2) return (3 - x, 3 - y);
        if (rotation == 3) return (3 - y, x);
        return (0, 0);
    }

    private void addGate(int x, int y, ushort r, CharacterClass c)
    {
        GateData data;
        data.r = r;
        data.c = c;

        gateMap.Add(new Vector2Int(x, y), data);
    }

    public void addPlayerActions(ushort id, ActionType[] actions) {
        List<ActionType> a = new List<ActionType>();

        a.AddRange(actions);

        playerActions.Add(id, a);
    }

    #endregion

    #region timer

    public void fixedUpdate() {
        if (timePaused) return;
        incrementTimer();
    }

    public void incrementTimer() {
        timerTick -= timeFlowScalar;
        if (timerTick <= 0) outOfTime();
    }

    public void pauseTimer() {
        timePaused = true;
    }

    public void startTimer() {
        timePaused = false;
    }

    public void flipTimer() {
        timerTick = gameLengthInTicks - timerTick;
    }

    private void outOfTime() {
        GameController.RenderEndScene(false);
    }

    #endregion

    #region Board Actions

    public void moveCharacter(ushort id, ushort ind, short startX, short startY, short endX, short endY)
    {
        Vector2Int pos = new Vector2Int(startX, startY);
        Vector2Int endPos = new Vector2Int(endX, endY);
        Vector2Int diff = endPos - pos;

        if (diff.x != 0 && diff.y != 0) return;
        if (diff.x != 0) {
            if (diff.x > 0 && !playerActions[id].Contains(ActionType.moveR)) return;
            if (diff.x < 0 && !playerActions[id].Contains(ActionType.moveL)) return;
        }
        if (diff.y != 0) {
            if (diff.y > 0 && !playerActions[id].Contains(ActionType.moveU)) return;
            if (diff.y < 0 && !playerActions[id].Contains(ActionType.moveD)) return;
        }

        if (isPlayScene() && charMap.ContainsKey(pos) && charMap[pos].Ind == ind && !charMap.ContainsKey(endPos))
        {
            Character character = charMap[pos];
            charMap.Remove(pos);
            character.setPos(endX, endY);
            charMap.Add(new Vector2Int(endX, endY), character);

            GameController.sendCharacterMoved((ushort)character.Ind, endX, endY);

            if (GameController.currentScene == GameScene.explore) checkItems();
            if (GameController.currentScene == GameScene.escape) checkExits();
        }
    }
    public void teleportCharacter(ushort id, ushort ind, short startX, short startY, short endX, short endY) {
        Vector2Int pos = new Vector2Int(startX, startY);
        Vector2Int endPos = new Vector2Int(endX, endY);
        if (!playerActions[id].Contains(ActionType.teleport)) return;
        if (GameController.currentScene == GameScene.explore 
            && charMap.ContainsKey(pos) 
            && charMap[pos].Ind == ind 
            && !charMap.ContainsKey(endPos) 
            && portalMap.ContainsKey(pos)
            && portalMap.ContainsKey(endPos)
            && characterList[ind].C == portalMap[pos].characterClass
            && characterList[ind].C == portalMap[endPos].characterClass) {

            Character character = charMap[pos];
            charMap.Remove(pos);
            character.setPos(endX, endY);
            charMap.Add(endPos, character);

            GameController.sendCharacterMoved((ushort)character.Ind, endX, endY);
        }
    }
    public void ventCharacter(ushort id, ushort ind, short startX, short startY, short endX, short endY) {
        Vector2Int pos = new Vector2Int(startX, startY);
        Vector2Int endPos = new Vector2Int(endX, endY);
        if (!playerActions[id].Contains(ActionType.vent)) return;
        if (isPlayScene()
            && charMap.ContainsKey(pos)
            && charMap[pos].Ind == ind
            && !charMap.ContainsKey(endPos)
            && lportalMap.ContainsKey(pos)
            && lportalMap.ContainsKey(endPos)
            && lportalMap[pos].lx == lportalMap[endPos].x
            && lportalMap[pos].ly == lportalMap[endPos].y) {

            Character character = charMap[pos];
            charMap.Remove(pos);
            character.setPos(endX, endY);
            charMap.Add(endPos, character);

            GameController.sendCharacterMoved((ushort)character.Ind, endX, endY);
        }
    }

    public bool tryFlipTimer(ushort ind, short x, short y) {
        Vector2Int pos = new Vector2Int(x, y);
        if (isPlayScene() 
            && charMap.ContainsKey(pos)
            && charMap[pos].Ind == ind
            && timerMap.ContainsKey(pos)
            && timerMap[pos].active == true ) {

            TimerData t = timerMap[pos];
            t.active = false;
            timerMap[pos] = t;

            flipTimer();

            return true;
        }

        return false;
    }

    public void explore(ushort id, short x, short y)
    {
        Vector2Int pos = new Vector2Int(x, y);
        if (!playerActions[id].Contains(ActionType.explore)) return;
        if (isPlayScene() && charMap.ContainsKey(pos) && gateMap.ContainsKey(pos) && charMap[pos].C == gateMap[pos].c)
        {
            GateData g = gateMap[pos];

            short X;
            short Y;

            if (g.r == 0)
            {
                X = -1;
                Y = 1;
            }
            else if (g.r == 1)
            {
                X = 1;
                Y = -2;
            }
            else if (g.r == 2)
            {
                X = -2;
                Y = -4;
            }
            else if (g.r == 3)
            {
                X = -4; 
                Y = -1;
            }
            else return;

            X += x;
            Y += y;

            Room room = Collections.getRoom(roomInd, X, Y, g.r);
            roomInd++;

            addRoom(room);
            GameController.sendRoomPlaced(room);


            gateMap.Remove(pos);
            GameController.sendGateRemoved((short)pos.x, (short)pos.y);
            
            Vector2Int gatePos = new Vector2Int(X + 2, Y + 3);
            Vector2Int checkPos = new Vector2Int(X + 2, Y + 4);

            removeGates(gatePos, checkPos, 1);

            gatePos = new Vector2Int(X + 3, Y + 1);
            checkPos = new Vector2Int(X + 4, Y + 1);

            removeGates(gatePos, checkPos, 2);

            gatePos = new Vector2Int(X + 1, Y + 0);
            checkPos = new Vector2Int(X + 1, Y -1);

            removeGates(gatePos, checkPos, 4);

            gatePos = new Vector2Int(X + 0, Y + 2);
            checkPos = new Vector2Int(X - 1, Y + 2);

            removeGates(gatePos, checkPos, 8);
        }
    }

    private void removeGates(Vector2Int gatePos, Vector2Int checkPos, int addToTile)
    {
        if (gateMap.ContainsKey(gatePos))
        {
            if (tileMap.ContainsKey(checkPos))
            {
                gateMap.Remove(gatePos);
                GameController.sendGateRemoved((short)gatePos.x, (short)gatePos.y);
                if (gateMap.ContainsKey(checkPos))
                {
                    gateMap.Remove(checkPos);
                    GameController.sendGateRemoved((short)checkPos.x, (short)checkPos.y);
                }
                else if (tileMap[checkPos] % (addToTile * 2) >= (addToTile - 1))
                {
                    tileMap[gatePos] += addToTile;
                    GameController.sendTileChanged((short)gatePos.x, (short)gatePos.y, (ushort)tileMap[gatePos]);
                }
            }
        }
        else if (gateMap.ContainsKey(checkPos))
        {
            gateMap.Remove(checkPos);
            GameController.sendGateRemoved((short)checkPos.x, (short)checkPos.y);
            int t = addToTile * 2;
            t = t > 15 ? t + 1 : t;
            t = t % 16;
            t *= 2;
            t = t > 15 ? t + 1 : t;
            t = t % 16;

            Debug.Log($"x:{checkPos.x}, y:{checkPos.y}, t:{t}");

            tileMap[checkPos] += t;
            GameController.sendTileChanged((short)checkPos.x, (short)checkPos.y, (ushort)tileMap[checkPos]);
            
        }
    }

    public void checkItems() {
        bool ret = true;

        foreach (Character c in characterList) {
            ret = ret && itemMap.ContainsKey(new Vector2Int(c.X, c.Y))
                && itemMap[new Vector2Int(c.X, c.Y)].characterClass == c.C;
        }

        if (ret) GameController.RenderEscapeScene();
    }

    public void checkExits() {
        bool ret = true;

        foreach (Character c in characterList) {
            ret = ret && itemMap.ContainsKey(new Vector2Int(c.X, c.Y))
                && exitMap[new Vector2Int(c.X, c.Y)].characterClass == c.C;
        }
        Debug.Log($"Exits: {ret}");

        if (ret) GameController.RenderEndScene(true);
    }

    private bool isPlayScene() {
        return GameController.currentScene == GameScene.explore || GameController.currentScene == GameScene.escape;
    }

    #endregion

}
