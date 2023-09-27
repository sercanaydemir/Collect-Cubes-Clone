using System;
using CaseStudy.Core.Managers;
using CaseStudy.Core.Utils;
using S_Utils;
using TMPro;
using UnityEngine;

namespace CaseStudy.UI
{
    public class TimerUI : MonoBehaviour
    {
        public TextMeshProUGUI counterText;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void SetRemainingTime(int seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            counterText.text = t.Minutes.ToString() + ":" + t.Seconds.ToString();
        }

        private void EventManagerOnOnChangeGameState(GameState obj)
        {
            _canvasGroup.SetActive(obj == GameState.TimeChallenge);
        }

        private void OnEnable()
        {
            OnSetRemainingTime += SetRemainingTime;
            EventManager.OnChangeGameState += EventManagerOnOnChangeGameState;
        }

        private void OnDisable()
        {
            OnSetRemainingTime -= SetRemainingTime;
            EventManager.OnChangeGameState -= EventManagerOnOnChangeGameState;
        }

        #region events

        public static event Action<int> OnSetRemainingTime;

        public static void SetRemainingTimeEvent(int second)
        {
            OnSetRemainingTime?.Invoke(second);
        }
        
        #endregion

    }
}