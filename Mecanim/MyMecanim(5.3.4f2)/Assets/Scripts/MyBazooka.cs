using UnityEngine;
using System.Collections;

public class MyBazooka : MonoBehaviour
{

    private Animator animator;
    public GameObject targetA = null;
    public GameObject leftHandPos = null;
    public GameObject rightHandPos = null;
    public GameObject bazoo = null;
    public GameObject bullet = null;
    public GameObject spawn = null;
    private bool load = false;


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null)
        {
            return;
        }


        if (animator)
        {
            animator.SetFloat("Aim", load ? 1 : 0, 0.1f, Time.deltaTime);

            float aim = animator.GetFloat("Aim");
            float fire = animator.GetFloat("Fire");

            if (Input.GetButton("Fire1") && fire < 0.01f && aim > 0.99)
            {
                animator.SetFloat("Fire", 1);

                if (bullet != null && spawn != null)
                {
                    GameObject newBullet = Instantiate(bullet, spawn.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;

                    Rigidbody rb = newBullet.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.velocity = spawn.transform.TransformDirection(Vector3.forward * 20);
                    }
                }
            }
            else
            {
                animator.SetFloat("Fire", 0, 0.1f, Time.deltaTime);
            }


            if (Input.GetButton("Fire2"))
            {
                if (load && aim > 0.99)
                {
                    load = false;
                }
                else if (!load && aim < 0.01f)
                {
                    load = true;
                }
            }


            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, 0.25f, Time.deltaTime);


        }



    }



    private void OnAnimatorIK(int layerIndex)
    {
        float aim = animator.GetFloat("Aim");

        if (layerIndex == 0)
        {
            if (targetA != null)
            {
                Vector3 target = targetA.transform.position;
                target.y = target.y + 0.2f * (target - animator.rootPosition).magnitude;
                animator.SetLookAtPosition(target);
                animator.SetLookAtWeight(aim, 0.5f, 0.5f, 0.0f, 0.5f);
                if (bazoo != null)
                {
                    float fire = animator.GetFloat("Fire");
                    Vector3 pos = new Vector3(0.195f, -0.0557f, -0.155f);
                    Vector3 scale = new Vector3(0.2f, 0.8f, 0.2f);
                    pos.x -= fire * 0.2f;
                    scale = scale * aim;
                    bazoo.transform.localScale = scale;
                    bazoo.transform.localPosition = pos;
                }
            }
        }

        if (layerIndex == 1)
        {
            if (leftHandPos != null)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos.transform.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPos.transform.rotation);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, aim);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, aim);
            }

            if (rightHandPos != null)
            {
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos.transform.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandPos.transform.rotation);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, aim);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, aim);
            }
        }




    }

    void OnGUI()
    {
        GUILayout.Label("按Fire1键发射炮弹");
        GUILayout.Label("按Fire2键抬起或放下火箭炮");
    }






}
