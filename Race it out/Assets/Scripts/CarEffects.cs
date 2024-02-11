using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour
{
    [SerializeField] TrailRenderer[] skidTrails;
    bool isBraking = false;
    float brakeInput;
    private void FixedUpdate()
    {
        brakeInput = SimpleInput.GetAxis("Jump");
        isBraking = brakeInput > 0.1 ? true : false;// Input.GetKey(KeyCode.Space);
        if (isBraking)
        {
            foreach(TrailRenderer trail in skidTrails)
            {
                trail.emitting = true;
            }
        }
        else
        {
            foreach (TrailRenderer trail in skidTrails)
            {
                trail.emitting = false;
            }
        }
    }
}
