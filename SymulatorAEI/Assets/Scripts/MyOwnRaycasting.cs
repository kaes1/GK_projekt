﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyOwnRaycasting : MonoBehaviour
{
    public static GameObject CastRay(Vector3 startingPosition, Vector3 direction, float length = Mathf.Infinity)
    {
        GameObject closestObject = null;
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("InteractablePlaque");

        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in allObjects)
        {
            /* Collider col = obj.GetComponent<Collider>();

             bool hit = false;
             Vector3 act_pos = startingPosition;
             float act_dist = 0f;
             while (act_dist < length && !hit)
             {
                 act_dist = Vector3.SqrMagnitude(act_pos - startingPosition);
                 if (col.bounds.Contains(act_pos))
                 {
                     hit = true;                  
                     if(act_dist < closestDistance)
                     {
                         closestDistance = act_dist;
                         closestObject = obj;
                     }
                 }
                 act_pos += direction.normalized;
             }*/


            try
            {
                Collider col = obj.GetComponent<Collider>();
                Vector3 ABC = obj.transform.forward;
                Vector3 pos = obj.transform.position;
                float D = Mathf.Round(-Vector3.Dot(ABC, pos)*10f)/10f;
                float licznik = ABC.x * startingPosition.x + ABC.y * startingPosition.y + ABC.z * startingPosition.z + D;
                float mianownik = Vector3.Dot(ABC, direction);

                Vector3 intersection_point = startingPosition - direction * licznik / mianownik;
                float distance = (intersection_point - startingPosition).sqrMagnitude;
                if (distance <= length)
                {
                    if (col.bounds.Contains(intersection_point))
                    {
                        if (distance < closestDistance)
                        {
                            closestObject = obj;
                            closestDistance = distance;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {

            }

        }
        return closestObject;
    }
}
