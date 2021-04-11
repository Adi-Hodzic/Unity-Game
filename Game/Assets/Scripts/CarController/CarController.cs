using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
namespace Car.CarController
{
    public class CarController : MonoBehaviour
    {
        [ReorderableList] [SerializeField] private List<GameObject> Wheels;
        [SerializeField] private InputManagement.InputManagement Manager;
        [SerializeField] private Transform CenterOfMass;
        [SerializeField] private LayerMask GroundLayer;
        [SerializeField] [Range(0.001f, 100)] private float MaxSpeedT;
        [SerializeField] [Range(0.001f, 20)] private float MaxSpeedR;
        [SerializeField] [Range(0.001f, 100)] private float TurnSpeed;
        [SerializeField] private float MaximumRotation;
        [SerializeField] private float RotationSpeed;

        private float CurrentRotation { get; set; }
        private float CurrentSpeed { get; set; }
        private float Last { get; set; }
        //private Rigidbody Rigidbody;
        //private bool IsCarGrounded;


        void Start()
        {
            Manager.OnInputChanged += OnInputChanged;
            Manager.OnBrake += OnBrake;
            //Rigidbody = GetComponent<Rigidbody>();
            //Rigidbody.centerOfMass = CenterOfMass.localPosition;
            //this.transform.position = Rigidbody.position;
        }
       
        private void OnBrake()
        {
            if (CurrentSpeed > 0) CurrentSpeed -= 1.5f;
            if (CurrentSpeed < 0) CurrentSpeed = 0;
        }
        private void OnInputChanged(float Horizontal, float Vertical)
        {
            Debug.Log(CurrentSpeed);
            if (Vertical != 0)
            {
                Last = Vertical;
            }

            if (Vertical > 0 || Vertical < 0)  
            {
                //Forward
                if (Vertical > 0)
                {
                    if (CurrentSpeed < MaxSpeedT)
                        CurrentSpeed += 1;
                    this.transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed * Vertical);
                    foreach (GameObject i in Wheels)
                        i.transform.Rotate(CurrentSpeed, i.transform.localRotation.y, i.transform.localRotation.z);
                }
               
                //Back
                else if (Vertical < 0 && CurrentSpeed<MaxSpeedR)
                {
                    if (CurrentSpeed < MaxSpeedR)
                        CurrentSpeed += 0.5f;
                    this.transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed * Vertical);
                    foreach (GameObject i in Wheels)
                        i.transform.Rotate(-CurrentSpeed, i.transform.localRotation.y, i.transform.localRotation.z);
                }
                //ControllingSpeedWhenUWantToGoBack
                else if (CurrentSpeed > MaxSpeedR && Vertical < 0)
                {
                    CurrentSpeed = 0;
                }
                //Limit
                else if (CurrentSpeed == MaxSpeedR || CurrentSpeed == MaxSpeedT)
                {
                    this.transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed * Vertical);
                }
                //Rotation
                if (Horizontal != 0 && Vertical > 0)
                {
                    this.transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime * Horizontal);
                }
                else if (Vertical < 0 && Horizontal != 0)
                {
                    this.transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime * -Horizontal);
                }
                //Speed-wheels
                else
                {
                    foreach (GameObject i in Wheels)
                        i.transform.Rotate(CurrentSpeed, i.transform.localRotation.y, i.transform.localRotation.z);
                }
            }
            else
            {
                if (CurrentSpeed > 0)
                {
                    if (Last > 0)
                    {
                        CurrentSpeed -= 0.2f;
                        this.transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed);
                        this.transform.Rotate(Vector3.up, CurrentSpeed * Time.deltaTime * Horizontal);
                    }
                    if (Last < 0)
                    {
                        CurrentSpeed -= 0.1f;
                        this.transform.Translate(Vector3.forward * Time.deltaTime * -CurrentSpeed);
                        this.transform.Rotate(Vector3.up, CurrentSpeed * Time.deltaTime * -Horizontal);
                    }
                }
            }

            //Rotation - WHEELS
            CurrentRotation = Horizontal * RotationSpeed * Time.deltaTime;
            if (CurrentRotation <= MaximumRotation || CurrentRotation >= -MaximumRotation)
            {
                Wheels[0].transform.localRotation = Quaternion.Euler(CurrentSpeed, CurrentRotation, Wheels[0].transform.localRotation.z);
                Wheels[1].transform.localRotation = Quaternion.Euler(CurrentSpeed, CurrentRotation, Wheels[1].transform.localRotation.z);
            }
        }
        //private void FixedUpdate()
        //{
        //if (CurrentSpeed != 0)
        //{
        //    CurrentSpeed -= 2;
        //    this.transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed * Vert);
        //}
        //}

        //////////private void FixedUpdate()
        //////////{
        //////////    if (isCarGrounded)
        //////////        Sphere.AddForce(transform.forward * MoveInput, ForceMode.Acceleration);
        //////////    else
        //////////        Sphere.AddForce(transform.up * -20f);
        //////////}

        //public Transform FrontTarget;
        //public Transform BackTarget;

        //RaycastHit Hit;
        //IsCarGrounded = Physics.Raycast(transform.position, -transform.up, out Hit, 1f, GroundLayer);
    }
}
