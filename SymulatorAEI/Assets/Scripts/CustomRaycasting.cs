using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRaycasting
{
    public static GameObject Raycast(Vector3 origin, Vector3 direction, float maxDistance)
    {
        GameObject[] allPlanes = GameObject.FindGameObjectsWithTag("RaycastingPlane");

        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject planeObject in allPlanes)
        {
            //Debug.DrawRay(planeObject.transform.position, planeObject.transform.up * maxDistance, Color.red);
            //Mathematical plane.
            Plane plane = new Plane(planeObject.transform.up.normalized, planeObject.transform.position);

            float A = plane.normal.x;
            float B = plane.normal.y;
            float C = plane.normal.z;
            float D = plane.distance;
            //Licznik i mianownik równania.
            float numerator = -(A * origin.x + B * origin.y + C * origin.z + D);
            float denominator = A * direction.x + B * direction.y + C * direction.z;

            float t = numerator / denominator;

            if (t > maxDistance || t > closestDistance)
                continue;

            Vector3 intersectionPoint = origin + t * direction.normalized;

            float planeWidth = planeObject.transform.lossyScale.x * 10f / 2f;
            float planeHeight = planeObject.transform.lossyScale.z * 10f / 2f;

            if (Vector3.Distance(intersectionPoint, planeObject.transform.position) < planeWidth)
            {
                closestDistance = t;
                closestObject = planeObject.transform.parent.gameObject;
            }
        }

        return closestObject;
    }

    //Old method, by KP.
    public static GameObject CastRay(Vector3 startingPosition, Vector3 direction, float length = Mathf.Infinity)
    {
        GameObject closestObject = null;
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Interactable");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in allObjects)
        {
            try
            {
                Collider col = obj.GetComponent<Collider>();
                Vector3 ABC = obj.transform.forward;
                Vector3 pos = obj.transform.position;
                float D = Mathf.Round(-Vector3.Dot(ABC, pos) * 10f) / 10f;
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
            catch (System.Exception ex)
            {

            }

        }
        return closestObject;
    }
}
