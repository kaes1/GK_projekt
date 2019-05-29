using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindaNowe : MonoBehaviour
{
    public int duration = 1;
    public int aktualnePietro = 0;
    public int pietro = 1;
    float time = 0;

    List<GameObject> objs;
    // Start is called before the first frame update
    void Start()
    {
        objs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T) && time > 1)
        {
            GetComponent<Animator>().SetBool("Open", true);
            time = 0;

        }
        if (Input.GetKeyDown(KeyCode.Y) && time > 1)
        {
            GetComponent<Animator>().SetBool("Open", false);
            time = 0;

        }
        time += Time.deltaTime;

    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.R) && time > 1)
        {
            StartCoroutine(tepaj());
            time = 0;

        }
    }
    private void OnTriggerEnter(Collider other)
    {

        GetComponent<Animator>().SetBool("Open", true);

        Debug.Log(other.name);
        if (other.tag == "Player")
            objs.Add(other.gameObject);
        if (other.tag == "NPC")
        {
            objs.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
            objs.Remove(other.gameObject);
        if (other.tag == "NPC")
        {
            objs.Remove(other.gameObject);
        }

        GetComponent<Animator>().SetBool("Open", false);
    }

    IEnumerator tepaj()
    {
        GetComponent<Animator>().SetBool("Open", false);
        yield return new WaitForSeconds((Mathf.Abs(pietro - aktualnePietro)) * duration);

        foreach (GameObject obj in objs)
        {
            Vector3 pos = new Vector3(obj.transform.position.x, 3 * (pietro - aktualnePietro) + obj.transform.position.y, obj.transform.position.z);

            if (obj.tag == "Player")
            {
                obj.GetComponent<CharacterController>().enabled = false;
                obj.transform.position = pos;
                obj.GetComponent<CharacterController>().enabled = true;
            }
            else if (obj.tag == "NPC")
                obj.transform.position = pos;
        }

        objs.Clear();
    }
}
