using System;
using CaseStudy.Collectables;
using CaseStudy.Core.Managers;
using CaseStudy.Core.Utils;
using JetBrains.Annotations;
using Pathfinding;
using S_Utils.FoV;
using UnityEngine;
using UnityEngine.AI;

namespace CaseStudy.AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private Transform collectorPoint;
        [SerializeField] private Transform centerPoint;
        private AIDestinationSetter _destinationSetter;
        private FieldOfView _fieldOfView;
        private Transform _target;

        private bool _isCollectState;
        private int CollectedAILayer => LayerMask.NameToLayer("CollectedAI");
        private void Awake()
        {
            _destinationSetter = GetComponent<AIDestinationSetter>();
            _fieldOfView = GetComponent<FieldOfView>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(FindTarget),1,0.5f);
        }

        void FindTarget()
        {
            if(_target != null) return;
            
            
            _target = _fieldOfView.GetRandomTarget();
//            _target.position = new Vector3(_target.position.x,transform.position.y,_target.position.z);
            
            if(_target == null) return;
            _fieldOfView.StopScanTarget();
            
            GoToTarget();

            _isCollectState = true;
        }

        void GoToTarget()
        {
            if(_target == null) return;

            _destinationSetter.target = _target; 
            
            CancelInvoke(nameof(FindTarget));
            
            InvokeRepeating(nameof(CheckReachDestination),1,0.5f);
        }

        void CheckReachDestination()
        {

            if (_target == null) return;
            
            if (Vector3.Distance(transform.position,_target.position) < 1f)
            {
                if (_isCollectState)
                {
                    GoToCollector();
                }
                else
                {
                    _target = null;
                    _isCollectState = true;
                    _fieldOfView.StartScanTarget();
                    InvokeRepeating(nameof(FindTarget),1,0.5f);
                    
                }

            }
            

        }

        void GoToCollector()
        {
            _target = collectorPoint;
            _destinationSetter.target = collectorPoint;
            _isCollectState = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out ICollectable collectable)) return;
            
            if(collectable.ItemType == CollectedItemType.AI) return;
            
            collectable.CollectItemPlayer(CollectedItemType.AI,CollectedAILayer,false);
            
        }

        private void OnEnable()
        {
            EventManager.OnChangeGameState += EventManagerOnOnChangeGameState;
        }
        private void OnDisable()
        {
            EventManager.OnChangeGameState -= EventManagerOnOnChangeGameState;
        }

        private void EventManagerOnOnChangeGameState(GameState obj)
        {
            gameObject.SetActive(obj == GameState.RivalAI);
        }
    }
}