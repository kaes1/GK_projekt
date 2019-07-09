using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{

    private Color originalColor;
    private Color highlightColor;
    private bool highlightThis = false;



    // Start is called before the first frame update
    void Awake()
    {
        if(GetComponent<Renderer>() != null)
        {
            highlightThis = true;
            originalColor = this.GetComponent<Renderer>().material.color;
            highlightColor = new Color(originalColor.r * 1.6f, originalColor.g * 1.6f, originalColor.b * 1.6f);
        }
    }

    public void HighlightStart()
    {
        if (highlightThis)
            GetComponent<Renderer>().material.color = highlightColor;

        for(int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).GetComponent<Highlight>())
                transform.GetChild(i).GetComponent<Highlight>().HighlightStart();
    }

    public void HighlightEnd()
    {
        if (highlightThis)
            GetComponent<Renderer>().material.color = originalColor;

        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).GetComponent<Highlight>())
                transform.GetChild(i).GetComponent<Highlight>().HighlightEnd();
    }

}
