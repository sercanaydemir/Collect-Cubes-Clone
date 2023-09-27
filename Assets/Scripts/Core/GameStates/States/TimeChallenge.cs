using System.Collections;
using CaseStudy.Core.Managers;
using CaseStudy.Core.ScriptableObjects;
using S_Utils;
using CaseStudy.UI;
using UnityEngine;


namespace CaseStudy.Core.GameStates.States
{
    public class TimeChallenge : BaseState
    {
        private WaitForSeconds _waitForOneSecond = new WaitForSeconds(1);
        private Coroutine _timerRoutine;
        
        private int _levelTime;
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            Debug.Log( "GameState: "+"TimeChallenge");
            AddEventListeners();
            //Spawn Level
            _levelTime = TimeChallengeLevel.Instance.GetLevel().levelTime;
        }

        public override void OnStateUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            RemoveEventListeners();
        }

        void AddEventListeners()
        {
            EventManager.OnGameStarted += StartTimer;
            EventManager.OnGameEnd += StopTimer;
        }

        void RemoveEventListeners()
        {
            EventManager.OnGameStarted -= StartTimer;
            EventManager.OnGameEnd -= StopTimer;

        }

        private void StartTimer()
        {
            if(_timerRoutine != null) return;
            _timerRoutine = CoroutineUtil.Instance.StartCoroutineExternal(Timer());
        }

        private void StopTimer()
        {
            if(_timerRoutine == null) return;
            CoroutineUtil.Instance.StopCoroutineExternal(_timerRoutine);
            _timerRoutine = null;
        }
        
        IEnumerator Timer()
        {
            while (_levelTime>0)
            {
                TimerUI.SetRemainingTimeEvent(_levelTime);
                yield return _waitForOneSecond;
                _levelTime--;
            }

            _timerRoutine = null;
            EventManager.GameEnd();
        }
        
        
        
    }
}