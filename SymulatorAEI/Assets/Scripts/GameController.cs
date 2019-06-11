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
        HideInteractionPrompt();
        HideDetails();
        //If previously selected something, disable previous highlighting and hide interaction prompt and details.
        if (selectedForInteraction != null)
        {
            if (selectedForInteraction.GetComponent<Highlight>() != null)
                selectedForInteraction.GetComponent<Highlight>().HighlightEnd();
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
            if (gameObject.GetComponent<Highlight>() != null)
                gameObject.GetComponent<Highlight>().HighlightStart();
            DisplayInteractionPrompt(gameObject);
            selectedForInteraction = gameObject;
        }
    }

    private void DisplayInteractionPrompt(GameObject gameObject)
    {
        if (gameObject != null)
            if (gameObject.GetComponent<Interactable>() != null)
                InteractionPromptText.text = gameObject.GetComponent<Interactable>().GetInteractPromptText();
        InteractionPromptUI.SetActive(true);
    }

    private void HideInteractionPrompt()
    {
        InteractionPromptUI.SetActive(false);
    }

    public void DisplayDetails(string mainText, string secondaryText)
    {
        DetailsMainText.text = mainText;
        DetailsSecondaryText.text = secondaryText;

        HideInteractionPrompt();
        DetailsUI.SetActive(true);
    }

    private void HideDetails()
    {
        DetailsUI.SetActive(false);
    }

    public void InteractWithSelected()
    {
        if (selectedForInteraction != null)
            if (selectedForInteraction.GetComponent<Interactable>() != null)
                selectedForInteraction.GetComponent<Interactable>().Interact();
    }
}
