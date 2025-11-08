using UnityEngine;

public class HandPoseApplier : MonoBehaviour
{
    public HandPoseSO pose;      // ton ScriptableObject
    public Transform[] joints;   // les os de ta main dans l'ordre du ScriptableObject

    [ContextMenu("Apply Pose")]
    public void ApplyPose()
    {
        if (pose == null || joints == null || joints.Length == 0) return;
        if (pose.joints.Length != joints.Length) return;

        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].localRotation = pose.joints[i].rotation;
        }
        Debug.Log("Pose appliquée : " + pose.name);
    }

    [ContextMenu("Capture Pose")]
    public void CapturePose()
    {
        if (pose == null || joints == null || joints.Length == 0) return;

        // Crée la liste si nécessaire
        if (pose.joints == null || pose.joints.Length != joints.Length)
            pose.joints = new HandPoseSO.Joint[joints.Length];

        for (int i = 0; i < joints.Length; i++)
        {
            if (pose.joints[i] == null) pose.joints[i] = new HandPoseSO.Joint();
            pose.joints[i].name = joints[i].name;
            pose.joints[i].rotation = joints[i].localRotation;
        }

        // Marque l'asset comme modifié pour que Unity sauvegarde les changements
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(pose);
#endif

        Debug.Log("Pose capturée depuis la scène : " + pose.name);
    }
}
