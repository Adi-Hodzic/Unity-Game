using NaughtyAttributes;
using Osiris.Controllers.Core;
using System.Collections.Generic;
using UnityEngine;
namespace Osiris.Controllers.CarController
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] [ReorderableList] private List<GameObject> Wheels;
        [SerializeField] [Range(0.001f, 100)] private float MaxSpeedT;
        [SerializeField] [Range(0.001f, 20)] private float MaxSpeedR;
        [SerializeField] [Range(0.001f, 100)] private float TurnSpeed;
        [SerializeField] private Transform CenterOfMass;
        [SerializeField] private LayerMask GroundLayer;
        [SerializeField] private InputManagement Manager;
        [SerializeField] private float MaximumRotation;
        [SerializeField] private float RotationSpeed;

        private float CurrentRotation { get; set; }
        private float CurrentSpeed { get; set; }
        private float Last { get; set; }
        private Rigidbody Rigidbody;
        //private bool IsCarGrounded;

        void Start()
        {
            Manager.OnInputChanged += OnInputChanged;
            Manager.OnBrake += OnBrake;
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.centerOfMass = CenterOfMass.localPosition;
            this.transform.position = Rigidbody.position;
        }

        private void OnBrake()
        {
            if (CurrentSpeed > 0) CurrentSpeed -= 1.5f;
            if (CurrentSpeed < 0) CurrentSpeed = 0;
        }
        private void OnInputChanged(float Horizontal, float Vertical)
        {
            if (Vertical != 0)
                Last = Vertical;
            

            if (Vertical > 0 || Vertical < 0)
            {
                //Forward
                if (Vertical > 0)
                {
                    if (CurrentSpeed < MaxSpeedT)
                        CurrentSpeed += 1;
                    this.transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed * Vertical);
                }
                //Back
                else if (Vertical < 0 && CurrentSpeed < MaxSpeedR)
                {
                    if (CurrentSpeed < MaxSpeedR)
                        CurrentSpeed += 0.5f;
                    this.transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed * Vertical);
                }
                //ControllingSpeedWhenUWantToGoBack
                else if (CurrentSpeed > MaxSpeedR && Vertical < 0)
                    CurrentSpeed = 0;
                //Limit
                else if (CurrentSpeed == MaxSpeedR || CurrentSpeed == MaxSpeedT)
                    this.transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed * Vertical);
                //Rotation
                if (Horizontal != 0 && Vertical > 0)
                    this.transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime * Horizontal);
                else if (Vertical < 0 && Horizontal != 0)
                    this.transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime * -Horizontal);
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
                Vector3 from_v = new Vector3(Wheels[0].transform.rotation.x, Wheels[0].transform.localRotation.y, Wheels[0].transform.localRotation.z);
                Vector3 to_v = new Vector3(Wheels[1].transform.rotation.x, CurrentRotation, Wheels[1].transform.localRotation.z);
                Quaternion from = Quaternion.Euler(from_v);
                Quaternion to = Quaternion.Euler(to_v);
                Wheels[0].transform.localRotation = Quaternion.Lerp(from, to, 1f);
                Wheels[1].transform.localRotation = Quaternion.Lerp(from, to, 1f);
            }
            //Speed-wheels
            int NegOrPos = 1;
            if (Last < 0) NegOrPos = -1;
            foreach (GameObject i in Wheels)
                i.transform.Rotate(NegOrPos * CurrentSpeed, i.transform.localRotation.y, i.transform.localRotation.z);
        }
    }
}
