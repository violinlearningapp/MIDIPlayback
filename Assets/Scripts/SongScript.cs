using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class SongScript : MonoBehaviour
{

    public Text songName;


    void Start()
    {
        songName.text = ButtonPrefab.nameOfSong;
    }

    public void BackToSongSelect()
    {
        SceneManager.LoadScene("ListScene");
    }
}
