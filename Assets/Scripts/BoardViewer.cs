using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardViewer : MonoBehaviour
{
    
    [Header("Game prefabs")]
    [SerializeField] private GameObject _whiteStone;
    [SerializeField] private GameObject _blackStone;
    /*
    [SerializeField] private GameObject _peakPrefab;
    [SerializeField] private GameObject _platformPrefab;*/

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed;
    [Header("Highlighting")]
    [Range(0, 1)]
    [SerializeField] private float _unselectedPeakTransparency;
    [Range(0, 1)]
    [SerializeField] private float _duration;

    /*  
        private readonly int _rowCount = 4;
        private readonly int _columnCount = 4;
        private readonly float _radiusPercent = 0.75f;*/

    private GameObject[,,] _stones;
    private Peak[,] _peaks;
    private Stack<(int row, int column, int peak)> _spawnedIndicesOrder;

    private float _highlightingProgressValue;
    private Coroutine _highlightingPeakCoroutine;


    private void Start()
    {
        _stones = new GameObject[4, 4, 4];
        _peaks = new Peak[4, 4];
        _spawnedIndicesOrder = new Stack<(int row, int column, int peak)>();

        var peakContainer = GetComponentInChildren<PeakContainer>().gameObject;
        foreach (var peak in peakContainer.GetComponentsInChildren<Peak>())
        {
            (int row, int column) = peak.GetBoardCoordinates();
            _peaks[row, column] = peak;
        }
    }


    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(Vector3.up, -Input.GetAxis("Mouse X") * _rotationSpeed);
            // transform.Rotate(_observerTransform.right, Input.GetAxis("Mouse Y") * _rotationSpeed, Space.World);
        }
    }


    public void SpawnStone(PlayerColor playerColor, int row, int column, int peak)
    {
        Peak peakObject = _peaks[row, column];
        float peakHeight = peakObject.Height;

        GameObject stone;
        if (playerColor.IsWhite)
        {
            stone = _whiteStone;
        }
        else if (playerColor.IsBlack)
        {
            stone = _blackStone;
        }
        else
        {
            throw new System.Exception("");
        }

        stone = Instantiate(stone, peakObject.gameObject.transform.position + Vector3.up * peakHeight * 2, Quaternion.identity, peakObject.gameObject.transform);
        stone.transform.localScale = new Vector3(1, peakHeight, 1);
        _stones[row, column, peak] = stone;
        _spawnedIndicesOrder.Push((row, column, peak));
    }

    public void DeleteAllStones()
    {
        foreach (var stone in _stones)
        {
            Destroy(stone);
        }
    }

    public void DeleteLastStone()
    {
        if (_spawnedIndicesOrder.Count > 0)
        {
            var lastIndex = _spawnedIndicesOrder.Pop();
            GameObject lastStone = _stones[lastIndex.row, lastIndex.column, lastIndex.peak];

            Destroy(lastStone);

            _stones[lastIndex.row, lastIndex.column, lastIndex.peak] = null;
        }
    }


    public void DeleteStone(int row, int column)
    {
        throw new NotImplementedException();
    }


    public void ResetHighlighting()
    {
        if (Input.GetMouseButton(1) == false)
        {
            if (_highlightingPeakCoroutine != null)
            {
                StopCoroutine(_highlightingPeakCoroutine);
            }

            _highlightingPeakCoroutine = StartCoroutine(ChangePeakTransparencyExcept(Move.EmptyMove, _unselectedPeakTransparency, 1f, 0.1f));
        }
            

       
/*        for (int row = 0; row < _peaks.GetLength(0); row++)
        {
            for (int column = 0; column < _peaks.GetLength(1); column++)
            {
                _peaks[row, column].Transparency = 1f;
            }
        }*/
 
    }

    public void HighlightPeak(Move move)
    {
        
        if (Input.GetMouseButton(1) == false)
        {
            _peaks[move.Row, move.Column].Transparency = 1f;

            if (_highlightingPeakCoroutine != null)
            {
                StopCoroutine(_highlightingPeakCoroutine);
            }

            _highlightingPeakCoroutine = StartCoroutine(ChangePeakTransparencyExcept(move, 1f, _unselectedPeakTransparency, 0.1f));
        }
    }


    public void HighlightWinningStones(List<(int Row, int Column, int Peak)> winningIndices)
    {

    }


    private IEnumerator ChangePeakTransparencyExcept(Move move, float fromTransparency, float toTransparency, float duration)
    {
        Debug.LogError("Start cort");
        var waitForEndOfFraim = new WaitForEndOfFrame();
        float transparency = fromTransparency;
        while (Mathf.Abs(transparency - toTransparency) > 0)
        {
            for (int row = 0; row < _peaks.GetLength(0); row++)
            {
                for (int column = 0; column < _peaks.GetLength(1); column++)
                {
                    if (row != move.Row || column != move.Column)
                    {
                        _peaks[row, column].Transparency = transparency;
                    }
                }
            }

            transparency = Mathf.MoveTowards(transparency, toTransparency, Time.deltaTime / duration);
            yield return waitForEndOfFraim;
        }

        yield break;
    }

    /*    private void SpawnEmptyBoard(Vector3 position)
        {
            var platform = Instantiate(_platformPrefab, position, Quaternion.identity, transform);

            float platformHeight = _platformPrefab.transform.lossyScale.y;
            float platformRadius = _platformPrefab.transform.lossyScale.x;

            float peakHeight = _peakPrefab.transform.lossyScale.y;
            float peakRadius = _peakPrefab.transform.lossyScale.x / 2f;

            float scaleMultiplier = _radiusPercent * platformRadius * Mathf.Sqrt(2) / (_rowCount - 1);

            for (int row = 0; row < _rowCount; row++)
            {
                for (int column = 0; column < _columnCount; column++)
                {
                    float z = column - (_rowCount - 1) / 2f;
                    float x = -row + (_rowCount - 1) / 2f;

                    x *= scaleMultiplier;
                    z *= scaleMultiplier;

                    Vector3 peakPosition = new Vector3(x, platformHeight + peakHeight, z);
                    var peak = Instantiate(_peakPrefab, peakPosition, Quaternion.identity, transform);
                    peak.name = $"Peak{row}{column}";

                    _peaks[row, column] = peak;
                }
            }
        }*/
}
