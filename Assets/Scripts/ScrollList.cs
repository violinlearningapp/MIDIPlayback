using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Item
{
    public string songName;
}



public class ScrollList : MonoBehaviour
{

    //public List<Item> itemList;
    public List<string> songList;
    public Transform contentPanel;
    public SimpleObjectPool buttonObjectPool;

    // Use this for initialization
    void Start()
    {
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        RemoveButtons();
        AddButtons();
    }

    private void AddButtons()
    {
        GetSongList();
        for (int i = 0; i < songList.Count; i++)
        {
            string song = songList[i];
            GameObject newButton = buttonObjectPool.GetObject();
            newButton.transform.SetParent(contentPanel);

            ButtonPrefab buttonPrefab = newButton.GetComponent<ButtonPrefab>();
            buttonPrefab.Setup(song, this);
        }
        //for (int i = 0; i < itemList.Count; i++)
        //{
        //    Item item = itemList[i];
        //    GameObject newButton = buttonObjectPool.GetObject();
        //    newButton.transform.SetParent(contentPanel);

        //    ButtonPrefab buttonPrefab = newButton.GetComponent<ButtonPrefab>();
        //    buttonPrefab.Setup(item, this);
        //}
    }

    private void RemoveButtons()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    public void GetSongList()
    {
        string path = Application.persistentDataPath + "/Midis";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.txt");
        for (int i = 0; i < info.Length; i++)
        {
            //songList.Add(info[i].Name);

            songList.Add(info[i].Name.Substring(0, info[i].Name.Length - info[i].Extension.Length));
        }
    }
}
