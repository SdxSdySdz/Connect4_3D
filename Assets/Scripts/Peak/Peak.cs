using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Peak : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardCoordinates;
    
    private Renderer _renderer;
    private Notation _notation;
    private PeakModel3D _model;

    public event Action<Peak> OnMouseEntered;
    public event Action OnMouseExited;

    public Move AssociatedMove { get; private set; }
    public float Height => _model.transform.lossyScale.y * 2f;
    public float Transparency
    {
        set
        {
            SetMaterialTransparency(value);
            _notation.Transparency = value;
        }
    }

    private void Awake()
    {
        _model = GetComponentInChildren<PeakModel3D>();
        _renderer = _model.gameObject.GetComponent<Renderer>();
        _notation = GetComponentInChildren<Notation>();
    }
    
    private void Start()
    {
        (int row, int column) = GetBoardCoordinates();
        AssociatedMove = new Move(row, column);
    }
    
    public (int Row, int Column) GetBoardCoordinates()
    {
        return (_boardCoordinates.x, _boardCoordinates.y);
    }
    
    public Vector3 GetStoneSpawnPosition(int peak)
    {
        Vector3 basePosition = transform.position - transform.up * Height / 2f;
        Vector3 offset = transform.up * Height * (2 * peak + 1) / 8f;

        return basePosition + offset;
    }

    private void SetMaterialTransparency(float transparency)
    {
        Color oldColor = _renderer.material.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, transparency);
        _renderer.material.color = newColor;
    }
    
    private void OnMouseEnter()
    {
        OnMouseEntered?.Invoke(this);
    }
    
    private void OnMouseExit()
    {
        OnMouseExited?.Invoke();
    }
}
