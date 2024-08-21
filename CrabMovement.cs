using UnityEngine;
using System.Collections.Generic;

public class CrabMovement : MonoBehaviour
{
    public float speed = 0.5f;
    private List<Vector3> waypoints;
    private int currentWaypointIndex = 0;
    private Animation animationCrab;

    void Start() 
    {
        animationCrab = this.GetComponent<Animation>();
    }

    public void SetWaypoints(List<Vector3> waypoints)
    {
        this.waypoints = waypoints;
        currentWaypointIndex = 0;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        Vector3 target = waypoints[currentWaypointIndex];
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target) < 0.1f) currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    
        animationCrab.Play("crabIdle");
    }

    void AnimateCrab()
    {
        animationCrab.Play();
    }
}
