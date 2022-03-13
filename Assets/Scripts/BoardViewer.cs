using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardViewer : MonoBehaviour
{
    [SerializeField] private List<Peak> _peaksList;
    
    [Header("Game prefabs")]
    [SerializeField] private Stone _whiteStone;
    [SerializeField] private Stone _blackStone;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed;

    [Header("Highlighting")]
    [Range(0, 1)]
    [SerializeField] private float _unselectedPeakTransparency;
    [SerializeField] private float _highlightingDuration;
    [SerializeField] private float _unhighlightingDuration;
    
    private Stone[,,] _stones;
    private Peak[,] _peaks;
    private Stack<(int row, int column, int peak)> _spawnedIndicesOrder;
    private Coroutine _highlightingPeakCoroutine;
    
    private void OnEnable()
    {
        foreach (var peak in _peaks)
        {
            peak.OnMouseEntered += HighlightPeak;
            peak.OnMouseExited += ResetHighlighting;
        }
    }
    
    private void OnDisable()
    {
        foreach (var peak in _peaks)
        {
            peak.OnMouseEntered -= HighlightPeak;
            peak.OnMouseExited -= ResetHighlighting;
        }
    }

    private void Awake()
    {
        _stones = new Stone[4, 4, 4];
        _peaks = new Peak[4, 4];
        _spawnedIndicesOrder = new Stack<(int row, int column, int peak)>();
        
        if (_peaksList.Count != 16)
            throw new Exception();

        InitPeaks();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(Vector3.up, -Input.GetAxis("Mouse X") * _rotationSpeed);
        }
    }

    public void OnMoveApplied(GameState gameState)
    {
        
        
        Move move = gameState.LastMove;
        int row = move.Row;
        int column = move.Column;
        int peak = gameState.Board.GetHighestPeak(row, column);
        SpawnStone(gameState.PlayerColorToMove.Opposite, row, column, peak);
    }
    
    public void OnReturnMoveButtonClicked()
    {
        TryDeleteLastStone();
    }
    
    public void OnResetButtonClicked()
    {
        DeleteAllStones();
    }

    private void InitPeaks()
    {
        foreach (var peak in _peaksList)
        {
            (int row, int column) = peak.GetBoardCoordinates();
            _peaks[row, column] = peak;
        }
    }
    
    private void StopLastSpawnedStoneAnimation()
    {
        if (_spawnedIndicesOrder.Count > 0)
        {
            var lastIndex = _spawnedIndicesOrder.Peek();
            Stone lastStone = _stones[lastIndex.row, lastIndex.column, lastIndex.peak];
            lastStone.StopLastMoveAnimation();
        }
    }
    
    private void SpawnStone(PlayerColor playerColor, int row, int column, int peak)
    {
        StopLastSpawnedStoneAnimation();
        Stone stone = playerColor.IsWhite ? _whiteStone : _blackStone;
        Peak peakObject = _peaks[row, column];

        Vector3 spawnPosition = peakObject.GetStoneSpawnPosition(peak);

        stone = Instantiate(stone, spawnPosition, Quaternion.identity, peakObject.transform);
        stone.transform.localScale = new Vector3(1, peakObject.Height / 4f, 1);

        stone.StartLastMoveAnimation();

        _stones[row, column, peak] = stone;
        _spawnedIndicesOrder.Push((row, column, peak));
    }
    
    private void DeleteAllStones()
    {
        foreach (var stone in _stones)
        {
            Destroy(stone);
        }

        _stones = new Stone[4, 4, 4];
        _spawnedIndicesOrder = new Stack<(int row, int column, int peak)>();
    }
    
    private void TryDeleteLastStone()
    {
        if (_spawnedIndicesOrder.Count == 0) return;

        var lastIndex = _spawnedIndicesOrder.Pop();
        Stone lastStone = _stones[lastIndex.row, lastIndex.column, lastIndex.peak];

        Destroy(lastStone);
        _stones[lastIndex.row, lastIndex.column, lastIndex.peak] = null;
    }

    private void HighlightPeak(Peak peak)
    {
        ProvideHighlighting(ChangePeaksTransparencyExcept(peak, 1f, _unselectedPeakTransparency, _highlightingDuration));
    }
    
    private void ResetHighlighting()
    {
        ProvideHighlighting(ChangePeaksTransparency(_unselectedPeakTransparency, 1f, _unhighlightingDuration));
    }

    private void ProvideHighlighting(IEnumerator coroutine)
    {
        if (Input.GetMouseButton(1)) return;
        
        if (_highlightingPeakCoroutine != null)
        {
            StopCoroutine(_highlightingPeakCoroutine);
        }

        _highlightingPeakCoroutine = StartCoroutine(coroutine);
    }
    
    private IEnumerator ChangePeaksTransparencyExcept(Peak extraPeak, float fromTransparency, float toTransparency, float duration)
    {
        var waitForEndOfFrame = new WaitForEndOfFrame();
        float transparency = fromTransparency;
    
        if (extraPeak != null)
            extraPeak.Transparency = 1f;
        while (Mathf.Abs(transparency - toTransparency) > 0)
        {
            foreach (var peak in _peaks)
            {
                if (peak != extraPeak)
                {
                    peak.Transparency = transparency;
                }
            }

            transparency = Mathf.MoveTowards(transparency, toTransparency, Time.deltaTime / duration);
            yield return waitForEndOfFrame;
        }
    }
    
    private IEnumerator ChangePeaksTransparency(float fromTransparency, float toTransparency, float duration)
    {
        yield return ChangePeaksTransparencyExcept(null, fromTransparency, toTransparency, duration);
    }
}
