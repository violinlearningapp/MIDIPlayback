using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonPrefab : MonoBehaviour
{

    public Button button;
    public Text songText;

    public static string nameOfSong;

    private string song;
    private ScrollList scrollList;

    // Use this for initialization
    void Start()
    {

    }

    public void Setup(string currentSong, ScrollList currentScrollList)
    {
        song = currentSong;
        songText.text = song;
        scrollList = currentScrollList;
    }

    public void HandleClick()
    {
        nameOfSong = song;
        Debug.Log(nameOfSong);
        SceneManager.LoadScene("SongScene");
    }
}
