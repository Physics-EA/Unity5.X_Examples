using UnityEngine;
using System.Collections;

public class PopsicleController : MonoBehaviour
{

    public GameObject NPC;

    private void OnTriggerEnter(Collider collider)
    {
        if (!enabled)
        {
            return;
        }
        if (collider.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            NPC.SetActive(true);
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
