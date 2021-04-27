using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace Osiris.Controllers.CarController
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField] [ReorderableList] List<Transform> SpawnPositions;
        [SerializeField] private GameObject Prefab;
        private int NumberOfBots = 4;
        private void Start()
        {
            for (int i = 0; i < NumberOfBots; i++)
            {
                Prefab.gameObject.name = $"Bot" + i;
                Prefab.GetComponent<Rigidbody>().mass = Random.Range(10, 20);
                Instantiate(Prefab, SpawnPositions[i]);
            }
            for (int i = 0; i < NumberOfBots; i++)
            {
                int number = Random.Range(0, NumberOfBots);
                GameObject.Find($"Bot" + i + $"(Clone)").GetComponent<BotController>().Target = GameObject.Find($"Bot" + number + $"(Clone)");
            }
        }
        //private void OnCollisionEnter(Collision collision)
        //{
        //    for (int i = 0; i < NumberOfBots; i++)
        //    {
        //        if (collision.gameObject.name == $"Bot" + i + "(Clone)")
        //        {
        //            GameObject.Find($"Bot" + i + "(Clone)").GetComponent<BotController>().GetComponent<MeshCollider>().enabled = true;
        //            GameObject.Find($"Bot" + i + "(Clone)").GetComponent<NavMeshAgent>().enabled = false;
        //        }
        //        //else
        //        //{
        //        //    GameObject.Find($"Bot" + i + "(Clone)").GetComponent<BotController>().GetComponent<MeshCollider>().enabled = false;
        //        //    GameObject.Find($"Bot" + i + "(Clone)").GetComponent<NavMeshAgent>().enabled = true;
        //        //}
        //    }
        //}
    }
}
