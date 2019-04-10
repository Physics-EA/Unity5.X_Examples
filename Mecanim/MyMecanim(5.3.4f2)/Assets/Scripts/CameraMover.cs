using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{

    public Transform follow;
    public float distanceAway = 5.0f;
    public float distanceUp = 2.0f;
    public float smooth = 1.0f;
    private Vector3 targetPosition;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void LateUpdate()
    {
        targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
        transform.LookAt(follow);
    }
}
