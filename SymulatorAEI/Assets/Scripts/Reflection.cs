using UnityEngine;

[ExecuteInEditMode]
public class Reflection : MonoBehaviour
{
    // Attach this script to an object that uses a Reflective shader.
    // Realtime reflective cubemaps!

    int cubemapSize = 128;
    bool oneFacePerFrame = false;
    Camera cam;
    Material _MaterialShader;
    RenderTexture renderTexture;

    [Header("Wyłącz i włącz skrypt żeby nadpisać zmiany z suwaków/kolorów")]
    [Range(0.01f,1)]
    public float inten1 = 0.5f;//intensywnosc 1
    [Range(0.01f, 1)]
    public float inten2 = 0.5f;//intensywnosc 2

    public Color col1 = Color.white;
    public Color col2 = Color.white;

    void Start()
    {
        // render all six faces at startup
        UpdateCubemap(63);
    }

    //void OnDisable()
    //{
    //    DestroyImmediate(cam);
    //    DestroyImmediate(renderTexture);
    //    DestroyImmediate(_MaterialShader);
    //}

    void LateUpdate()
    {
        if (oneFacePerFrame)
        {
            var faceToRender = Time.frameCount % 6;
            var faceMask = 1 << faceToRender;
            UpdateCubemap(faceMask);
        }
        else
        {
            UpdateCubemap(63); // all six faces
        }
    }

    void UpdateCubemap(int faceMask)
    {
        if (!cam)
        {
            GameObject obj = new GameObject("CubemapCamera", typeof(Camera));
            obj.hideFlags = HideFlags.HideAndDontSave;
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.identity;
            cam = obj.GetComponent<Camera>();
            cam.farClipPlane = 100; // don't render very far into cubemap
            cam.enabled = false;
        }

        if (!renderTexture)
        {
            renderTexture = new RenderTexture(cubemapSize, cubemapSize, 16);
            renderTexture.dimension = UnityEngine.Rendering.TextureDimension.Cube;
            renderTexture.hideFlags = HideFlags.HideAndDontSave;
            //GetComponent<Renderer>().sharedMaterial.SetTexture("_Cube", renderTexture);
        }

        cam.transform.position = transform.position;
        cam.RenderToCubemap(renderTexture, faceMask);

        if (_MaterialShader == null)
        {
            _MaterialShader = new Material(Shader.Find("Custom/Reflection")); //stworzenie materialu ktory bedzie obslugiwal shader
            _MaterialShader.hideFlags = HideFlags.HideAndDontSave; //ukrycie materialu z shaderem
            GetComponent<Renderer>().material = _MaterialShader; //dodanie shadera do obiektu
            _MaterialShader.SetFloat("_Inten", inten1); //nadanie intensywnosci pierwszej 
            _MaterialShader.SetFloat("_Inten2", inten2); //nadanie intensywnosci drugiej 
            _MaterialShader.SetColor("_Color", col1);//nadanie  koloru pierwszego
            _MaterialShader.SetColor("_Color2", col2);//nadanie  koloru drugiego
        }
        _MaterialShader.SetTexture("_Cube", renderTexture); //dodanie cubemapa do shadera

    }
}