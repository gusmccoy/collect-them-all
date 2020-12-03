using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var startBtn = GameObject.FindObjectOfType<Button>();
        startBtn.onClick.AddListener(() => StartGame());
    }

    private void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
