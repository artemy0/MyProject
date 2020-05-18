using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Target;

    [Range(0f, 1f)]
    [SerializeField] private float SmoothSpeed = 0.8f;
    [SerializeField] private Vector3 Offset;

    private void FixedUpdate() //LateUpdate получается рвано
    {
        Vector3 desiredPosition = Target.position + Offset;
        Vector3 smoothedPossition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime * 40f);
        transform.position = smoothedPossition;
    }
}
