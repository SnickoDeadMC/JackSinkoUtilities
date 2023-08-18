using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

public class TweenHelper : MonoBehaviour
{
    
    [Header("Position")]
    [SerializeField] private bool doPosition;
    [ConditionalField(nameof(doPosition)), SerializeField] private float positionDuration;
    [ConditionalField(nameof(doPosition)), SerializeField, Range(0,1)] private float positionRandomisedDurationPercent = 0.1f;
    [ConditionalField(nameof(doPosition)), SerializeField] private float xMovement;
    [ConditionalField(nameof(doPosition)), SerializeField] private float yMovement;
    [ConditionalField(nameof(doPosition)), SerializeField] private float zMovement;
    [ConditionalField(nameof(doPosition)), SerializeField] private Ease positionEase;
    [ConditionalField(nameof(doPosition)), SerializeField, ReadOnly] private Vector3 defaultPosition;
    [ConditionalField(nameof(doPosition)), SerializeField, ReadOnly] private float finalPositionDuration;
    
    private Sequence currentTween;
    private bool initialised;

    private void Initialise()
    {
        initialised = true;
        if (transform is RectTransform rectTransform)
            defaultPosition = rectTransform.anchoredPosition;
        else defaultPosition = transform.position;

        float positionDurationAsPercent = positionRandomisedDurationPercent * positionDuration;
        finalPositionDuration = positionDuration *
                                Random.Range(positionDuration - positionDurationAsPercent, positionDuration + positionDurationAsPercent);
    }
    
    private void OnEnable()
    {
        StartTween();
    }

    private void OnDisable()
    {
        StopTween();
    }

    private void ResetTransform()
    {
        if (transform is RectTransform rectTransform)
            rectTransform.anchoredPosition = defaultPosition;
        else transform.position = defaultPosition;
    }
    
    private void StartTween()
    {
        if (!initialised)
            Initialise();
        
        ResetTransform();

        currentTween?.Kill();

        currentTween = DOTween.Sequence();
        
        if (doPosition)
            currentTween.Join(GetPositionTween());
        
        currentTween.SetLoops(-1, LoopType.Yoyo);
    }

    private void StopTween()
    {
        currentTween?.Kill();
        currentTween = null;
    }

    private Tween GetPositionTween()
    {
        Tween tween;
        if (transform is RectTransform rectTransform)
            tween = DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x,
                new Vector2(defaultPosition.x, defaultPosition.y) + new Vector2(xMovement, yMovement), finalPositionDuration);
        else tween = transform.DOMove(defaultPosition + new Vector3(xMovement, yMovement, zMovement), finalPositionDuration);
        
        tween.SetEase(positionEase);
        return tween;
    }
    
}
