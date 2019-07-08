using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{

    private Color originalColor;
    private Color highlightColor;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = this.GetComponent<Renderer>().material.color;

        if (this.tag == "InteractablePlaque")
        {
            //highlightColor = new Color(originalColor.r * 2.7f, originalColor.g, originalColor.b);
            highlightColor = new Color(200f, 0f, 50f);
        }
        else
        {
            highlightColor = new Color(originalColor.r * 1.8f, originalColor.g * 1.8f, originalColor.b * 1.8f);
        }
        
    }

    void Update() { }

    public void HighlightStart()
    {
        this.GetComponent<Renderer>().material.color = highlightColor;
    }

    public void HighlightEnd()
    {
        this.GetComponent<Renderer>().material.color = originalColor;
    }

}
