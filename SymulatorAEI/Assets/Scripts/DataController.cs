using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{
    public Dictionary<string, RoomInformation> RoomInformationDictionary { get; private set; }
    public TextAsset RoomInformationTextAsset;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        if (RoomInformationTextAsset != null)
            LoadRoomInformationFromTextAsset(RoomInformationTextAsset);
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(1);
    }

    private void LoadRoomInformationFromTextAsset(TextAsset textAsset)
    {
        RoomInformationDictionary = new Dictionary<string, RoomInformation>();

        string text = textAsset.ToString();
        string line;
        RoomInformation roomInfo = null;
        using (StringReader reader = new StringReader(text))
        {
            while ((line = reader.ReadLine()) != null)
            {
                //Lines starting with ! are comments.
                if (line.StartsWith("!"))
                    continue;
                //Lines starting with #id denote a new room.
                if (line.StartsWith("#"))
                {
                    AddRoomInformation(roomInfo);
                    string roomID = line.Substring(1);
                    roomInfo = new RoomInformation();
                    roomInfo.roomID = roomID;
                    roomInfo.details = "";
                    continue;
                }
                roomInfo.details += line + "\n";
            }
            AddRoomInformation(roomInfo);
        }
    }

    private void AddRoomInformation(RoomInformation roomInfo)
    {
        if (roomInfo == null)
            return;

        foreach (string key in RoomInformationDictionary.Keys)
            if (key.Equals(roomInfo.roomID))
                return;

        roomInfo.details.TrimEnd('\n');
        RoomInformationDictionary.Add(roomInfo.roomID, roomInfo);  
    }
}
