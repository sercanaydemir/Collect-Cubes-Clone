using S_Utils;
using UnityEngine;

namespace CaseStudy.Movements
{
    public class PlayerMover
    {
        private Transform _transform;
        public PlayerMover(Transform transform)
        {
            _transform = transform;
        }

        public void MoveAndRotate(Vector3 targetPos,float moveSpeed,float rotateSpeed)
        {
            _transform.MoveTowards(targetPos,
                moveSpeed * Time.deltaTime);
            
            Vector3 dir = targetPos - _transform.position;

            if (dir != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(dir.normalized);
                _transform.Slerp(rotation, rotateSpeed * Time.smoothDeltaTime);
            }
        }
    }
}