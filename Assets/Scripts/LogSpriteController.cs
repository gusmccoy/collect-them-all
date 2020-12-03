using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpriteController : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;
    public Sprite unobtained;

    void Start()
    {
        if(!SaveState.capturedCreatures[id])
        {
            GetComponent<SpriteRenderer>().sprite = unobtained;
        }
        
    }
}
