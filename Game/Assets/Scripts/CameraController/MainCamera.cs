using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Car.CameraController
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private GameObject obj;
        [SerializeField] private Car.InputManagement.InputManagement Manager;

        void Start()
        {
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
