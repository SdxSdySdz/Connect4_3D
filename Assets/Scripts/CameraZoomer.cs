using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoomer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _zoomSpeed;

    private Vector3 ZoomDirection => (_target.position - transform.position).normalized;


    private void Update()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue > 0.1)
        {
            transform.Translate(ZoomDirection * _zoomSpeed, Space.World);
        }
        else if (scrollValue < -0.1)
        {
            transform.Translate(-ZoomDirection * _zoomSpeed, Space.World);
        }
    }
}
