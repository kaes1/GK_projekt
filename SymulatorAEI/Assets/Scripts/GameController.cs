using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    //Enumerate representing the game state.
    public enum GameState
    {
        Running,
        Paused
    }
    private GameState gameState = GameState.Running;

    //UI displayed when game is paused via Escape.
    public GameObject PauseMenuUI;

    //UI displayed when something interactable is selected.
    public GameObject InteractionPromptUI;
    public TextMeshProUGUI InteractionPromptText;

    //UI for displaying details of object.
    public GameObject DetailsUI;
    public TextMeshProUGUI DetailsMainText;
    public TextMeshProUGUI DetailsSecondaryText;

    //Object currently selected for interaction.
    private GameObject selectedForInteraction;

    //Background Audio Control
    public AudioSource BackGroundAudioSource;

    void Start()
    {
        ResumeGame();
        BackGroundAudioSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch(gameState)
            {
                case GameState.Running:
                    PauseGame();
                    break;
                case GameState.Paused:
                    ResumeGame();
                    break;
            }
        }
    }

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        gameState = GameState.Running;

        BackGroundAudioSource.UnPause();
    }

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameState = GameState.Paused;

        BackGroundAudioSource.Pause();
    }

    public void GoToMainMenu()
    {
        Debug.Log("Going to MainMenu!");
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING NOW");
        Application.Quit();
    }

    public void ClearInteractable()
    {
        //If previously selected something, disable previous highlighting and hide interaction prompt and details.
        if (selectedForInteraction != null)
        {
            Highlight highlight = selectedForInteraction.GetComponent<Highlight>();
            if (highlight != null)
                highlight.HighlightEnd();
            HideInteractionPrompt();
            HideDetails();
            selectedForInteraction = null;
        }
    }

    public void SelectInteractable(GameObject gameObject)
    {
        //If selecting same thing, don't do anything.
        if (gameObject == selectedForInteraction)
            return;
        //Clear previous selection.
        ClearInteractable();
        //If object is interactable highlight it, show interaction prompt and remember selected object.
        if (gameObject != null && gameObject.tag.Contains("Interactable"))
        {
            Highlight highlight = gameObject.GetComponent<Highlight>();
            if (highlight != null)
                highlight.HighlightStart();
            DisplayInteractionPrompt(gameObject);
            selectedForInteraction = gameObject;
        }
        else
            selectedForInteraction = null;
    }

    public void DisplayInteractionPrompt(GameObject gameObject)
    {
        if (gameObject != null)
            switch (gameObject.tag)
            {
                case "InteractablePlaque":
                    InteractionPromptText.text = "Press E to Read";
                    break;
                default:
                    InteractionPromptText.text = "Press E to Interact";
                    break;
            }
        InteractionPromptUI.SetActive(true);
    }

    public void HideInteractionPrompt()
    {
        InteractionPromptUI.SetActive(false);
    }

    public void DisplayDetails(GameObject gameObject)
    {
        //Display different details based on object.
        switch (gameObject.tag)
        {
            case "InteractablePlaque":
                RoomInformation roomInfo = gameObject.GetComponent<RoomInformation>();
                DetailsMainText.text = roomInfo.plaqueText;
                DetailsSecondaryText.text = "'E' to close";
                break;
            default:
                DetailsMainText.text = "Main";
                DetailsSecondaryText.text = "'E' to close";
                break;
        }
        DetailsUI.SetActive(true);
    }

    public void HideDetails()
    {
        DetailsUI.SetActive(false);
    }

    public void InteractWithSelected()
    {
        //Return if there's nothing to interact with.
        if (selectedForInteraction == null || !selectedForInteraction.tag.Contains("Interactable"))
            return;

        //Do different interaction based on object.
        switch (selectedForInteraction.tag)
        {
            case "InteractablePlaque":
                if (!DetailsUI.activeSelf)
                {
                    HideInteractionPrompt();
                    DisplayDetails(selectedForInteraction);
                }
                else
                {
                    HideDetails();
                }
                break;
            default:
                break;
        }
    }
}
