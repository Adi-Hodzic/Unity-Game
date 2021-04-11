using System;
using UnityEngine;
namespace Car.InputManagement
{
    public class InputManagement : MonoBehaviour
    {
        //SerializedField
        [SerializeField] private KeyCode keyN1;
        [SerializeField] private KeyCode keyN2;
        //Actions
        public Action<float, float> OnInputChanged;
        public Action<float, float> OnRotate;
        //public Action OnInputChangedWheels;
        public Action OnBrake;
        private void FixedUpdate()
        {

            //OnInputChangedWheels?.Invoke();
            OnInputChanged?.Invoke(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (Input.GetKey(keyN1))
                OnRotate?.Invoke(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (Input.GetKey(keyN2))
                OnBrake?.Invoke();
        }

    }
}
