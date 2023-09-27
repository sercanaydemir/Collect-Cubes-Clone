using System;
using CaseStudy.Core.GameStates.States;
using CaseStudy.Core.Managers;
using CaseStudy.Core.ScriptableObjects;
using CaseStudy.Core.Utils;
using CaseStudy.UI;
using CaseStudy.UI.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CaseStudy.Core.GameStates
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance;
        
        private BaseState _currentState;
        private readonly ClassicGame _classicGame = new ClassicGame();
        private readonly RivalAI _rivalAI = new RivalAI();
        private readonly TimeChallenge _timeChallenge = new TimeChallenge();
        
        public int LevelScore { get; private set; }
        private int _bestScore;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            LoadData();
            ChangeGameState(GameState.Wait);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                ChangeGameState(GameState.TimeChallenge);
            if(Input.GetKeyDown(KeyCode.S))
                EventManager.GameStarted();
        }

        private void ChangeGameState(GameState states)
        {
            _currentState?.OnStateExit();
            
            switch (states)
            {
                case GameState.Classic:
                    _currentState = _classicGame;
                    break;
                case GameState.TimeChallenge:
                    _currentState = _timeChallenge;
                    break;
                case GameState.RivalAI:
                    _currentState = _rivalAI;
                    break;
                case GameState.Wait:
                    SetWaitState();
                    break;
            }
            
            _currentState?.OnStateEnter();
            
        }

        void LoadData()
        {
            _bestScore = GameData.Instance.BestScore;
            ScoreboardUI.SetScoreEvent(_bestScore);
        }

        void SetWaitState()
        {
            UIScreen.ChangeUIScreenType(UIScreenType.StateSelection);
        }

        private void CheckBestScore(int score)
        {
            LevelScore = score;
            if(_bestScore >= score) return;
            
            GameData.UpdateBestScore(score);
            ScoreboardUI.SetScoreEvent(score);
            _bestScore = score;
        }

        private void OnEnable()
        {
            EventManager.OnChangeGameState += ChangeGameState;
            OnCheckBestScore += CheckBestScore;
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
        }

        private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            ChangeGameState(GameState.Wait);
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
            EventManager.OnChangeGameState -= ChangeGameState;
            OnCheckBestScore -= CheckBestScore;
        }

        #region events

        private static event Action<int> OnCheckBestScore;

        public static void CheckBestScoreEvent(int score)
        {
            OnCheckBestScore?.Invoke(score);
        }

        #endregion
    }
}