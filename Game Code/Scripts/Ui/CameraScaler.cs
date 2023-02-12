using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public Camera cam;
    public float xRatio= 16, yRatio = 9;

    private void Start() {
        float targetRatio = xRatio / yRatio;
        cam.aspect = targetRatio;
    }
}
