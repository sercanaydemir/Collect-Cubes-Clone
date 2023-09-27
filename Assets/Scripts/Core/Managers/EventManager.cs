using System;
using CaseStudy.Core.Utils;
using UnityEngine;

namespace CaseStudy.Core.Managers
{
    public static class EventManager
    {
        public static event Action OnGameStarted;

        public static void GameStarted()
        {
            OnGameStarted?.Invoke();
        }
        
        public static event Action OnGameEnd;

        public static void GameEnd()
        {
            OnGameEnd?.Invoke();
        }

        public static event Action<GameState> OnChangeGameState;

        public static void ChangeGameState(GameState state)
        {
            OnChangeGameState?.Invoke(state);
        }
        
    }
}