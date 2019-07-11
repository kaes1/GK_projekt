using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Linq;

public class GameController : MonoBehaviour
{
    //Enumerate representing the game state.
    public enum GameState
    {
        Running,
        Paused
    }
    //Current game state.
    private GameState gameState = GameState.Running;

    //Data Controller.
    private DataController dataController;

    //Player GameObject
    private GameObject Player;
    //Background audio source.
    private AudioSource BackGroundAudioSource;
    //Object currently selected for interaction.
    private GameObject selectedObject;

    //UI displayed when game is paused via Escape.
    public GameObject PauseMenuUI;
    public GameObject MainPauseMenuUI;
    public GameObject ChooseRoomMenuUI;
    //UI displayed when something interactable is selected.
    public GameObject InteractionPromptUI;
    public TextMeshProUGUI InteractionPromptText;
    //UI for displaying details of object.
    public GameObject DetailsUI;
    public TextMeshProUGUI DetailsMainText;
    public TextMeshProUGUI DetailsSecondaryText;
    //UI for displaying currently chosen room.
    public GameObject ChosenRoomUI;
    public TextMeshProUGUI ChosenRoomText;



    //Currently chosen room that the player wants to go to.
    private RoomInformation ChosenRoom;
    //Waypoint corresponding to currently chosen room.
    private Waypoint ChosenRoomWaypoint;
    
    List<Waypoint> waypointsPathToRoom;

    Waypoint closestWaypoint;


    //All waypoints.
    private List<Waypoint> Waypoints = new List<Waypoint>();
    //Line Renderer for drawing path to chosen room.
    private LineRenderer lineRenderer;

    void Start()
    {
        //Find Data Controller.
        dataController = GameObject.FindGameObjectWithTag("DataController").GetComponent<DataController>();
        //Find player GameObject and background audio source.
        Player = GameObject.FindGameObjectWithTag("Player");
        BackGroundAudioSource = Player.GetComponent<AudioSource>();
        //Find waypoints.
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Waypoint"))
            Waypoints.Add(gameObject.GetComponent<Waypoint>());
        //Find LineRenderer component.
        lineRenderer = GetComponent<LineRenderer>();
        //Resume the game.
        ResumeGame();
        //Play background audio.
        BackGroundAudioSource.Play();
        //Initially choose no room.
        ChooseRoom("");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            switch (gameState)
            {
                case GameState.Running:
                    PauseGame();
                    break;
                case GameState.Paused:
                    ResumeGame();
                    break;
            }

        if (ChosenRoom != null && ChosenRoomWaypoint != null)
            DrawPathTo(ChosenRoomWaypoint);
    }

    public void ChooseRoom(string roomID)
    {
        ChosenRoom = null;
        ChosenRoomWaypoint = null;
        ChosenRoomText.text = "No room chosen\nPress 'ESC' to pause\nand choose a room.";

        if (dataController.RoomInformationDictionary.Keys.Contains(roomID))
        {
            ChosenRoom = dataController.RoomInformationDictionary[roomID];
            ChosenRoomText.text = ChosenRoom.roomID + "\n" + ChosenRoom.details;
            ChosenRoomWaypoint = null;
            foreach (Waypoint w in Waypoints)
                if (w.roomID == roomID)
                    if (ChosenRoomWaypoint == null
                        || Vector3.Distance(w.transform.position, Player.transform.position) < Vector3.Distance(ChosenRoomWaypoint.transform.position, Player.transform.position))
                        ChosenRoomWaypoint = w;

        }
    }

    private void DrawPathTo(Waypoint target)
    {
        Waypoint currentClosestWaypoint = FindClosestWaypointToPlayer();
        if (currentClosestWaypoint == target)
        {
            lineRenderer.positionCount = 0;
            return;
        }


        if (closestWaypoint != currentClosestWaypoint)
        {
            closestWaypoint = currentClosestWaypoint;
            if (waypointsPathToRoom != null && currentClosestWaypoint == waypointsPathToRoom[0])
            {
                //Moving along the right path.
                waypointsPathToRoom.RemoveAt(0);
            }
            else
            {
                waypointsPathToRoom = FindShortestPathTo(target);
            } 
        }
            
        if (waypointsPathToRoom.Count > 6)
        {
            lineRenderer.positionCount = 6;
            lineRenderer.SetPosition(0, Player.transform.position);
            for (int i = 1; i < 6; i++)
                lineRenderer.SetPosition(i, waypointsPathToRoom[i].transform.position);
        }
        else
        {
            lineRenderer.positionCount = waypointsPathToRoom.Count;
            lineRenderer.SetPosition(0, Player.transform.position);
            for (int i = 1; i < waypointsPathToRoom.Count; i++)
                lineRenderer.SetPosition(i, waypointsPathToRoom[i].transform.position);
        }



    }

