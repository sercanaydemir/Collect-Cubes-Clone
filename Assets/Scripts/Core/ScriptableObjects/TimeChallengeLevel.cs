using System;
using System.Collections.Generic;
using UnityEngine;

namespace CaseStudy.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TimeChallengeData", menuName = "CaseStudy/GameStates/TimeChallengeData", order = 2)]
    public class TimeChallengeLevel : ScriptableObject
    {
        private const string Path = "GameStates/TimeChallengeData";
        public static TimeChallengeLevel Instance
        {
            get
            {
                if (_instance == null)
                   _instance = Resources.Load<TimeChallengeLevel>(Path);
                
                return _instance;
            }
        }
        private static TimeChallengeLevel _instance;

        [SerializeField] private List<TimeChallengeLevelData> allLevelData;

        [SerializeField] private int levelID;

        public TimeChallengeLevelData GetLevel()
        {
            return allLevelData[levelID];
        }
    }

    [Serializable]
    public struct TimeChallengeLevelData
    {
        public int levelTime;
    }
}