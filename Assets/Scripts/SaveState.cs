using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveState
{
    // WILL BE USED TO RETURN PLAYER TO CORRECT COORDIATES AFTER MAIN SCENE IS RELOADED
    public static float playerCoordinateX { get; set; }
    public static float playerCoordinateY { get; set; }

    // WILL BE USED TO GET THE CORRECT SPRITES
    public static int allyID { get; set; }
    public static int enemyID { get; set; }

    // WILL BE USED TO DETERMINE IF GAME IS OVER/INFORM USER WHAT CREATURES THEY'VE CAPTURED
    public static bool[] capturedCreatures { get; set; }

}
