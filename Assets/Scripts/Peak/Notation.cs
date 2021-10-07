using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class Notation : MonoBehaviour
{
    private Text _text;
    private Transform _target;

    public Text Text => _text;

    private void Start()
    {
        _target = GetComponent<Canvas>().worldCamera.transform;
        _text = GetComponentInChildren<Text>();
    }


    private void Update()
    {
        if (_target != null)
        {
            transform.LookAt(_target);
            transform.Rotate(Vector3.up, 180);
        }
    }
}
