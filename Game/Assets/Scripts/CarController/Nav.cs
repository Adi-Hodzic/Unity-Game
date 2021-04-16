using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Player.CarController
{
    public class Nav : MonoBehaviour
    {
        private NavMeshAgent navAgent;
        [SerializeField] private GameObject cars;
        void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            navAgent.SetDestination(cars.transform.position);
        }
    }
}
