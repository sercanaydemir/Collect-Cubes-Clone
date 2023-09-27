using System;
using System.Collections.Generic;
using CaseStudy.Core.Managers;
using CaseStudy.Core.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CaseStudy.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameData", menuName = "CaseStudy/General/Data", order = 0)]
    public class GameData : ScriptableObject
    {
        private const string Path = "GameData";
        public static GameData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<GameData>(Path);
                
                return _instance;
            }
        }
        
        private static GameData _instance;
        
        public int BestScore
        {
            get
            {
                if (_bestScore == 0)
                    _bestScore = PlayerPrefs.GetInt("best_score", 0);
                return _bestScore;
            }
            set
            {
                PlayerPrefs.SetInt("best_score", value);
                _bestScore = value;
            }
        }

        public int LevelID
        {
            get
            {
                if(_levelID == -1 )
                    _levelID = PlayerPrefs.GetInt("LevelID",0);
                return _levelID;
            }
            set
            {
                PlayerPrefs.SetInt("LevelID",value);
                _levelID = value;
            }
        }

        private int _bestScore;
        private int _levelID = -1;

        [SerializeField] private Vector2 areaBoundsMax;
        [SerializeField] private Vector2 areaBoundsMin;

        public Vector2 AreaBoundsMax => areaBoundsMax;
        public Vector2 AreaBoundsMin => areaBoundsMin;

        public float AreaWidth => Mathf.Abs(areaBoundsMin.x) + Mathf.Abs(areaBoundsMax.x);

        public List<Level> allLevels = new List<Level>();

        private GameState _gameState;
        
        public Level GetCurrentLevel()
        {
            if(_levelID<allLevels.Count)
                return allLevels[LevelID];
            else
            {
                return allLevels[Random.Range(0, allLevels.Count)];
            }
        }

        public void LevelCompletedForTimeChallenge()
        {
            LevelID++;
        }


        private void SetBestScore(int score)
        {
            BestScore = score;
        }
        

        private void EventManagerOnOnGameEnd()
        {
            if(_gameState != GameState.TimeChallenge )
                LevelID++;
        }
        private void EventManagerOnOnChangeGameState(GameState obj)
        {
            _gameState = obj;
        }

        private void OnEnable()
        {
            OnUpdateBestScore += SetBestScore;
            EventManager.OnGameEnd += EventManagerOnOnGameEnd;
            EventManager.OnChangeGameState += EventManagerOnOnChangeGameState;
        }


        private void OnDisable()
        {
            OnUpdateBestScore -= SetBestScore;
            EventManager.OnGameEnd -= EventManagerOnOnGameEnd;
            EventManager.OnChangeGameState -= EventManagerOnOnChangeGameState;

        }

        #region events

        private static event Action<int> OnUpdateBestScore;

        public static void UpdateBestScore(int score)
        {
            OnUpdateBestScore?.Invoke(score);
        }

        #endregion
    }

    [Serializable]
    public struct Level
    {
        public Texture2D LevelTexture;
    }
}