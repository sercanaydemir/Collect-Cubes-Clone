using CaseStudy.Core.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CaseStudy.UI.Core
{
    public class TapToStartScreen : MonoBehaviour ,IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            EventManager.GameStarted();
        }
    }
}