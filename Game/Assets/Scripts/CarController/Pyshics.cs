using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Car.CarController
{
    public class Pyshics : MonoBehaviour
    {
        private Rigidbody Rb { get; set; }

        private void Start()
        {
            Rb = this.GetComponent<Rigidbody>();
        }
        private void Update()
        {
            Rb.AddForce(new Vector3(10, 10, 10), ForceMode.Force);
        }
    }
}
