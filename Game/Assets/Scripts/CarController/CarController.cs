using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
namespace Car.CarController
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] [Range(0.001f, 100)] private float Speed;
        [SerializeField] [Range(0.001f, 100)] private float TurnSpeed;
        [SerializeField] private InputManagement.InputManagement Manager;
        [SerializeField] private Transform CenterOfMass;
        [SerializeField] private LayerMask GroundLayer;

        private Rigidbody Rigidbody;
        private bool IsCarGrounded;
        void Start()
        {
            Manager.OnInputChanged += OnInputChanged;
            this.transform.position = Rigidbody.position;
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.centerOfMass = CenterOfMass.localPosition;
        }

        private void OnInputChanged(float Horizontal, float Vertical)
        {

            //Forward and back
            RaycastHit Hit;
            IsCarGrounded = Physics.Raycast(transform.position, -transform.up, out Hit, 1f, GroundLayer);
            if (!IsCarGrounded)
            {
                if (Vertical > 0)
                    this.transform.Translate(Vector3.forward * Time.deltaTime * Speed * Vertical);
                else if (Vertical < 0)
                    this.transform.Translate(Vector3.forward * Time.deltaTime * 10 * Vertical);
            }
            //Right and left
            if (Horizontal != 0 && Vertical > 0)
                this.transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime * Horizontal);
            else if (Vertical < 0 && Horizontal != 0)
                this.transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime * -Horizontal);

        }
        private void FixedUpdate()
        {







        }

        //////////private void FixedUpdate()
        //////////{
        //////////    if (isCarGrounded)
        //////////        Sphere.AddForce(transform.forward * MoveInput, ForceMode.Acceleration);
        //////////    else
        //////////        Sphere.AddForce(transform.up * -20f);
        //////////}

        //public Transform FrontTarget;
        //public Transform BackTarget;


    }
}
