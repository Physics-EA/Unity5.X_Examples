using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public AudioClip shoutingClip;
    public float turnSmoothing = 15f;
    public float speedDampTime = 0.1f;
    private Animator animator;
    private HashIDs hash;

    void Awake()
    {
        animator = GetComponent<Animator>();
        hash = GameObject.FindWithTag(Tags.GameController).GetComponent<HashIDs>();
        animator.SetLayerWeight(1, 1f);
    }


    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool sneak = Input.GetButton("Sneak");
        MovementManagement(h, v, sneak);
    }


    void Update()
    {
        bool shout = Input.GetButtonDown("Attract");
        animator.SetBool(hash.shoutingBool, shout);
        AudioManagement(shout);
    }



    void MovementManagement(float h, float v, bool sneaking)
    {
        animator.SetBool(hash.sneakingBool, sneaking);
        if (h != 0 || v != 0)
        {
            Rotating(h, v);
            animator.SetFloat(hash.speedFloat, 5.5f, speedDampTime, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(hash.speedFloat, 0f);
        }
    }


    void Rotating(float h, float v)
    {
        Vector3 targetDir = new Vector3(h, 0, v);
        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
        Rigidbody r = GetComponent<Rigidbody>();
        Quaternion newRotation = Quaternion.Lerp(r.rotation, targetRotation, turnSmoothing * Time.deltaTime);
        r.MoveRotation(newRotation);

    }

    void AudioManagement(bool shout)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.locomotionState)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
        if (shout)
        {
            AudioSource.PlayClipAtPoint(shoutingClip, transform.position);
        }
    }

}
