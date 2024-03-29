﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveState
{
    // WILL BE USED TO RETURN PLAYER TO CORRECT COORDIATES AFTER MAIN SCENE IS RELOADED
    public static float playerCoordinateX = 0;
    public static float playerCoordinateY = 0;

    // WILL BE USED TO DETERMINE WHICH MUSIC TO PLAY ON MAIN SCENE LOAD
    public static bool inTown = true;

    // WILL BE USED TO GET THE CORRECT SPRITES
    public static int allyID { get; set; }
    public static int enemyID { get; set; }

    // WILL BE USED TO DETERMINE IF GAME IS OVER/INFORM USER WHAT CREATURES THEY'VE CAPTURED
    public static bool[] capturedCreatures = new bool[10];
}
