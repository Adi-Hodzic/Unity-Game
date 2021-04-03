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
        //[SerializeField] public WheelCollider[] wheelCollider = new WheelCollider[4];
        //[SerializeField] public Transform[] wheelMesh = new Transform[4];
        [SerializeField ]public Transform t_CenterOfMass;

        private Rigidbody r_Ridgedbody;
        //public Transform FrontTarget;
        //public Transform BackTarget;

        void Start()
        {
            Manager.OnInputChanged += OnInputChanged;

            r_Ridgedbody = GetComponent<Rigidbody>();
            r_Ridgedbody.centerOfMass = t_CenterOfMass.localPosition;
        }

        private void OnInputChanged(float Horizontal, float Vertical)
        {
            //Forward and back
            if (Vertical > 0)
                this.transform.Translate(Vector3.forward * Time.deltaTime * Speed * Vertical);
            else if (Vertical < 0)
                this.transform.Translate(Vector3.forward * Time.deltaTime * 10 * Vertical);

            //Right and left
            if (Horizontal != 0 && Vertical > 0)
                this.transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime * Horizontal);
            else if (Vertical < 0 && Horizontal != 0)
                this.transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime * -Horizontal);
        }


        //public void Update()
        //{
        //    UpdateMeshPosition();
        //}

        //public void FixedUpdate()
        //{
        //    float steer = Input.GetAxis("Horizontal") * maxSteerAngle;
        //    float torque = Input.GetAxis("Vertical") * maxTorque;

        //    wheelCollider[0].steerAngle = steer;
        //    wheelCollider[1].steerAngle = steer;

        //    for (int i = 0; i < 4; i++)
        //    {
        //        wheelCollider[i].motorTorque = torque;
        //    }
        //}

        //public void UpdateMeshPosition()
        //{
        //    for (int i = 0; i < 4; i++)
        //    {
        //        Quaternion quat;
        //        Vector3 pos;

        //        //Gets the current position of the physics WheelColliders.
        //        wheelCollider[i].GetWorldPose(out pos, out quat);

        //        ///Sets the mesh to match the position and rotation of the physics WheelColliders.
        //        wheelMesh[i].position = pos;
        //        wheelMesh[i].rotation = quat;
        //    }
        //}
    }
}
