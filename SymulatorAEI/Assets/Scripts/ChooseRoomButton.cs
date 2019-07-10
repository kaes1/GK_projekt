using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseRoomButton : MonoBehaviour
{
    private string text = "ND";

    public void SetText(string text)
    {
        this.text = text;
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
    }

    public void OnClick()
    {
        FindObjectOfType<GameController>().ChooseRoom(text);
    }

}
