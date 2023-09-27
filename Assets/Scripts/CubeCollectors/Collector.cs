using CaseStudy.Collectables;
using CaseStudy.Core.GameStates;
using CaseStudy.Core.Utils;
using UnityEngine;

namespace CaseStudy.CubeCollectors
{
    public class Collector : MonoBehaviour
    {
        [SerializeField] private CollectedItemType itemType;
        [SerializeField] private float attractionForce;
        [SerializeField] private Material itemTargetMaterial;
        [SerializeField] private LayerMask itemTargetLayerMask;

        private int _collectedLayerID => LayerMask.NameToLayer("CollectedItem");
        private int _playerScore;
        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out ICollectable collectable)) return;
            
            if(collectable.ItemType != itemType) return;
            CollectItem(collectable);
        }

        private void CollectItem(ICollectable collectable)
        {
            collectable.CollectItemFromCollector(itemType,itemTargetMaterial,_collectedLayerID);
            collectable.AttractToCollector(transform.position,attractionForce);

            if (itemType == CollectedItemType.Player)
            {
                _playerScore++;
                GameStateManager.CheckBestScoreEvent(_playerScore);
            }
        }
        
        
        
    }
}