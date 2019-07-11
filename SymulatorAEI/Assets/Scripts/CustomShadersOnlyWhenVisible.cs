using UnityEngine;

public class CustomShadersOnlyWhenVisible : MonoBehaviour
{
    Refract refraction;
    Reflection reflection;

    private void Update()
    {
        if (IsVisibleFrom(gameObject.GetComponent<Renderer>(), Camera.main))
        {
            if (refraction)
                refraction.enabled = true;
            if (reflection)
                reflection.enabled = true;
        }
        else
        {
            if (refraction)
                refraction.enabled = false;
            if (reflection)
                reflection.enabled = false;
        }
    }

    void Start()
    {
        refraction = gameObject.GetComponent<Refract>();
        reflection = gameObject.GetComponent<Reflection>();
    }

    public bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

}
