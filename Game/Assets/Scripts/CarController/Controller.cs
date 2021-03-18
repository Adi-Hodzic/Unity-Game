using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Car.CarController
{

    public class Controller : MonoBehaviour
    {
        [SerializeField]
        [Range(0.1f, 100f)]
        private float Speed;

        private void Update()
        {
 
            if (this.transform.rotation.eulerAngles.y > -40 && this.transform.rotation.eulerAngles.y < 40)
            {
                if (Input.GetKey(KeyCode.A))
                    this.transform.Rotate(0, Speed * Time.deltaTime, 0);
                if (Input.GetKey(KeyCode.D))
                    this.transform.Rotate(0, -Speed * Time.deltaTime, 0);
            }

        }

        //private void Start()
        //{
        //    InputManager.OnInputChanged += OnInputChanged;
        //}
        //private void OnInputChanged(float horizontal)
        //{
        //    this.transform.Rotate(horizontal * Time.deltaTime);
        //}
    }
}
