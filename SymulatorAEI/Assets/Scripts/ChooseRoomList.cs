using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRoomList : MonoBehaviour
{

    public GameObject buttonTemplate;
    public GameObject content;

    private List<string> roomIDs;

    //Data Controller.
    private DataController dataController;
    //Current search input.
    private string currentSearchInput;

    // Start is called before the first frame update
    void Start()
    {
        //Find Data Controller.
        dataController = GameObject.FindGameObjectWithTag("DataController").GetComponent<DataController>();
        //Initial list population.
        RepopulateList("");
    }

    public void RepopulateList(string searchInput)
    {
        if (currentSearchInput == searchInput)
            return;

        ClearList();
        currentSearchInput = searchInput;
        List<string> roomIDs = new List<string>();
        foreach(string roomID in dataController.RoomInformationDictionary.Keys)
            if (roomID.Contains(searchInput))
                roomIDs.Add(roomID);
        roomIDs.Sort();
        foreach (string roomID in roomIDs)
            AddButton(roomID);
    }

    private void AddButton(string text)
    {
        GameObject button = Instantiate(buttonTemplate) as GameObject;
        button.SetActive(true);
        button.name = "Button"+text;
        button.GetComponent<ChooseRoomButton>().SetText(text);
        button.transform.SetParent(content.transform, false);
    }

    private void ClearList()
    {
        foreach(Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
