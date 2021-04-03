using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Player.CarController
{
    public class WayPointSystem : MonoBehaviour
    {
        [ReorderableList] [SerializeField] private List<GameObject> Waypoints;

        private GameObject NextWaypoint { get; set; }

        private bool IsWaypointFetched { get; set; }

        private void Start()
        {
            if(Waypoints.Count == 0)
            {
                Debug.LogError("Please add waypoints in the editor");
                UnityEditor.EditorApplication.isPlaying = false;
            }

            NextWaypoint = Waypoints[0];
        }

        private int Indexer = 0;
        private void Update()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, NextWaypoint.transform.position, 2 * Time.deltaTime);
            this.transform.LookAt(NextWaypoint.transform);

            if (Vector3.Distance(this.transform.position, NextWaypoint.transform.position) <= 1)
            {
                NextWaypoint = Waypoints[Indexer++ % Waypoints.Count];
            }
        }
    }
}
