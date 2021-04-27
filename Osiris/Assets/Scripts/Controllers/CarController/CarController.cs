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
        //[SerializeField] [Range(0.001f, 100)] 
        private float TurnSpeed;
        [SerializeField] private LayerMask GroundLayer;
        [SerializeField] private InputManagement Manager;
        [SerializeField] private float MaximumRotation;
        [SerializeField] private float RotationSpeed;
        [SerializeField] private GameObject Mass;
        private Rigidbody CarRigidbody;
        private float CurrentRotation { get; set; }
        private float CurrentSpeed { get; set; }
        private float Last { get; set; }
        private bool IsCarGroundedRearWheels { get; set; }
        private bool IsCarGroundedFrontWheels { get; set; }
        private float DistToGround { get; set; }
        void Start()
        {
            Manager.OnInputChanged += OnInputChanged;
            Manager.OnBrake += OnBrake;
            CarRigidbody = GetComponent<Rigidbody>();
            DistToGround = transform.position.y + 1f;
        }

        private void OnBrake()
        {
            if (CurrentSpeed > 0) CurrentSpeed -= 1.5f;
            if (CurrentSpeed < 0) CurrentSpeed = 0;
        }
        private void OnInputChanged(float Horizontal, float Vertical)
        {
            if (MaxSpeedT - CurrentSpeed < 25)
                TurnSpeed = 25;
            else
                TurnSpeed = MaxSpeedT - CurrentSpeed;
            float NewChange = 0;
            //Last
            if (Vertical != 0)
                Last = Vertical;
            if (Vertical > 0)
                NewChange = Vertical;
            //IsCarGrounded
            IsCarGroundedRearWheels =
                        Physics.Raycast(Wheels[2].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer) ||
                        Physics.Raycast(Wheels[3].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer);
            IsCarGroundedFrontWheels =
                        Physics.Raycast(Wheels[4].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer) ||
                        Physics.Raycast(Wheels[5].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer);

            if (!IsCarGroundedRearWheels && IsCarGroundedFrontWheels)
                CarRigidbody.MovePosition(Mass.transform.position);
            else
                CarRigidbody.MovePosition(this.transform.position);


            if ((Vertical > 0 || Vertical < 0) && IsCarGroundedRearWheels && IsCarGroundedFrontWheels)
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
                    if (NewChange > 0)
                        while (CurrentSpeed <= 0)
                            CurrentSpeed -= 0.001f;

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
                //CarRotation
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
                        if (IsCarGroundedRearWheels)
                            this.transform.Rotate(Vector3.up, CurrentSpeed * Time.deltaTime * Horizontal);
                    }
                    if (Last < 0)
                    {
                        CurrentSpeed -= 0.1f;
                        this.transform.Translate(Vector3.forward * Time.deltaTime * -CurrentSpeed);
                        if (IsCarGroundedRearWheels)
                            this.transform.Rotate(Vector3.up, CurrentSpeed * Time.deltaTime * -Horizontal);
                    }
                }
            }
            float NewRotation = RotationSpeed;
            if (TurnSpeed == 25)
                NewRotation = 4 + RotationSpeed / 2;
            //Rotation - WHEELS
            CurrentRotation = Horizontal * NewRotation * Time.deltaTime;
            Wheels[4].transform.localRotation = Quaternion.Slerp(
                                                           /*1*/Wheels[4].transform.localRotation,
                                                           /*2*/new Quaternion(Wheels[4].transform.localRotation.x,
                                                           /*2*/Mathf.Clamp(CurrentRotation, -MaximumRotation, MaximumRotation),
                                                           /*2*/Wheels[4].transform.localRotation.z,
                                                           /*2*/Wheels[4].transform.localRotation.w),
                                                           /*3*/NewRotation * Time.deltaTime * 0.5f);
            Wheels[5].transform.localRotation = Quaternion.Slerp(
                                                           /*1*/Wheels[5].transform.localRotation,
                                                           /*2*/new Quaternion(Wheels[5].transform.localRotation.x,
                                                           /*2*/Mathf.Clamp(CurrentRotation, -MaximumRotation, MaximumRotation),
                                                           /*2*/Wheels[5].transform.localRotation.z,
                                                           /*2*/Wheels[4].transform.localRotation.w),
                                                           /*3*/NewRotation * Time.deltaTime * 0.5f);

            //Speed-wheels
            int NegOrPos = 1;
            if (Last < 0) NegOrPos = -1;
            if (!IsCarGroundedRearWheels && (Vertical > 0 || Vertical < 0))
            {
                Wheels[2].transform.Rotate(new Vector3(NegOrPos * 5, 0, 0));
                Wheels[3].transform.Rotate(new Vector3(NegOrPos * 5, 0, 0));
            }
            for (int i = 0; i < Wheels.Count - 2; i++)
            {
                GameObject g = Wheels[i];
                g.transform.Rotate(NegOrPos * CurrentSpeed, g.transform.localRotation.y, 0);
            }
        }

    }
}

