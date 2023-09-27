using System;
using CaseStudy.Core.Managers;
using CaseStudy.Core.Utils;
using S_Utils;
using UnityEngine;

namespace CaseStudy.UI.Core
{
    public class UIScreen : MonoBehaviour
    {
        [SerializeField] private UIScreenType type;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnOnChangeUIScreenType(UIScreenType obj)
        {
            _canvasGroup.SetActive(obj == type);
        }

        private void EventManagerOnOnGameEnd()
        {
            OnOnChangeUIScreenType(UIScreenType.GameEnd);
        }

        private void EventManagerOnOnGameStarted()
        {
            OnOnChangeUIScreenType(UIScreenType.Game);
        }
        
        private void OnEnable()
        {
            OnChangeUIScreenType += OnOnChangeUIScreenType;
            EventManager.OnGameStarted += EventManagerOnOnGameStarted;
            EventManager.OnGameEnd += EventManagerOnOnGameEnd;
        }

        private void OnDisable()
        {
            OnChangeUIScreenType -= OnOnChangeUIScreenType;
            EventManager.OnGameStarted -= EventManagerOnOnGameStarted;
            EventManager.OnGameEnd -= EventManagerOnOnGameEnd;
        }

        public static event Action<UIScreenType> OnChangeUIScreenType;

        public static void ChangeUIScreenType(UIScreenType type)
        {
            OnChangeUIScreenType?.Invoke(type);
        }
    }
}