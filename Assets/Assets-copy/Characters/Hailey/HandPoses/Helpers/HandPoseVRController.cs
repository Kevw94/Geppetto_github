using UnityEngine;
using UnityEngine.XR;

public class HandPoseVRController : MonoBehaviour
{
    public HandPoseApplier handApplier;

    public HandPoseSO relaxPose;
    public HandPoseSO gripPose;
    public HandPoseSO triggerPose;
    public HandPoseSO grabPose;

    public XRNode xrNode = XRNode.LeftHand;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(xrNode);
        if (!device.isValid) return;

        bool gripValue = false;
        bool triggerValue = false;

        device.TryGetFeatureValue(CommonUsages.gripButton, out gripValue);
        device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue);


        if (gripValue && triggerValue)
        {
            handApplier.pose = grabPose;
        }
        else if (gripValue)
        {
            handApplier.pose = gripPose;
        }
        else if (triggerValue)
        {
            handApplier.pose = triggerPose;
        }
        else
        {
            handApplier.pose = relaxPose;
        }

        handApplier.ApplyPose();
    }
}
