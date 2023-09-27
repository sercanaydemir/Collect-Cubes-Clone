using System;
using UnityEngine;

namespace CaseStudy.Core.Utils
{
    public class DontDestroyThisClass : MonoBehaviour
    {
        public static DontDestroyThisClass Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);
        }
    }
}