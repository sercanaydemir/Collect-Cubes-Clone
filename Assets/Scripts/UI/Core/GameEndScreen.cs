using System;
using CaseStudy.Core.GameStates;
using CaseStudy.Core.GameStates.States;
using CaseStudy.Core.Managers;
using CaseStudy.Core.ScriptableObjects;
using CaseStudy.Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CaseStudy.UI.Core
{
    public class GameEndScreen : MonoBehaviour,IPointerDownHandler
    {
        [SerializeField] private TextMeshProUGUI levelEndText;
        [SerializeField] private TextMeshProUGUI collectedCubeCount;
        
        private GameState _gameState;

        private void LevelCompleted()
        {
            switch (_gameState)
            {
                case GameState.Classic:
                    ClassicModeCompleted();
                    break;
                case GameState.TimeChallenge:
                    TimeChallengeCompleted();
                    break;
                case GameState.RivalAI:
                    RivalAIModCompleted();
                    break;
            }
            
        }

        private void RivalAIModCompleted()
        {
            levelEndText.text = "Level " + GameData.Instance.LevelID + " Completed";
            collectedCubeCount.text = "Score: " + GameStateManager.Instance.LevelScore;
        }

        private void TimeChallengeCompleted()
        {
            levelEndText.text = "Level " + GameData.Instance.LevelID + " Completed";
            collectedCubeCount.text = "Score: " + GameStateManager.Instance.LevelScore;
        }

        private void ClassicModeCompleted()
        {
            levelEndText.text = "Level " + GameData.Instance.LevelID + " Completed";
            collectedCubeCount.text = "Score: " + GameStateManager.Instance.LevelScore;
        }

        private void EventManagerOnOnChangeGameState(GameState obj)
        {
            _gameState = obj;
        }

        private void OnEnable()
        {
            EventManager.OnGameEnd += LevelCompleted;
            EventManager.OnChangeGameState += EventManagerOnOnChangeGameState;
        }

        private void OnDisable()
        {
            EventManager.OnGameEnd -= LevelCompleted;
            EventManager.OnChangeGameState -= EventManagerOnOnChangeGameState;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}