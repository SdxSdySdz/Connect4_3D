using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MoveButton : MonoBehaviour
{
    [SerializeField] private Game game;
    private Button _button;
    private Text _moveNotation;


    private void Start()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnMoveButtonClicked);

        _moveNotation = _button.GetComponentInChildren<Text>();
    }


    public void OnMoveButtonClicked()
    {
        game.OnMoveButtonClicked(_moveNotation.text);
    }


    private void OnMouseOver()
    {
        Debug.LogError("Over");
    }
}
