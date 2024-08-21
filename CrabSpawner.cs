using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CrabSpawner : MonoBehaviour
{
    public GameObject crabPrefab;  
    public AnimationClip crabAnimation;
    public float spawnRadius = 2.0f;  
    public int crabCount = 5;  
    public List<Vector3> waypoints = new List<Vector3>(); 
    public bool showGizmos = true;  

    

    void Start()
    {
        // int playerLayer = LayerMask.NameToLayer("Player");
        // int crabsLayer = LayerMask.NameToLayer("Crabs");
        // Physics.IgnoreLayerCollision(playerLayer, crabsLayer);

        // StartCoroutine(SpawnCrabsAfterStart());
        SpawnCrabs();
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Vector3 startPoint = waypoints[i];
            Vector3 endPoint = waypoints[i + 1];
            DrawWireCapsule(startPoint, endPoint, spawnRadius);
        }
    }

    private void DrawWireCapsule(Vector3 startPoint, Vector3 endPoint, float radius)
    {
        Gizmos.DrawWireSphere(startPoint, radius);
        Gizmos.DrawWireSphere(endPoint, radius);

        Vector3 direction = endPoint - startPoint;
        float distance = direction.magnitude;
        direction.Normalize();

        Vector3 offsetUp = Vector3.Cross(direction, Vector3.up).normalized * radius;
        Vector3 offsetRight = Vector3.Cross(direction, Vector3.right).normalized * radius;

        Gizmos.DrawLine(startPoint + offsetUp, endPoint + offsetUp);
        Gizmos.DrawLine(startPoint - offsetUp, endPoint - offsetUp);
        Gizmos.DrawLine(startPoint + offsetRight, endPoint + offsetRight);
        Gizmos.DrawLine(startPoint - offsetRight, endPoint - offsetRight);
    }

    public void SpawnCrabs()
    {
        if (crabPrefab == null || waypoints.Count < 2) return;
        
        int spawnedCrabs = 0;

        while (spawnedCrabs < crabCount)
        {
            int segmentIndex = Random.Range(0, waypoints.Count - 1);
            Vector3 startPoint = waypoints[segmentIndex];
            Vector3 endPoint = waypoints[segmentIndex + 1];
            Vector3 direction = (endPoint - startPoint).normalized;

            float randomDistance = Random.Range(0f, (endPoint - startPoint).magnitude);
            Vector3 spawnPoint = startPoint + direction * randomDistance;
            spawnPoint += Random.insideUnitSphere * spawnRadius;
            spawnPoint.y += 5.0f;

            GameObject crab = Instantiate(crabPrefab, spawnPoint, Quaternion.identity, transform);

            BoxCollider collider = crab.AddComponent<BoxCollider>();
            // collider.isTrigger = false;
            collider.providesContacts = false;
            collider.size = new Vector3(0.15f, 0.15f, 0.15f);
            collider.center = new Vector3(0f, 0.1f, 0f);  

            Animation animation = crab.AddComponent<Animation>();
            animation.AddClip(crabAnimation, "crabIdle");

            Rigidbody rb = crab.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = 1.0f;  
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            crab.AddComponent<CrabMovement>();
            CrabMovement crabMovement = crab.GetComponent<CrabMovement>();

            // crab.layer = LayerMask.NameToLayer("Crabs");

            if (crabMovement != null) crabMovement.SetWaypoints(waypoints);

            spawnedCrabs++;
        }

    }

    IEnumerator SpawnCrabsAfterStart()
    {
        SpawnCrabs();
        yield break;  
    }

}
