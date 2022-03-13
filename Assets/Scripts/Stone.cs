using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;


[RequireComponent(typeof(AudioSource))]
public class Stone : MonoBehaviour
{
    [Header("Last Move Animation")]
    [SerializeField] private float _duration;
    [SerializeField] private float _scaleMultiplier;

    [Header("Move Sounds")]
    [SerializeField] private List<AudioClip> _moveSounds;

    private TweenerCore<Vector3, Vector3, VectorOptions> _lastMoveAnimation;
    private AudioSource _audioSource;
    private static int _currentSoundIndex;
    private Vector3 _originScale;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayMoveSound();
    }
    
    public void StartLastMoveAnimation()
    {
        _originScale = transform.localScale;
        _lastMoveAnimation = transform
                                      .DOScale(transform.lossyScale * _scaleMultiplier, _duration)
                                      .SetLoops(-1, LoopType.Yoyo);
    }
    
    public void StopLastMoveAnimation()
    {
        if (_lastMoveAnimation == null) return;
        _lastMoveAnimation.Kill();
        
        transform.localScale = _originScale;
    }
    
    private void PlayMoveSound()
    {
        _audioSource.clip = _moveSounds[_currentSoundIndex];
        _audioSource.Play();

        _currentSoundIndex = (_currentSoundIndex + 1) % _moveSounds.Count;
    }
    
    private void OnDestroy()
    {
        transform.DOKill();
        Destroy(gameObject);
    }
}
