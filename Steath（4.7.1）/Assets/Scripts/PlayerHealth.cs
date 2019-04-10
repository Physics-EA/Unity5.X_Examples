using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public float health = 100f;
    public float resetAfterDeadTime = 5f;
    public AudioClip deathClip;
    private Animator anim;
    private PlayerMovement playerMovement;
    private HashIDs hash;
    private SceneFadeInOut sceneFadeInOut;
    private LastPlayerSighting lastPlayerSighting;
    private float timer;
    private bool playerDead;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        hash = GameObject.FindWithTag(Tags.GameController).GetComponent<HashIDs>();
        sceneFadeInOut = GameObject.FindWithTag(Tags.Fader).GetComponent<SceneFadeInOut>();
        lastPlayerSighting = GameObject.FindWithTag(Tags.GameController).GetComponent<LastPlayerSighting>();

    }

    void PlayerDying()
    {
        playerDead = true;
        anim.SetBool(hash.deadBool, playerDead);
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
    }

    void PlayerDead()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.dyingState)
        {
            anim.SetBool(hash.deadBool, false);
            playerMovement.enabled = false;
            lastPlayerSighting.position = lastPlayerSighting.resetPosition;
            GetComponent<AudioSource>().Stop();
        }
    }


    void LevelReset()
    {
        timer += Time.deltaTime;
        if (timer >= resetAfterDeadTime)
        {
            sceneFadeInOut.EndScene();
        }
    }


    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            if (!playerDead)
            {
                PlayerDying();
            }
            else
            {
                PlayerDead();
                LevelReset();
            }
        }
    }
}
