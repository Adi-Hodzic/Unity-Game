using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Osiris.Controllers.CarController
{
    public class BotController : MonoBehaviour
    {
        [SerializeField] [ReorderableList] private List<GameObject> Wheels;
        [SerializeField] private int HowMuchPositions;
        [SerializeField] private int MaxSpeed;
        [SerializeField] private int CurveSpeed;
        [SerializeField] private LayerMask GroundLayer;
        [SerializeField] private GameObject MassFront;
        [SerializeField] private GameObject MassRear;
        private NavMeshAgent NavAgent;
        private NavMeshPath Path { get; set; }
        private Rigidbody BotRigidbody;
        private MeshCollider Mesh;
        public GameObject Target;
        private Vector3 Current { get; set; }
        private Vector3 LastRotation { get; set; }
        private Vector3 NextRotation { get; set; }
        private bool Ine = false;
        private bool IsCarGroundedRearWheels;
        private bool IsCarGroundedFrontWheels;

        private int NumberOfBots = 4;

        void Start()
        {
            NavAgent = GetComponent<NavMeshAgent>();
            BotRigidbody = GetComponent<Rigidbody>();
            Mesh = GetComponent<MeshCollider>();
        }

        private void Update()
        {
            if (NavAgent.isOnNavMesh)
                NavAgent.enabled = true;
            else
                NavAgent.enabled = false;

            if (NavAgent.enabled == true)
            {
                Path = NavAgent.path;
                NavAgent.SetDestination(Target.transform.position);
                StartCoroutine(StartCoroutine());
            }
            else if (NavAgent.enabled == false && IsCarGrounded())
            {
                Mesh.enabled = false;
                NavAgent.enabled = true;
            }
            else if (!IsCarGrounded())
                Balance();

        }

        private bool IsRearWheelsGrounded()
        {
            IsCarGroundedRearWheels =
                      Physics.Raycast(Wheels[2].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer) ||
                      Physics.Raycast(Wheels[3].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer);
            return IsCarGroundedRearWheels;
        }
        private bool IsFrontWheelsGrounded()
        {
            IsCarGroundedFrontWheels =
                       Physics.Raycast(Wheels[4].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer) ||
                       Physics.Raycast(Wheels[5].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer);
            return IsCarGroundedFrontWheels;
        }
        private bool IsCarGrounded()
        {
            if (IsFrontWheelsGrounded() && IsRearWheelsGrounded())
                return true;
            return false;
        }
        private void Balance()
        {
            if (!IsCarGrounded())
                Mesh.enabled = true;
            else
                Mesh.enabled = false;

            if (!IsFrontWheelsGrounded() && IsRearWheelsGrounded())
            {
                NavAgent.enabled = false;
                BotRigidbody.MovePosition(MassFront.transform.position);
            }
            else if (IsFrontWheelsGrounded() && !IsRearWheelsGrounded())
            {
                NavAgent.enabled = false;
                BotRigidbody.MovePosition(MassRear.transform.position);
            }
        }
        private void ChangeRotation(float y)
        {
            Wheels[4].transform.localRotation = Quaternion.Slerp(
                                                           /*1*/Wheels[4].transform.localRotation,
                                                           /*2*/new Quaternion(Wheels[4].transform.localRotation.x,
                                                           /*2*/Mathf.Clamp(y, -10, 10) * Time.deltaTime,
                                                           /*2*/Wheels[4].transform.localRotation.z,
                                                           /*2*/Wheels[4].transform.localRotation.w),
                                                           /*3*/2 * Time.deltaTime * 0.5f);
            Wheels[5].transform.localRotation = Quaternion.Slerp(
                                                           /*1*/Wheels[5].transform.localRotation,
                                                           /*2*/new Quaternion(Wheels[5].transform.localRotation.x,
                                                           /*2*/Mathf.Clamp(y, -10, 10) * Time.deltaTime,
                                                           /*2*/Wheels[5].transform.localRotation.z,
                                                           /*2*/Wheels[5].transform.localRotation.w),
                                                           /*3*/2 * Time.deltaTime * 0.5f);
        }
        private void OnCollisionEnter(Collision collision)
        {
            for (int i = 0; i < NumberOfBots; i++)
            {
                if (collision.gameObject.name == $"Bot" + i + "(Clone)")
                {
                    Mesh.enabled = true;
                    NavAgent.enabled = false;
                }
                Debug.Log("Udar");
            }
        }
        IEnumerator StartCoroutine()
        {
            //Wheels
            if (LastRotation == null)
                LastRotation = NavAgent.transform.localEulerAngles;

            NextRotation = NavAgent.transform.localEulerAngles;
            if (NextRotation != LastRotation)
            {
                if (NextRotation.y < LastRotation.y)
                    ChangeRotation(7);
                else if (NextRotation.y > LastRotation.y)
                    ChangeRotation(-7);
                else if (NextRotation.y == LastRotation.y)
                    ChangeRotation(0);
            }
            LastRotation = NavAgent.transform.localEulerAngles;

            for (int i = 0; i < Wheels.Count - 2; i++)
            {
                GameObject v = Wheels[i];
                v.transform.Rotate(NavAgent.acceleration, v.transform.rotation.y, v.transform.rotation.z);
            }

            //Curves
            if (Path.corners.Length / 2 > 1)
                Current = Path.corners[Path.corners.Length / 2];
            //else
            //    Current = Path.corners[0];

            float p = Current.x - NavAgent.transform.position.x;
            float a = Current.z - NavAgent.transform.position.z;
            if (p < HowMuchPositions && p > -HowMuchPositions && a < HowMuchPositions && a > -HowMuchPositions && Current != Path.corners[0])
            {
                NavAgent.speed = CurveSpeed;
                Ine = true;
            }
            else
            {
                if (Ine)
                {
                    yield return new WaitForSeconds(1);
                    if (NavAgent.speed < MaxSpeed)
                        NavAgent.speed += 2;
                    else
                        NavAgent.speed = MaxSpeed - 1;
                }
                else
                {
                    if (NavAgent.speed < MaxSpeed)
                        NavAgent.speed += 4;
                    else
                        NavAgent.speed = MaxSpeed - 1;
                }
            }
        }
        private bool isAnyWheelGrounded()
        {
            return Physics.Raycast(Wheels[2].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer) ||
                    Physics.Raycast(Wheels[3].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer) ||
                    Physics.Raycast(Wheels[4].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer) ||
                    Physics.Raycast(Wheels[5].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer);
        }
    }
}
