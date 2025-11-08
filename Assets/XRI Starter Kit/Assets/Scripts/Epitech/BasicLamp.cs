using MikeNspired.XRIStarterKit;
using UnityEngine;

public class BasicLamp : MonoBehaviour
{
    public Light lampLight;
    public Renderer rend;
    public bool isEnabled = true;
    private void Start()
    {
        lampLight.enabled = isEnabled;
        rend.enabled = isEnabled;
    }

    public void SwitchState()
    {
        isEnabled = !isEnabled;
        lampLight.enabled = isEnabled;
        rend.enabled = isEnabled;
    }
}