    private List<Waypoint> FindShortestPathTo(Waypoint target)
    {
        //Initial node is the closest one to player.
        Waypoint source = FindClosestWaypointToPlayer();
        //Shortest path from source to target;
        List<Waypoint> shortestPath = new List<Waypoint>();
        //Return only target if source and target are the same.
        if (target == source)
        {
            shortestPath.Add(target);
            return shortestPath;
        }

        //Dijkstra algorithm.
        var distances = new Dictionary<Waypoint, float>();
        var previous = new Dictionary<Waypoint, Waypoint>();
        List<Waypoint> Q = new List<Waypoint>();
        foreach (Waypoint w in Waypoints)
        {
            distances[w] = Mathf.Infinity;
            previous[w] = null;
            Q.Add(w);
        }
        distances[source] = 0;
        while (Q.Count > 0)
        {
            float minDistance = Mathf.Infinity;
            Waypoint current = null;
            foreach (Waypoint w in Q)
                if (distances[w] < minDistance)
                {
                    minDistance = distances[w];
                    current = w;
                }
            Q.Remove(current);
            if (current == target || current == null)
                break;


            foreach (Waypoint v in current.Neighbours)
                if (v != null && Q.Contains(v))
                {
                    float alt = distances[current] + Vector3.Distance(current.transform.position, v.transform.position);
                    if (alt < distances[v])
                    {
                        distances[v] = alt;
                        previous[v] = current;
                    }
                }
        }

        //Read shortest path from source to target.
        Waypoint u = target;
        if (previous[u] || u == source)
            while (u != null)
            {
                shortestPath.Insert(0, u);
                u = previous[u];
            }

        return shortestPath;
    }

    private Waypoint FindClosestWaypointToPlayer()
    {
        Waypoint closestWaypoint = null;
        float closestDistance = Mathf.Infinity;
        foreach (Waypoint waypoint in Waypoints)
        {
            float distance = Vector3.Distance(Player.transform.position, waypoint.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypoint = waypoint;
            }
        }
        return closestWaypoint;
    }




    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        MainPauseMenuUI.SetActive(false);
        ChooseRoomMenuUI.SetActive(false);
        BackGroundAudioSource.UnPause();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        gameState = GameState.Running;
    }

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        MainPauseMenuUI.SetActive(true);
        ChooseRoomMenuUI.SetActive(false);
        BackGroundAudioSource.Pause();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameState = GameState.Paused;
    }

    public void GoToMainMenu()
    {
        Debug.Log("Going to MainMenu!");
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING NOW");
        Application.Quit();
    }



    private void ClearInteractable()
    {
        HideInteractionPrompt();
        HideDetails();
        //If previously selected something, disable previous highlighting and hide interaction prompt and details.
        if (selectedObject != null)
        {
            if (selectedObject.GetComponent<Highlight>() != null)
                selectedObject.GetComponent<Highlight>().HighlightEnd();
            selectedObject = null;
        }
    }

    public void SelectObject(GameObject newObject)
    {
        //If selecting same thing, don't do anything.
        if (newObject == selectedObject)
            return;
        //Clear previous selection.
        ClearInteractable();
        //If object is interactable highlight it, show interaction prompt and remember selected object.
        if (newObject != null && newObject.tag.Contains("Interactable"))
        {
            if (newObject.GetComponent<Highlight>() != null)
                newObject.GetComponent<Highlight>().HighlightStart();
            DisplayInteractionPrompt(newObject);
            selectedObject = newObject;
        }
    }

    private void DisplayInteractionPrompt(GameObject gameObject)
    {
        if (gameObject != null && gameObject.GetComponent<Interactable>() != null)
            InteractionPromptText.text = gameObject.GetComponent<Interactable>().GetInteractPromptText();
        InteractionPromptUI.SetActive(true);
    }

    private void HideInteractionPrompt()
    {
        InteractionPromptUI.SetActive(false);
    }

    public void DisplayDetailsForRoom(string roomID)
    {
        if (dataController.RoomInformationDictionary.Keys.Contains(roomID))
        {
            RoomInformation roomInfo = dataController.RoomInformationDictionary[roomID];
            DetailsMainText.text = roomInfo.roomID;
            DetailsSecondaryText.text = roomInfo.details;
        }
        else
        {
            DetailsMainText.text = "404";
            DetailsSecondaryText.text = "Not Found";
        }
        HideInteractionPrompt();
        DetailsUI.SetActive(true);
    }

    private void HideDetails()
    {
        DetailsUI.SetActive(false);
    }

    public void InteractWithSelected()
    {
        if (selectedObject != null)
            if (selectedObject.GetComponent<Interactable>() != null)
                selectedObject.GetComponent<Interactable>().Interact();
    }
}
