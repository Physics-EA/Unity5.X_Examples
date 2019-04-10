using UnityEngine;
using System.Collections;

public class PlayerFall : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)
        {
            transform.position = new Vector3(0, 25, 3);
        }
    }
}
