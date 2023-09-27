using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.InputSystem
{
    public class InputController : MonoBehaviour , IPointerDownHandler, IPointerUpHandler,IDragHandler
    {
        private bool _isClickHold;
        private PointerEventData _eventData;
        public void OnPointerDown(PointerEventData eventData)
        {
            _eventData = eventData;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _eventData = null;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _eventData = eventData;
            OnPointerDelta?.Invoke(new Vector3(_eventData.delta.x,0,_eventData.delta.y));
        }

        #region events

        public static event Action<Vector3> OnPointerDelta;
        
        #endregion


    }
}