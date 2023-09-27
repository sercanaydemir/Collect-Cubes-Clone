using UnityEngine;

namespace S_Utils
{
    public static class ExtensionMethods
    {
        public static void LerpPos(this Transform t, Vector3 targetPos, float maxDistanceDelta)
        {
            t.position = Vector3.Lerp(t.position, targetPos, maxDistanceDelta);
        }
        public static void MoveTowards(this Transform t, Vector3 targetPos, float maxDistanceDelta)
        {
            t.position = Vector3.MoveTowards(t.position, targetPos, maxDistanceDelta);
        }

        public static void Slerp(this Transform t, Quaternion rotation, float speed)
        {
            t.rotation = Quaternion.Slerp(t.rotation, rotation, 
                speed);
        }

        public static bool IsRange(this float t, float minValue, float maxValue)
        {
            if (t >= minValue && t <= maxValue) return true;
            return false;
        }
        
        public static void SetActive(this CanvasGroup group, bool active)
        {
            if (active)
            {
                group.alpha = 1;
                group.interactable = true;
                group.blocksRaycasts = true;
            }
            else
            {
                group.alpha = 0;
                group.interactable = false;
                group.blocksRaycasts = false;
            }
        }
    }
}