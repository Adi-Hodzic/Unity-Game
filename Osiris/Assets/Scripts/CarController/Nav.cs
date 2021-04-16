using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Osiris.CarController
{
    public class Nav : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        [SerializeField] private GameObject cars;
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.updateRotation = false;

        }

        void Update()
        {
            navMeshAgent.SetDestination(cars.transform.position);
            transform.LookAt(cars.transform.position);
        }
    }
}
