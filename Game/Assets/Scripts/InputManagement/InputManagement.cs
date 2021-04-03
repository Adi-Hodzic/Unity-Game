using System;
using UnityEngine;
namespace Car.InputManagement
{
    public class InputManagement : MonoBehaviour
    {
        //SerializedField
        [SerializeField] private KeyCode keyN1;

        //Actions
        public Action<float, float> OnInputChanged;
        public Action<float, float> OnRotate;
        public Action OnInputChangedWheels;
        private void FixedUpdate()
        {
            OnInputChangedWheels?.Invoke();
            OnInputChanged?.Invoke(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (Input.GetKey(keyN1))
                OnRotate?.Invoke(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
          
        }

    }
}
