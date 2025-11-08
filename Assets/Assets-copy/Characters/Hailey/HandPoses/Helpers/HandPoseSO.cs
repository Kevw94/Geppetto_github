using UnityEngine;

[CreateAssetMenu(fileName = "NewHandPose", menuName = "HandPose/HandPose")]
public class HandPoseSO : ScriptableObject
{
    [System.Serializable]
    public class Joint
    {
        public string name;        // Nom du joint (Thumb, Index, etc.)
        public Quaternion rotation; // Rotation locale
    }

    public Joint[] joints; // Liste des joints de la main
}
