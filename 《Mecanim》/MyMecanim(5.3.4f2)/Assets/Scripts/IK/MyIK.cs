using UnityEngine;
using System.Collections;

public class MyIK : MonoBehaviour
{

    public Transform bodyObj = null;
    public Transform leftFootObj = null;
    public Transform rightFootObj = null;
    public Transform leftHandObj = null;
    public Transform rightHandObj = null;
    public Transform lookAtObj = null;
    private Animator avatar;
    private bool ikActive = false;


    // Use this for initialization
    void Start()
    {
        avatar = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ikActive)
        {
            if (bodyObj != null)
            {
                bodyObj.position = avatar.bodyPosition;
                bodyObj.rotation = avatar.bodyRotation;
            }

            if (leftFootObj != null)
            {
                leftFootObj.position = avatar.GetIKPosition(AvatarIKGoal.LeftFoot);
                leftFootObj.rotation = avatar.GetIKRotation(AvatarIKGoal.LeftFoot);
            }

            if (lookAtObj != null)
            {
                lookAtObj.position = avatar.bodyPosition + avatar.bodyRotation * new Vector3(0, 0.5f, 1);
            }
        }
    }



    private void OnAnimatorIK(int layerIndex)
    {
        if (avatar == null)
        {
            return;
        }
        if (ikActive)
        {
            avatar.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);
            avatar.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1.0f);
            avatar.SetLookAtWeight(1.0f, 0.3f, 0.6f, 1.0f, 0.5f);

            if (bodyObj != null)
            {
                avatar.bodyPosition = bodyObj.position;
                avatar.bodyRotation = bodyObj.rotation;
            }
            if (leftFootObj != null)
            {
                avatar.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootObj.position);
                avatar.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootObj.rotation);
            }
            if (lookAtObj != null)
            {
                avatar.SetLookAtPosition(lookAtObj.position);
            }
        }
        else
        {
            avatar.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            avatar.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            avatar.SetLookAtWeight(0.0f);
        }

    }



    private void OnGUI()
    {
        GUILayout.Label("激活IK，然后在场景中移动Effects对象观察效果");
        ikActive = GUILayout.Toggle(ikActive, "激活");
    }



}
