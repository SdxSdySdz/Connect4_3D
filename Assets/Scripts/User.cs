using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class User : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    public event UnityAction<Move> MoveSelected;
    
    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) == false) return;
        
        var hittedObject = hit.collider.gameObject;
        if (Input.GetMouseButtonDown(0) && hittedObject.TryGetComponent(out Peak peak))
        { 
            MoveSelected?.Invoke(peak.AssociatedMove);
        }
    }
}
