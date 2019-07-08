using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum shaderScript { REFLECTION, REFRATCION };
public class VisibleOnCamera : MonoBehaviour
{
    Refract refract;
    Reflection reflection;
    
    public shaderScript script;
    private void Update()
    {

        if (IsVisibleFrom(gameObject.GetComponent<Renderer>(), Camera.main))
        {
            if (script == shaderScript.REFRATCION)
                refract.enabled = true;
            else if (script == shaderScript.REFLECTION)
                reflection.enabled = true;
        }
        else
        {
            if (script == shaderScript.REFRATCION)
                refract.enabled = false;
            else if (script == shaderScript.REFLECTION)
                reflection.enabled = false;
        }

        //if (rend.isVisible)
        //{
        //    refract.enabled = true;
        //}
        //else
        //    refract.enabled = false;


        //Vector3 screenPoint = camera.WorldToViewportPoint(gameObject.transform.position);
        //if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        //{
        //     refract.enabled= true;
        //}
        //else
        //    refract.enabled = false;
    }


    void Start()
    {
        refract = gameObject.GetComponent<Refract>();
        reflection = gameObject.GetComponent<Reflection>();
    }

    public bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

}
