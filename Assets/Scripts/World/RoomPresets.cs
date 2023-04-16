using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Collections {
    #region Rooms
    private enum GateClass {
        None = -1,
        Alchemist = 0,
        Archer = 1,
        Swordsman = 2,
        Warrior = 3,

    }
    private static int[] roomOrder = randomList(23);

    public static ushort[][][] roomPreset = new ushort[][][] {
        #region Tile 1
        new ushort[][] {
            new ushort[] { 13, 1, 0, 7 },
            new ushort[] { 5, 0, 0, 7 },
            new ushort[] { 13, 0, 2, 13 },
            new ushort[] { 13, 0, 6, 15 },
        },
        #endregion
        #region Tile 2 
        new ushort[][] {
            new ushort[] { 15, 15, 13, 7 },
            new ushort[] { 3, 15, 15, 15 },
            new ushort[] { 8, 3, 15, 15 },
            new ushort[] { 14, 8, 7, 15 },
        },
        #endregion
        #region Tile 3 
        new ushort[][] {
            new ushort[] { 9, 3, 11, 15 },
            new ushort[] { 6, 8, 4, 7 },
            new ushort[] { 13, 4, 1, 5 },
            new ushort[] { 15, 9, 6, 15 },
        },
        #endregion
        #region Tile 4 
        new ushort[][] {
            new ushort[] { 15, 15, 8, 7 },
            new ushort[] { 15, 11, 10, 15 },
            new ushort[] { 13, 0, 4, 5 },
            new ushort[] { 15, 10, 15, 15 },
        },
        #endregion

        #region Tile 5 
        new ushort[][] {
            new ushort[] { 9, 3, 12, 3 },
            new ushort[] { 6, 10, 11, 10 },
            new ushort[] { 13, 4, 0, 4 },
            new ushort[] { 15, 9, 6, 15 },
        },
        #endregion
        #region Tile 6 
        new ushort[][] {
            new ushort[] { 15, 15, 10, 15 },
            new ushort[] { 5, 5, 2, 15 },
            new ushort[] { 15, 9, 4, 7 },
            new ushort[] { 13, 2, 15, 15 },
        },
        #endregion
        #region Tile 7 
        new ushort[][] {
            new ushort[] { 15, 15, 11, 15 },
            new ushort[] { 9, 5, 4, 3 },
            new ushort[] { 14, 15, 15, 8 },
            new ushort[] { 15, 11, 15, 14 },
        },
        #endregion
        #region Tile 8 
        new ushort[][] {
            new ushort[] { 9, 5, 1, 3 },
            new ushort[] { 4, 15, 14, 10 },
            new ushort[] { 15, 15, 15, 8 },
            new ushort[] { 13, 1, 5, 6 },
        },
        #endregion
        
        #region Tile 9 
        new ushort[][] {
            new ushort[] { 9, 5, 5, 3 },
            new ushort[] { 10, 15, 13, 2 },
            new ushort[] { 12, 3, 15, 8 },
            new ushort[] { 15, 10, 15, 14 },
        },
        #endregion
        #region Tile 10
        new ushort[][] {
            new ushort[] { 13, 1, 7, 15 },
            new ushort[] { 5, 6, 15, 15 },
            new ushort[] { 15, 15, 9, 5 },
            new ushort[] { 13, 1, 6, 15 },
        },
        #endregion
        #region Tile 11
        new ushort[][] {
            new ushort[] { 9, 5, 1, 7 },
            new ushort[] { 4, 3, 10, 15 },
            new ushort[] { 9, 6, 12, 5 },
            new ushort[] { 12, 1, 7, 15 },
        },
        #endregion
        #region Tile 12
        new ushort[][] {
            new ushort[] { 13, 5, 4, 3 },
            new ushort[] { 11, 15, 15, 14 },
            new ushort[] { 12, 1, 7, 15 },
            new ushort[] { 13, 0, 7, 15 },
        },
        #endregion
        
        #region Tile 13
        new ushort[][] {
            new ushort[] { 9, 5, 1, 3 },
            new ushort[] { 6, 11, 14, 10 },
            new ushort[] { 9, 4, 5, 4 },
            new ushort[] { 12, 3, 15, 15 },
        },
        #endregion
        #region Tile 14
        new ushort[][] {
            new ushort[] { 9, 5, 5, 7 },
            new ushort[] { 2, 15, 15, 15 },
            new ushort[] { 10, 13, 1, 5 },
            new ushort[] { 14, 9, 6, 15 },
        },
        #endregion
        #region Tile 15
        new ushort[][] {
            new ushort[] { 15, 13, 4, 7 },
            new ushort[] { 5, 3, 15, 15 },
            new ushort[] { 15, 12, 1, 7 },
            new ushort[] { 13, 1, 6, 15 },
        },
        #endregion
        #region Tile 16
        new ushort[][] {
            new ushort[] { 9, 1, 7, 15 },
            new ushort[] { 2, 8, 5, 3 },
            new ushort[] { 10, 14, 9, 4 },
            new ushort[] { 14, 9, 6, 15 },
        },
        #endregion
        
        #region Tile 17
        new ushort[][] {
            new ushort[] { 15, 11, 15, 11 },
            new ushort[] { 9, 4, 1, 6 },
            new ushort[] { 12, 3, 8, 5 },
            new ushort[] { 15, 10, 14, 15 },
        },
        #endregion
        #region Tile 18
        new ushort[][] {
            new ushort[] { 15, 15, 11, 15 },
            new ushort[] { 5, 3, 10, 15 },
            new ushort[] { 15, 8, 4, 7 },
            new ushort[] { 15, 10, 15, 15 },
        },
        #endregion
        #region Tile 19
        new ushort[][] {
            new ushort[] { 9, 3, 12, 3 },
            new ushort[] { 2, 12, 5, 2 },
            new ushort[] { 8, 1, 7, 8 },
            new ushort[] { 14, 10, 15, 14 },
        },
        #endregion
        #region Tile 20
        new ushort[][] {
            new ushort[] { 15, 9, 3, 15 },
            new ushort[] { 1, 6, 12, 3 },
            new ushort[] { 12, 3, 15, 8 },
            new ushort[] { 15, 10, 15, 14 },
        },
        #endregion
        
        #region Tile 21
        new ushort[][] {
            new ushort[] { 9, 5, 5, 3 },
            new ushort[] { 4, 5, 3, 10 },
            new ushort[] { 9, 5, 6, 8 },
            new ushort[] { 12, 1, 7, 14 },
        },
        #endregion
        #region Tile 22
        new ushort[][] {
            new ushort[] { 9, 3, 9, 3 },
            new ushort[] { 6, 8, 6, 10 },
            new ushort[] { 15, 12, 3, 12 },
            new ushort[] { 13, 1, 6, 15 },
        },
        #endregion
        #region Tile 23
        new ushort[][] {
            new ushort[] { 9, 1, 5, 7 },
            new ushort[] { 2, 10, 15, 15 },
            new ushort[] { 14, 12, 3, 9 },
            new ushort[] { 15, 9, 4, 6 },
        },
        #endregion
        #region Tile 24
        new ushort[][] {
            new ushort[] { 15, 13, 1, 3 },
            new ushort[] { 5, 1, 6, 10 },
            new ushort[] { 15, 10, 15, 8 },
            new ushort[] { 13, 0, 7, 14 },
        },
        #endregion

    };

    public static short[][] gatePreset = new short[][] {
        #region Tile 1
        new short[]
        {
            (short)GateClass.Alchemist,
            (short)GateClass.Warrior,
            (short)GateClass.Archer,
            (short)GateClass.Swordsman,
        },
        #endregion 
        #region Tile 2
        new short[]
        {
            (short)GateClass.Warrior,
            (short)GateClass.None,
            (short)GateClass.None,
        },
        #endregion 
        #region Tile 3
        new short[]
        {
            (short)GateClass.Alchemist,
            (short)GateClass.None,
            (short)GateClass.Swordsman,
        },
        #endregion 
        #region Tile 4
        new short[]
        {
            (short)GateClass.None,
            (short)GateClass.Alchemist,
            (short)GateClass.Archer,
        },
        #endregion 
        
        #region Tile 5
        new short[]
        {
            (short)GateClass.Swordsman,
            (short)GateClass.Warrior,
            (short)GateClass.Archer,
        },
        #endregion 
        #region Tile 6
        new short[]
        {
            (short)GateClass.Warrior,
            (short)GateClass.Archer,
            (short)GateClass.None,
        },
        #endregion 
        #region Tile 7
        new short[]
        {
            (short)GateClass.None,
            (short)GateClass.None,
            (short)GateClass.Alchemist,
        },
        #endregion 
        #region Tile 8
        new short[]
        {
            (short)GateClass.Warrior,
            (short)GateClass.None,
            (short)GateClass.Alchemist,
        },
        #endregion 
        
        #region Tile 9
        new short[]
        {
            (short)GateClass.None,
            (short)GateClass.None,
            (short)GateClass.Swordsman,
        },
        #endregion 
        #region Tile 10
        new short[]
        {
            (short)GateClass.Alchemist,
            (short)GateClass.None,
            (short)GateClass.Swordsman,
        },
        #endregion 
        #region Tile 11
        new short[]
        {
            (short)GateClass.Archer,
            (short)GateClass.None,
            (short)GateClass.Warrior,
        },
        #endregion 
        #region Tile 12
        new short[]
        {
            (short)GateClass.None,
            (short)GateClass.Swordsman,
            (short)GateClass.None,
        },
        #endregion 
        
        #region Tile 13
        new short[]
        {
            (short)GateClass.Archer,
            (short)GateClass.None,
            (short)GateClass.Swordsman,
        },
        #endregion 
        #region Tile 14
        new short[]
        {
            (short)GateClass.Warrior,
            (short)GateClass.None,
            (short)GateClass.Archer,
        },
        #endregion 
        #region Tile 15
        new short[]
        {
            (short)GateClass.Archer,
            (short)GateClass.Warrior,
            (short)GateClass.None,
        },
        #endregion 
        #region Tile 16
        new short[]
        {
            (short)GateClass.Warrior,
            (short)GateClass.None,
            (short)GateClass.Alchemist,
        },
        #endregion 
        
        #region Tile 17
        new short[]
        {
            (short)GateClass.None,
            (short)GateClass.None,
            (short)GateClass.Warrior,
        },
        #endregion 
        #region Tile 18
        new short[]
        {
            (short)GateClass.Archer,
            (short)GateClass.None,
            (short)GateClass.None,
        },
        #endregion 
        #region Tile 19
        new short[]
        {
            (short)GateClass.Swordsman,
            (short)GateClass.Alchemist,
            (short)GateClass.Warrior,
        },
        #endregion 
        #region Tile 20
        new short[]
        {
            (short)GateClass.Archer,
            (short)GateClass.None,
            (short)GateClass.Swordsman,
        },
        #endregion 
        
        #region Tile 21
        new short[]
        {
            (short)GateClass.Alchemist,
            (short)GateClass.None,
            (short)GateClass.Warrior,
        },
        #endregion 
        #region Tile 22
        new short[]
        {
            (short)GateClass.Warrior,
            (short)GateClass.None,
            (short)GateClass.Archer,
        },
        #endregion 
        #region Tile 23
        new short[]
        {
            (short)GateClass.Warrior,
            (short)GateClass.None,
            (short)GateClass.Alchemist,
        },
        #endregion 
        #region Tile 24
        new short[]
        {
            (short)GateClass.Swordsman,
            (short)GateClass.None,
            (short)GateClass.Archer,
        },
        #endregion 

    };



    public static short[][][] itemPreset = new short[][][] {
        #region Tile 1
        new short[][]
        {
            new short[] { 0, 0, (short)ItemType.ArcherPortal },
            new short[] { 0, 1, (short)ItemType.WarriorPortal },
            new short[] { 3, 2, (short)ItemType.SwordsmanPortal },
            new short[] { 3, 3, (short)ItemType.AlchemistPortal },
            new short[] { 2, 0, (short)ItemType.FirstBridge },
            new short[] { 3, 1, (short)ItemType.FirstBridge },
            new short[] { 0, 3, (short)ItemType.Timer },
        },
        #endregion
        #region Tile 2
        new short[][]
        {
            new short[] { 0, 0, (short)ItemType.ArcherPortal },
            new short[] { 2, 0, (short)ItemType.AlchemistPortal },
            new short[] { 0, 2, (short)ItemType.FirstBridge },
            new short[] { 2, 3, (short)ItemType.FirstBridge },
            new short[] { 3, 3, (short)ItemType.AlchemistExit },
        },
        #endregion
        #region Tile 3
        new short[][]
        {
            new short[] { 2, 3, (short)ItemType.WarriorPortal },
            new short[] { 3, 2, (short)ItemType.ArcherPortal },
            new short[] { 0, 1, (short)ItemType.Timer },
        },
        #endregion
        #region Tile 4
        new short[][]
        {
            new short[] { 0, 1, (short)ItemType.WarriorPortal },
            new short[] { 3, 3, (short)ItemType.SwordsmanPortal },
            new short[] { 1, 2, (short)ItemType.Timer },
        },
        #endregion
        
        #region Tile 5
        new short[][]
        {
            new short[] { 0, 1, (short)ItemType.AlchemistPortal },
            new short[] { 2, 2, (short)ItemType.Timer },
        },
        #endregion
        #region Tile 6
        new short[][]
        {
            new short[] { 0, 0, (short)ItemType.AlchemistPortal },
            new short[] { 3, 1, (short)ItemType.SwordsmanItem },
        },
        #endregion
        #region Tile 7
        new short[][]
        {
            new short[] { 1, 0, (short)ItemType.FirstBridge },
            new short[] { 2, 2, (short)ItemType.FirstBridge },
            new short[] { 2, 3, (short)ItemType.ArcherPortal },
            new short[] { 3, 0, (short)ItemType.WarriorPortal },
            new short[] { 0, 1, (short)ItemType.WarriorItem },
        },
        #endregion
        #region Tile 8
        new short[][]
        {
            new short[] { 2, 2, (short)ItemType.SwordsmanPortal },
            new short[] { 0, 0, (short)ItemType.ArcherItem },
        },
        #endregion

        #region Tile 9
        new short[][]
        {
            new short[] { 2, 2, (short)ItemType.WarriorPortal },
            new short[] { 3, 0, (short)ItemType.AlchemistItem },
        },
        #endregion
        #region Tile 10
        new short[][]
        {
            new short[] { 0, 0, (short)ItemType.ArcherPortal },
            new short[] { 1, 2, (short)ItemType.FirstBridge },
            new short[] { 2, 1, (short)ItemType.FirstBridge },
            new short[] { 2, 3, (short)ItemType.WarriorPortal },
            new short[] { 0, 3, (short)ItemType.ArcherExit },
        },
        #endregion
        #region Tile 11
        new short[][]
        {
            new short[] { 2, 0, (short)ItemType.SwordsmanPortal },
            new short[] { 3, 3, (short)ItemType.SwordsmanExit },
        },
        #endregion
        #region Tile 12
        new short[][]
        {
            new short[] { 0, 0, (short)ItemType.AlchemistPortal },
            new short[] { 2, 0, (short)ItemType.WarriorPortal },
            new short[] { 0, 2, (short)ItemType.FirstBridge },
            new short[] { 1, 3, (short)ItemType.FirstBridge },
            new short[] { 2, 1, (short)ItemType.SecondBridge },
            new short[] { 3, 2, (short)ItemType.SecondBridge },
            new short[] { 0, 3, (short)ItemType.WarriorExit },
        },
        #endregion

        #region Tile 13
        new short[][]
        {
            new short[] { 2, 2, (short)ItemType.AlchemistPortal },
        },
        #endregion
        #region Tile 14
        new short[][]
        {
            new short[] { 0, 0, (short)ItemType.SwordsmanPortal },
            new short[] { 3, 3, (short)ItemType.AlchemistPortal },
            new short[] { 1, 1, (short)ItemType.FirstBridge },
            new short[] { 2, 3, (short)ItemType.FirstBridge },
        },
        #endregion
        #region Tile 15
        new short[][]
        {
            new short[] { 3, 1, (short)ItemType.SwordsmanPortal },
            new short[] { 2, 1, (short)ItemType.FirstBridge },
            new short[] { 3, 3, (short)ItemType.FirstBridge },
        },
        #endregion
        #region Tile 16
        new short[][]
        {
            new short[] { 0, 0, (short)ItemType.ArcherPortal },
        },
        #endregion

        #region Tile 17
        new short[][]
        {
            new short[] { 1, 3, (short)ItemType.ArcherPortal },
        },
        #endregion
        #region Tile 18
        new short[][]
        {
            new short[] { 2, 3, (short)ItemType.AlchemistPortal },
        },
        #endregion
        #region Tile 19
        new short[][]
        {
            new short[] { 3, 0, (short)ItemType.ArcherPortal },
        },
        #endregion
        #region Tile 20
        new short[][]
        {
            new short[] { 1, 1, (short)ItemType.FirstBridge },
            new short[] { 2, 2, (short)ItemType.FirstBridge },
        },
        #endregion

        #region Tile 21
        new short[][]{},
        #endregion
        #region Tile 22
        new short[][]
        {
            new short[] { 0, 0, (short)ItemType.SwordsmanPortal },
        },
        #endregion
        #region Tile 23
        new short[][]
        {
            new short[] { 0, 1, (short)ItemType.ArcherPortal },
            new short[] { 3, 3, (short)ItemType.SwordsmanPortal },
        },
        #endregion
        #region Tile 24
        new short[][]
        {
            new short[] { 0, 0, (short)ItemType.WarriorPortal },
            new short[] { 1, 3, (short)ItemType.AlchemistPortal },
        },
        #endregion

    };

    public static Room getRoom(ushort ind, short x, short y, ushort rotation) {
        if (ind > roomOrder.Length + 1) return null;
        int index = 0;
        if (ind > 0) index = roomOrder[ind - 1] + 1;
        Room ret = new Room(Room.DefaultRoomSize, x, y, rotation);
        ret.addGates(gatePreset[index]);
        ret.addItem(itemPreset[index]);
        for (ushort i = 0; i < Room.DefaultRoomSize; i++) {
            for (ushort j = 0; j < Room.DefaultRoomSize; j++) {
                ret.addTile(i, j, roomPreset[index][Room.DefaultRoomSize - (int)j - 1][i]);
            }
        }

        return ret;
    }

    #endregion

    #region Roles

    private static ActionType[][][] playerActions = new ActionType[][][] {
        #region 1 player
        new ActionType[][] {
            new ActionType[] {
                ActionType.moveD,
                ActionType.moveU,
                ActionType.moveL,
                ActionType.moveR,
                ActionType.explore,
                ActionType.teleport,
                ActionType.vent,
            },
        },
        #endregion
        #region 2 players
        new ActionType[][] {
            new ActionType[] {
                ActionType.moveD,
                ActionType.moveU,
                ActionType.explore,
            },
            new ActionType[] {
                ActionType.moveL,
                ActionType.moveR,
                ActionType.teleport,
                ActionType.vent,
            },
        },
        #endregion
        #region 3 players
        new ActionType[][] {
            new ActionType[] {
                ActionType.moveD,
                ActionType.explore,
            },
            new ActionType[] {
                ActionType.moveR,
                ActionType.teleport,
            },
            new ActionType[] {
                ActionType.moveL,
                ActionType.moveU,
                ActionType.vent,
            },
        },
        #endregion
        #region 4 players
        new ActionType[][] {
            new ActionType[] {
                ActionType.moveD,
                ActionType.explore,
            },
            new ActionType[] {
                ActionType.moveL,
                ActionType.teleport,
            },
            new ActionType[] {
                ActionType.moveU,
            },
            new ActionType[] {
                ActionType.moveR,
                ActionType.vent,
            },
        },
        #endregion
        #region 5 players
        new ActionType[][] {
            new ActionType[] {
                ActionType.moveD,
                ActionType.explore,
            },
            new ActionType[] {
                ActionType.moveR,
            },
            new ActionType[] {
                ActionType.moveU,
            },
            new ActionType[] {
                ActionType.moveL,
                ActionType.vent,
            },
            new ActionType[] {
                ActionType.teleport,
                ActionType.moveD,
            },
        },
        #endregion
        #region 6 players
        new ActionType[][] {
            new ActionType[] {
                ActionType.moveD,
                ActionType.explore,
            },
            new ActionType[] {
                ActionType.moveR,
            },
            new ActionType[] {
                ActionType.moveU,
            },
            new ActionType[] {
                ActionType.moveL,
                ActionType.vent,
            },
            new ActionType[] {
                ActionType.teleport,
                ActionType.moveD,
            },
            new ActionType[] {
                ActionType.moveL,
            },
        },
        #endregion
        #region 7 players
        new ActionType[][] {
            new ActionType[] {
                ActionType.moveD,
                ActionType.explore,
            },
            new ActionType[] {
                ActionType.moveR,
            },
            new ActionType[] {
                ActionType.moveU,
            },
            new ActionType[] {
                ActionType.moveU,
                ActionType.teleport,
            },
            new ActionType[] {
                ActionType.moveL,
                ActionType.vent,
            },
            new ActionType[] {
                ActionType.moveD,
            },
            new ActionType[] {
                ActionType.moveL,
            },
        },
        #endregion
        #region 8 players
        new ActionType[][] {
            new ActionType[] {
                ActionType.moveD,
                ActionType.explore,
            },
            new ActionType[] {
                ActionType.moveR,
            },
            new ActionType[] {
                ActionType.moveR,
            },
            new ActionType[] {
                ActionType.moveU,
            },
            new ActionType[] {
                ActionType.moveU,
                ActionType.teleport,
            },
            new ActionType[] {
                ActionType.moveL,
                ActionType.vent,
            },
            new ActionType[] {
                ActionType.moveD,
            },
            new ActionType[] {
                ActionType.moveL,
            },
        },
        #endregion
    };

    public static ActionType[][] GetActionTypes(int numPlayers) {
        return playerActions[numPlayers - 1];
    }

    #endregion

    #region Util

    public static int[] randomList(int n) {
        int[] ret = new int[n];
        for (int i = 0; i < n; i++) ret[i] = i;
        for (int i = 0; i < n; i++) {
            int temp = ret[i];
            int rand = Random.Range(0, n);
            ret[i] = ret[rand];
            ret[rand] = temp;
        }
        return ret;
    }

    #endregion
}
