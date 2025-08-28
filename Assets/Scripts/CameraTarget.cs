using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool pixelPerfect;
    [SerializeField] private int PPU = 16;
    [SerializeField] private Transform target;
    [SerializeField] private CameraData parametersCamera;

    private Vector3 currentVelocity;
    private Vector3 lookAheadPos;
    private float lastTargetDirectionX;
    private void LateUpdate()
    {
        if (target == null) return;

        FollowTarget();
    }
    private void FollowTarget()
    {
        Vector3 targetPosition = CalculateTargetPosition();
        Vector3 smoothPosition = SmoothFollow(targetPosition);

        mainCamera.transform.position = smoothPosition;
        float pixelSize = 1f / PPU;
        uiCamera.transform.position = mainCamera.transform.position;
    }

    private Vector3 CalculateTargetPosition()
    {
        Vector3 targetPos = target.position + (Vector3)parametersCamera.targetOffset;
        float targetDirectionX = Mathf.Sign(target.position.x - transform.position.x);

        if (Mathf.Abs(target.position.x - transform.position.x) > parametersCamera.lookAheadMoveThreshold && Mathf.Sign(targetDirectionX) == Mathf.Sign(lastTargetDirectionX))
        {
            lookAheadPos = Vector3.Lerp(lookAheadPos, Vector3.right * targetDirectionX * parametersCamera.lookAheadFactor, Time.deltaTime * parametersCamera.lookAheadReturnSpeed);
        }
        else
        {
            lookAheadPos = Vector3.Lerp(lookAheadPos, Vector3.zero, Time.deltaTime * parametersCamera.lookAheadReturnSpeed);
        }

        lastTargetDirectionX = targetDirectionX;
        return targetPos + lookAheadPos;
    }

    private Vector3 SmoothFollow(Vector3 targetPosition)
    {
        targetPosition.z = transform.position.z;
        return Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, parametersCamera.smoothTime, parametersCamera.maxSpeed, Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position + (Vector3)parametersCamera.targetOffset, 0.2f);
        Gizmos.DrawLine(transform.position, target.position + (Vector3)parametersCamera.targetOffset);
    }
}
