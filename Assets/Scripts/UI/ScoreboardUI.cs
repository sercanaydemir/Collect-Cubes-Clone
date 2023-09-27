using System;
using TMPro;
using UnityEngine;

namespace CaseStudy.UI
{
    public class ScoreboardUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        
        private void SetScore(int score)
        {
            textMeshProUGUI.text = score.ToString();
        }

        private void OnEnable()
        {
            OnSetScore += SetScore;
        }
        private void OnDisable()
        {
            OnSetScore -= SetScore;
        }

        #region events

        private static event Action<int> OnSetScore;

        public static void SetScoreEvent(int score)
        {
            OnSetScore?.Invoke(score);
        }
        
        #endregion
    }
}