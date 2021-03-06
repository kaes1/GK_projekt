﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    //Player's camera.
    [SerializeField] private Camera PlayerCamera;
    //Distance of the ray used in raycasting.
    public float distanceToSee;

    private GameObject lookedAtObject;

    private GameController gameController;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        lookedAtObject = null;
    }

    void Update()
    {
        //Draw the ray used for raycasting.
        Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward * distanceToSee, Color.magenta);
        //Use Raycasting to get object hit.
        GameObject objectHit = null;
        objectHit = CustomRaycasting.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, distanceToSee);
        //If looking at something new, tell game controller what is currently selected.
        if (objectHit != lookedAtObject)
        {
            gameController.SelectObject(objectHit);
            lookedAtObject = objectHit;
        } 
    
        //If interacting, tell game controller to interact.
        if (Input.GetKeyDown(KeyCode.E))
            gameController.InteractWithSelected();
    }
}
