using System;
using CaseStudy.Core.Managers;
using CaseStudy.Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaseStudy.UI.Core
{
    public class StateSelectionButton : MonoBehaviour
    {
        public GameState state;

        private Button _button;
        private TextMeshProUGUI _nameText;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _nameText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            _button.onClick.AddListener(ChangeGameState);
            _nameText.text = state.ToString();
        }

        private void ChangeGameState()
        {
            EventManager.ChangeGameState(state);
        }
    }
}