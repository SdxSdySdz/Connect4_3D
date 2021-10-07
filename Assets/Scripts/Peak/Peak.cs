using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Peak : MonoBehaviour
{
    [SerializeField] private Game game;
    private Move _move;
    private Renderer _renderer;
    private Notation _notation;
    private PeakModel3D _model;

    public Renderer Renderer => _renderer;
    public float Height => _model.transform.lossyScale.y / 2f;
    public float Transparency
    {
        get
        {
            return _renderer.material.color.a;
        }

        set
        {
            Color oldColor = _renderer.material.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, value);
            _renderer.material.color = newColor;

            oldColor = _notation.Text.color;
            newColor = new Color(oldColor.r, oldColor.g, oldColor.b, value);
            _notation.Text.color = newColor;
        }
    }


    private void Start()
    {
        _model = GetComponentInChildren<PeakModel3D>();
        _renderer = _model.gameObject.GetComponent<Renderer>();

        _notation = GetComponentInChildren<Notation>();

        (int row, int column) = GetBoardCoordinates();
        _move = new Move(row, column);
    }


    public (int Row, int Column) GetBoardCoordinates()
    {
        // All peaks has name = $"Peak{Row}{Column}"
        int row;
        int column;
        if (int.TryParse(name[4].ToString(), out row) && int.TryParse(name[5].ToString(), out column))
        {
            return (row, column);
        }
        else
        {
            throw new System.Exception("Incorrect peak name");
        }
    }


    private void OnMouseDown()
    {
        game.OnPeakClicked(_move);
    }


    private void OnMouseEnter()
    {
        game.OnPeakEnter(_move);
    }


    private void OnMouseExit()
    {
        game.OnPeakExit(_move);
    }
}
