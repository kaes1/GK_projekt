using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{

    // public RoomInformation[] allRoomInformation;
    //public Dictionary<string, RoomInformation> allRoomInformation;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(1);
    }

    /*public RoomInformation GetRoomInformation(string roomID)
    {
        foreach (RoomInformation ri in allRoomInformation)
            if (ri.roomID.Equals(roomID))
                return ri;

        return null;
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
