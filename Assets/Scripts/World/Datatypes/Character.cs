using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Ind { get; private set; }
    public CharacterClass C { get; private set; }

    public Character(int ind, int x, int y, CharacterClass c)
    {
        Ind = ind;
        X = x;
        Y = y;
        C = c;
    }
    public void setPos(int x, int y)
    {
        X = x;
        Y = y;
    }
}
