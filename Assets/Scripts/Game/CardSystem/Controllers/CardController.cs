﻿using Config;
using Game.Managers;
using NaughtyBezierCurves;
using UnityEngine;
using Zenject;

namespace Game.CardSystem.Controllers
{
    public class CardController : MonoBehaviour
    {
        private BezierCurve3D _bezierCurve;
        private CardPoolManager _cardPoolManager;
        
        [Inject]
        private void OnInstaller(CardPoolManager cardPoolManager)
        {
            _cardPoolManager = cardPoolManager;
        }
        
        private void Awake()
        {
            _bezierCurve = GetComponentInChildren<BezierCurve3D>();

            float rotationZ = 55; //DegreeDiff
            float begin = 1f / GameConfig.PLAYER_DECK_COUNT / 2;
            float zPosition = 0;
            
            for (int i = 0; i < 11; i++)
            {
                var card = _cardPoolManager.Spawn();
                Vector3 newPos = _bezierCurve.GetPoint(begin);
                newPos.z = zPosition;
                card.transform.position = newPos;
                card.transform.eulerAngles = new Vector3(0,0,rotationZ);
                
                zPosition += 0.2f;
                rotationZ -= GameConfig.PLAYER_DECK_COUNT;
                begin += 1f / GameConfig.PLAYER_DECK_COUNT;
            }
        }
    }
}
