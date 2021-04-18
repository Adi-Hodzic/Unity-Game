using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Osiris.Controllers.CarController
{
    public class NavigationSystem : MonoBehaviour
    {
        [SerializeField] [ReorderableList] private List<GameObject> Wheels;
        [SerializeField] private GameObject Target;
        [SerializeField] private int HowMuchPositions;
        [SerializeField] private int MaxSpeed;
        [SerializeField] private int CurveSpeed;
        private NavMeshAgent navMeshAgent;
        private NavMeshPath Path { get; set; }
        private Vector3 Current { get; set; }
        private bool Ine = false;
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            Path = navMeshAgent.path;
            navMeshAgent.SetDestination(Target.transform.position);
            StartCoroutine(StartCoroutine());
            
        }

        IEnumerator StartCoroutine()
        {
            if (Path.corners.Length / 2 > 1)
                Current = Path.corners[Path.corners.Length / 2];
            else
                Current = Path.corners[0];

            float p = Current.x - navMeshAgent.transform.position.x;
            float a = Current.z - navMeshAgent.transform.position.z;
            if (p < HowMuchPositions && p > -HowMuchPositions && a < HowMuchPositions && a > -HowMuchPositions && Current != Path.corners[0])
            {
                navMeshAgent.speed = CurveSpeed;
                Ine = true;
            }
            else
            {
                if (Ine)
                {
                    yield return new WaitForSeconds(1);
                    if (navMeshAgent.speed < MaxSpeed)
                        navMeshAgent.speed += 2;
                    else
                        navMeshAgent.speed = MaxSpeed - 1;
                }
                else
                {
                    if (navMeshAgent.speed < MaxSpeed)
                        navMeshAgent.speed += 4;
                    else
                        navMeshAgent.speed = MaxSpeed - 1;
                }
            }
        }
    }
}
