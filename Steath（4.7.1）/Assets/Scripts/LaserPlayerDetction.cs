using UnityEngine;
using System.Collections;

public class LaserPlayerDetction : MonoBehaviour
{
    private GameObject player;
    private LastPlayerSighting lastPlayerSighting;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag(Tags.Player);
        lastPlayerSighting = GameObject.FindWithTag(Tags.GameController).GetComponent<LastPlayerSighting>();
    }

    void OnTriggerStay(Collider other)
    {
        if (GetComponent<Renderer>().enabled)
        {
            if (other.gameObject == player)
            {
                lastPlayerSighting.position = other.transform.position;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
