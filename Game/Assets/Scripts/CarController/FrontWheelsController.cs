using UnityEngine;
namespace Car.CarController
{
    public class FrontWheelsController : MonoBehaviour
    {
        //[SerializeField] [Range(0.1f, 100f)] private float Speed;
        [SerializeField] private Car.InputManagement.InputManagement Manager;
        [SerializeField] private float MaximumRotation;
        [SerializeField] private float RotationSpeed;
        
        private float CurrentRotation { get; set; } = 0;
        private void Start()
        {
            //Manager.OnInputChangedWheels += OnInputChangedWheels;
        }


       private void OnInputChangedWheels()
        {
            if (Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.S))
            {
                this.transform.Rotate(new Vector3(10, 0, 0));
                this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.x, 0, this.transform.localRotation.z);
            }
            //this.transform.Rotate()
            CurrentRotation = Input.GetAxisRaw("Horizontal") * RotationSpeed * Time.deltaTime;

            if (CurrentRotation <= MaximumRotation || CurrentRotation >= -MaximumRotation)
                this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.x, CurrentRotation, this.transform.localRotation.z);
        }
    }
}

