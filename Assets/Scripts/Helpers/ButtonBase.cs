using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;


public class ButtonBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action OnClickEvent;

    private bool _isPressed;
    private Tween _tween;

    public Transform AnimationObject;
    private float _firstScale;

    public bool Enable = true;

    private void Start()
    {
        if(AnimationObject != null)
        {
            _firstScale = AnimationObject.localScale.x;
        }

        _firstScale = 1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!Enable)
            return;
        
        if (_isPressed)
        {
            _isPressed = false;
            if(_tween != null)
            {
                _tween.Kill();
            }

            if (AnimationObject != null)
            {
                _tween = AnimationObject.DOScale(_firstScale, 0.2f);
            }
        }
        OnClickEvent.SafeInvoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!Enable)
            return;
        
        _isPressed = true;
        if(_tween != null)
        {
            _tween.Kill();
        }

        if (AnimationObject != null)
        {
            _tween = AnimationObject.DOScale(_firstScale * 0.9f, 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isPressed)
        {
            _isPressed = false;
            if(_tween != null)
            {
                _tween.Kill();
            }

            if (AnimationObject != null)
            {
                _tween = AnimationObject.DOScale(_firstScale, 0.2f);
            }
        }
    }
}
