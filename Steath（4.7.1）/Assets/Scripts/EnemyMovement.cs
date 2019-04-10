using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float deadZone = 5f;
    private Transform player;
    private EnenmySight enemySight;
    private NavMeshAgent nav;
    private Animator anim;
    private HashIDs hash;
    private SimpleLocomotion locomotion;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag(Tags.Player).transform;
        enemySight = GetComponent<EnenmySight>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hash = GameObject.FindWithTag(Tags.GameController).GetComponent<HashIDs>();
        nav.updatePosition = false;
        locomotion = new SimpleLocomotion(anim, hash);
        anim.SetLayerWeight(1, 1f);
        anim.SetLayerWeight(2, 1f);
        deadZone *= Mathf.Deg2Rad;
    }

    void OnAnimatorMove()
    {
        nav.velocity = anim.deltaPosition / Time.deltaTime;
        transform.rotation = anim.rootRotation;
    }


    float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if (toVector == Vector3.zero)
        {
            return 0f;
        }
        float angle = Vector3.Angle(fromVector, toVector);
        Vector3 nomal = Vector3.Cross(fromVector, toVector);
        angle *= Mathf.Sign(Vector3.Dot(nomal, upVector));
        angle *= Mathf.Deg2Rad;
        return angle;
    }


    void NavAnimSetup()
    {
        float speed;
        float angle;
        if (enemySight.playerInSight)
        {
            speed = 0f;
            angle = FindAngle(transform.forward, player.position - transform.position, transform.up);
        }
        else
        {
            speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
            angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);
            if (Mathf.Abs(angle) < deadZone)
            {
                transform.LookAt(transform.position + nav.desiredVelocity);
                angle = 0f;
            }
        }
        locomotion.Do(speed, angle);
    }
    // Update is called once per frame
    void Update()
    {
        NavAnimSetup();
    }
}
