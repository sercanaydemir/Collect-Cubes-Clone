using System;
using CaseStudy.Collectables;
using CaseStudy.Core.Utils;
using CaseStudy.Movements;
using CaseStudy.Core.ScriptableObjects;
using Core.InputSystem;
using UnityEngine;

namespace CaseStudy.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;

        private int CollectedPlayerLayer => LayerMask.NameToLayer("CollectedPlayer");
        private Vector3 _moveVec;
        private PlayerMover _playerMover;

        private void Awake()
        {
            _playerMover = new PlayerMover(transform);
            _moveVec = transform.position;
        }

        private void Update()
        {
            _playerMover.MoveAndRotate(_moveVec,_playerData.MovementSpeed,_playerData.RotationSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out ICollectable collectable)) return;
            
            if(collectable.ItemType == CollectedItemType.Player) return;
            
            collectable.CollectItemPlayer(CollectedItemType.Player,CollectedPlayerLayer,false);
            
            
        }

        private void InputControllerOnOnPointerDelta(Vector3 obj)
        {
            _moveVec = transform.position + obj.normalized;

            _moveVec = new Vector3(
                Mathf.Clamp(_moveVec.x, _playerData.BoundsMin.x, _playerData.BoundsMax.x), 
                _moveVec.y, 
                Mathf.Clamp(_moveVec.z, _playerData.BoundsMin.y, _playerData.BoundsMax.y));
        }

        private void OnEnable()
        {
            InputController.OnPointerDelta += InputControllerOnOnPointerDelta;
        }
        private void OnDisable()
        {
            InputController.OnPointerDelta -= InputControllerOnOnPointerDelta;
        }

    }
}