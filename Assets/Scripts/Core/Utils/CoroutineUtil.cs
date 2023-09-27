using System;
using System.Collections;
using UnityEngine;

namespace S_Utils
{
    public class CoroutineUtil : MonoBehaviour
    {
        public static CoroutineUtil Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Coroutine StartCoroutineExternal(IEnumerator enumerator)
        {
            return StartCoroutine(enumerator);
        }
        public void StopCoroutineExternal(Coroutine enumerator)
        {
            StopCoroutine(enumerator);
        }
    }
}