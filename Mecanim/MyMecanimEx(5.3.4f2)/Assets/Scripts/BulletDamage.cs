using UnityEngine;
using System.Collections;

public class BulletDamage : MonoBehaviour
{


    const float m_DamageAmount = 0.25f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collider)
    {
        PlayerHurt player = collider.gameObject.GetComponent<PlayerHurt>();
        if (player)
        {
            player.DoDamage(m_DamageAmount);
        }
    }
}
