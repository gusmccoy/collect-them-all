using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureLog
{
    public bool[] obtainedCreature = new bool[10];
    public Sprite[] creatureSprites;
    public Sprite[] creatureOutline;

    public CreatureLog()
    {
        for(int i = 0; i < 10; i++)
        {
            obtainedCreature[i] = false;
        }
    }
 
}
