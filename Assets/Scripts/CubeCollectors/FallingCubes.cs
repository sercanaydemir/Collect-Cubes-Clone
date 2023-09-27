using System;
using CaseStudy.Collectables;
using CaseStudy.Core.Managers;
using CaseStudy.Core.Pools;
using CaseStudy.Core.Utils;
using UnityEngine;

namespace CaseStudy.CubeCollectors
{
    public class FallingCubes : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CollectableBase collectable))
            {
                LevelManager.RemoveCollectableCube(collectable);
                CollectablePool.Instance.ReturnToPool(collectable);
            }
        }
    }
}