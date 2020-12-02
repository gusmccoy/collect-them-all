using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlerSpriteController : MonoBehaviour
{
    public Sprite[] enemySprites;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = enemySprites[SaveState.enemyID];
    }

}
