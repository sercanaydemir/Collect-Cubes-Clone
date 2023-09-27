using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CaseStudy.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "CaseStudy/Player/Data", order = 1)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private int currentPresetID;
        [SerializeField] private List<PlayerPreset> presets = new List<PlayerPreset>();

        public float MovementSpeed => presets[currentPresetID].movementSpeed;
        public float RotationSpeed => presets[currentPresetID].rotationSpeed;
        public Vector2 BoundsMax => GameData.Instance.AreaBoundsMax;
        public Vector2 BoundsMin => GameData.Instance.AreaBoundsMin;
    }

    [Serializable]
    public struct PlayerPreset
    {
        public string presetName;
        
        public float movementSpeed;
        public float rotationSpeed;
    }
}