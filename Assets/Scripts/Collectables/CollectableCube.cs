using System;
using UnityEngine;

namespace CaseStudy.Collectables
{
    public class CollectableCube : CollectableBase
    {
        private MeshRenderer _meshRenderer;

        protected override void Awake()
        {
            base.Awake();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public override void SetMaterial(Material material)
        {
            _meshRenderer.sharedMaterial = material;
        }

        protected override void SetLayer(int layerID)
        {
            gameObject.layer = layerID;
        }
        
    }
}