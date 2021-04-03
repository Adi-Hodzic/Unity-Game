using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Car.CameraController
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private GameObject obj;
        [SerializeField] private Car.InputManagement.InputManagement Manager;

        private Vector3 FirstPosition;
        private Vector3 FirstRotation;
        void Start()
        {
            FirstPosition = obj.transform.position;
            FirstRotation = obj.transform.rotation.eulerAngles;
            Manager.OnRotate += OnRotate;
        }


        private void OnRotate(float MouseX, float MouseY)
        {
            Vector3 vector3 = new Vector3(MouseX, -MouseY, 0);

            this.transform.LookAt(obj.transform.position);
            transform.RotateAround(obj.transform.position, Vector3.up, MouseX * 5);
        }
       
    }
}
