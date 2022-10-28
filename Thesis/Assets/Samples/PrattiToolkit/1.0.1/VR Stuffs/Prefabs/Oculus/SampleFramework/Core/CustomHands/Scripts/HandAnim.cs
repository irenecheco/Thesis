using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;

public class HandAnim : MonoBehaviour
{
    protected InputDevice controller;
    public InputDeviceCharacteristics desiredCharacteristics = InputDeviceCharacteristics.HeldInHand |InputDeviceCharacteristics.Controller;
    public Animator m_animator = null;

    public const string ANIM_LAYER_NAME_POINT = "Point Layer";
    public const string ANIM_LAYER_NAME_THUMB = "Thumb Layer";
    public const string ANIM_PARAM_NAME_FLEX = "Flex";
    public const string ANIM_PARAM_NAME_POSE = "Pose";

    private int m_animLayerIndexThumb = -1;
    private int m_animLayerIndexPoint = -1;
    private int m_animParamIndexFlex = -1;
    private int m_animParamIndexPose = -1;
    private Collider[] m_colliders = null;

    public float anim_frames = 4f;
    private float grip_state = 0f;
    private float trigger_state = 0f;
    private float triggerCap_state = 0f;
    private float thumbCap_state = 0f;

    private float grip_state_delta;
    private float trigger_state_delta;
    private float triggerCap_state_delta;
    private float thumbCap_state_delta;

    private void Awake()
    {
        // animator =
        // controller.GetComponent<XRController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_colliders = this.GetComponentsInChildren<Collider>().Where(childCollider => !childCollider.isTrigger).ToArray();
        for (int i = 0; i < m_colliders.Length; ++i)
        {
            Collider collider = m_colliders[i];
            // collider.transform.localScale = new Vector3(COLLIDER_SCALE_MIN, COLLIDER_SCALE_MIN, COLLIDER_SCALE_MIN);
            collider.enabled = true;
        }
        m_animLayerIndexPoint = m_animator.GetLayerIndex(ANIM_LAYER_NAME_POINT);
        m_animLayerIndexThumb = m_animator.GetLayerIndex(ANIM_LAYER_NAME_THUMB);
        m_animParamIndexFlex = Animator.StringToHash(ANIM_PARAM_NAME_FLEX);
        m_animParamIndexPose = Animator.StringToHash(ANIM_PARAM_NAME_POSE);

        GetController();

        // var inputDevices = new List<UnityEngine.XR.InputDevice>();
        // UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        //
        // foreach (var device in inputDevices)
        // {
        //     Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        //     solutionText.SetText(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        //
        // }
    }

    // Update is called once per frame
    void Update()
    {
#if !INPUT_SYSTEM_1_1_OR_NEWER // 1.1.0-preview.2 or newer, including pre-release

        if (!controller.isValid)
        {
            GetController();
            return;
        }

        if (controller.TryGetFeatureValue(CommonUsages.grip, out float gripTarget))
        {
            // solutionText.SetText(gripValue.ToString());
            grip_state_delta = gripTarget - grip_state;
            if (grip_state_delta > 0f)
            {
                grip_state = Mathf.Clamp(grip_state + 1 / anim_frames, 0f, gripTarget);
            }
            else if (grip_state_delta < 0f)
            {
                grip_state = Mathf.Clamp(grip_state - 1 / anim_frames, gripTarget, 1f);
            }
            else
            {
                grip_state = gripTarget;
            }

            m_animator.SetFloat(m_animParamIndexFlex, grip_state);
        }

        if (controller.TryGetFeatureValue(CommonUsages.trigger, out float triggerTarget))
        {

            trigger_state_delta = triggerTarget - trigger_state;
            if (trigger_state_delta > 0f)
            {
                trigger_state = Mathf.Clamp(trigger_state + 1 / anim_frames, 0f, triggerTarget);
            }
            else if (trigger_state_delta < 0f)
            {
                trigger_state = Mathf.Clamp(trigger_state - 1 / anim_frames, triggerTarget, 1f);
            }
            else
            {
                trigger_state = triggerTarget;
            }

            m_animator.SetFloat("Pinch", trigger_state);
        }

        if (controller.TryGetFeatureValue(CommonUsages.indexTouch, out float triggerCapTarget))
        {
            triggerCap_state_delta = triggerCapTarget - triggerCap_state;
            if (triggerCap_state_delta > 0f)
            {
                triggerCap_state = Mathf.Clamp(triggerCap_state + 1 / anim_frames, 0f, triggerCapTarget);
            }
            else if (triggerCap_state_delta < 0f)
            {
                triggerCap_state = Mathf.Clamp(triggerCap_state - 1 / anim_frames, triggerCapTarget, 1f);
            }
            else
            {
                triggerCap_state = triggerCapTarget;
            }

            m_animator.SetLayerWeight(m_animLayerIndexPoint, 1f - triggerCap_state);
        }

        if (controller.TryGetFeatureValue(CommonUsages.thumbTouch, out float thumbCapTarget))
        {
            thumbCap_state_delta = thumbCapTarget - thumbCap_state;
            if (thumbCap_state_delta > 0f)
            {
                thumbCap_state = Mathf.Clamp(thumbCap_state + 1 / anim_frames, 0f, thumbCapTarget);
            }
            else if (thumbCap_state_delta < 0f)
            {
                thumbCap_state = Mathf.Clamp(thumbCap_state - 1 / anim_frames, thumbCapTarget, 1f);
            }
            else
            {
                thumbCap_state = thumbCapTarget;
            }

            m_animator.SetLayerWeight(m_animLayerIndexThumb, 1f - thumbCap_state);
        }
#endif
    }

    private void GetController()
    {
        var cl = new List<UnityEngine.XR.InputDevice>();

        //UnityEngine.XR.InputDevices.GetDevicesAtXRNode(handNode, cl);
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, cl);

        if (cl.Count == 1)
        {
            controller = cl[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", controller.name, controller.role.ToString()));
        }
        else if (cl.Count != 1)
        {
            //Debug.Log("Found no/more than one left hand!");
        }


        //foreach (var device in cl)
        //{
        //    Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
        //}
    }



}
