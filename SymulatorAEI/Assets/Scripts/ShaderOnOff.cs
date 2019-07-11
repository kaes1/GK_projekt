using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderOnOff : MonoBehaviour
{
    public bool isEnable;
    public GameObject[] objs;

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach(GameObject obj in objs)
            obj.SetActive(isEnable);
        }
    }
}
