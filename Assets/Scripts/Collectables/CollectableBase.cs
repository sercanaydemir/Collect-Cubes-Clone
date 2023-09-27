using System;
using CaseStudy.AI;
using CaseStudy.Core.Managers;
using CaseStudy.Core.Pools;
using CaseStudy.Core.Utils;
using CaseStudy.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CaseStudy.Collectables
{
    public abstract class CollectableBase : MonoBehaviour,ICollectable
    {
        private Rigidbody _rb;
        public CollectedItemType itemType;
        public CollectedItemType ItemType => itemType;
        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public abstract void SetMaterial(Material material);
        protected abstract void SetLayer(int layerID);

        protected virtual void SetItemType(CollectedItemType item)
        {
            itemType = item;
        }

        protected void SetRigidbodyKinematic(bool isKinematic)
        {
            _rb.isKinematic = isKinematic;
        }
        
        public void CollectItemFromCollector(CollectedItemType type, Material material, int layerId)
        {
            SetItemType(type);
            SetMaterial(material);
            SetLayer(layerId);
            LevelManager.RemoveCollectableCube(this);
        }

        public void CollectItemPlayer(CollectedItemType type,LayerMask layerMask,bool isKinematic)
        {
            SetItemType(type);
            SetRigidbodyKinematic(isKinematic);
            SetLayer(layerMask);
        }

        public void AttractToCollector(Vector3 collectorPos,float force)
        {
            Vector3 dir = collectorPos-transform.position;
            
            _rb.AddForce(dir.normalized*force,ForceMode.Impulse);
        }

        private void EventManagerOnOnGameEnd()
        {
            ResetItem();
            CollectablePool.Instance.ReturnToPool(this);
        }

        public void ResetItem()
        {
            _rb.velocity = Vector3.zero;
            _rb.isKinematic = false;
            itemType = CollectedItemType.None;
            gameObject.layer = LayerMask.NameToLayer("CollectableItem");
        }

        private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            EventManagerOnOnGameEnd();
        }

        void ExitPlayer()
        {
            itemType = CollectedItemType.None;
            gameObject.layer = LayerMask.NameToLayer("CollectableItem");

        }

        private void OnTriggerExit(Collider other)
        {
            if(gameObject.layer == LayerMask.NameToLayer("CollectedItem")) return;
            
            if (other.TryGetComponent(out PlayerController controller) ||
                other.TryGetComponent(out AIController aiController))

            {
                ExitPlayer();
            }
        }

        private void OnEnable()
        {
            EventManager.OnGameEnd += EventManagerOnOnGameEnd;
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
        }

        private void OnDisable()
        {
            EventManager.OnGameEnd -= EventManagerOnOnGameEnd;
            SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
            
        }
    }

    public interface ICollectable
    {
        public CollectedItemType ItemType { get; }
        public void CollectItemFromCollector(CollectedItemType type, Material material, int layerId);
        public void CollectItemPlayer(CollectedItemType type,LayerMask layerMask,bool isKinematic);
        public void AttractToCollector(Vector3 dir,float force);
    }
}