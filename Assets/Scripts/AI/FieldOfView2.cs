using System;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace AI
{
    public class FieldOfView2 : MonoBehaviour
    {
        
        
        public GameObject eyes;
        public float fieldOfViewAngle;
        public float lookRadius;

        public Transform _player;

        public bool PlayerSpotted { get; set; }

        public Transform Player => _player;

        public float DistanceToPlayer => Vector3.Distance(_player.transform.position, eyes.transform.position);
        
        private void Awake()
        {
            //_player = GameObject.FindWithTag("Player");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, lookRadius);

            Vector3 fovLine1 = Quaternion.AngleAxis(fieldOfViewAngle, transform.up) * transform.forward * lookRadius;
            Vector3 fovLine2 = Quaternion.AngleAxis(-fieldOfViewAngle, transform.up) * transform.forward * lookRadius;
            
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, (_player.transform.position - transform.position).normalized * lookRadius);
            
            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, transform.forward * lookRadius);
        }

        public static void InView(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
        {
            Collider[] overlaps = new Collider[10];
            int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

            for (int i = 0; i < count + 1; i++)
            {
               if (overlaps[i] != null)
                {
                   if (overlaps[i] == target)
                    {
                       Vector3 direction = (target.position - checkingObject.position).normalized;
                        direction.y *= 0;

                        float angle = Vector3.Angle(checkingObject.forward, direction);

                        if (angle <= maxAngle)
                        {
                            Ray ray = new Ray(checkingObject.position, 
                                target.position -checkingObject.position);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit, maxRadius))
                            {
                                if (hit.transform == target)
                                {
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Detect()
        {
            Vector3 direction = (_player.position - transform.position);
            float angle = Vector3.Angle(transform.forward, direction);

            if (angle <= fieldOfViewAngle)
            {
            }
        }
        private void Update()
        {
            //InView(transform, _player, fieldOfViewAngle, lookRadius);
        }
    }
}
