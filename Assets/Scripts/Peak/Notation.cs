using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class Notation : MonoBehaviour
{
    private Text _text;
    private Transform _target;
    
    public float Transparency
    {
        set
        {
            Color oldColor = _text.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, value);
            _text.color = newColor;
        }
    }

    private void Start()
    {
        _target = GetComponent<Canvas>().worldCamera.transform;
        _text = GetComponentInChildren<Text>();
    }


    private void Update()
    {
        if (_target != null)
        {
            OrientToTarget();
        }
    }


    private void OrientToTarget()
    {
        transform.LookAt(_target);
        transform.Rotate(Vector3.up, 180);
    }
}
