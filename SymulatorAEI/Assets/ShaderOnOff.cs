using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderOnOff : MonoBehaviour
{
    public bool isEnable;
    public string tag;
    GameObject[] objs;

    private void Start()
    {
        objs = GameObject.FindGameObjectsWithTag(tag);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            foreach (GameObject obj in objs)
            {
                obj.SetActive(isEnable);
            }
        }
    }
}
