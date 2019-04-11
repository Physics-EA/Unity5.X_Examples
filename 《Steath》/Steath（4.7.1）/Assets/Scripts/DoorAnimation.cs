using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour
{
    public bool requireKey;
    public AudioClip doorSwitchClip;
    public AudioClip accessDeniedClip;
    private Animator anim;
    private HashIDs hash;
    private GameObject player;
    private PlayerInventory playerInventory;
    private int count;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        hash = GameObject.FindWithTag(Tags.GameController).GetComponent<HashIDs>();
        player = GameObject.FindWithTag(Tags.Player);
        playerInventory = player.GetComponent<PlayerInventory>();
    }

    void OnTriggerEnter(Collider other)
    {
        AudioSource audio = GetComponent<AudioSource>();

        if (other.gameObject == player)
        {
            if (requireKey)
            {
                if (playerInventory.hasKey)
                {
                    count++;
                }
                else
                {
                    audio.clip = accessDeniedClip;
                    audio.Play();
                }
            }
            else count++;

        }

        else if (other.gameObject.tag == Tags.Enemy)
        {
            if (other is CapsuleCollider)
            {
                count++;
            }
        }
    }





    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player || (other.gameObject.tag == Tags.Enemy && other is CapsuleCollider))
        {
            count = Mathf.Max(0, count - 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool(hash.openBool, count > 0);
        AudioSource audio = GetComponent<AudioSource>();
        if (anim.IsInTransition(0) && !audio.isPlaying)
        {
            audio.clip = doorSwitchClip;
            audio.Play();
        }
    }
}
