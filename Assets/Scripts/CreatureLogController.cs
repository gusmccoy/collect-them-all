using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreatureLogController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var startBtn = GameObject.FindObjectOfType<Button>();
        startBtn.onClick.AddListener(() => ReturnToGame());
    }
    private void ReturnToGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
