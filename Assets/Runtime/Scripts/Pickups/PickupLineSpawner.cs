using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLineSpawner : MonoBehaviour {

    [SerializeField] private Pickup pickupPrefab;
    [SerializeField] private Transform start, end;
    [SerializeField] private float spaceBetweenPickups = 3f;

    public void SpawnPickupLine() {
        Vector3 currentSpawnPosition = start.position;
        while (currentSpawnPosition.z < end.position.z) {
            Pickup pickup = Instantiate(pickupPrefab, currentSpawnPosition, Quaternion.identity, transform);
            currentSpawnPosition.z += spaceBetweenPickups;
        }
    }

    private void OnDrawGizmos() {
        Vector3 currentSpawnPosition = start.position;
        while (currentSpawnPosition.z < end.position.z) {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(currentSpawnPosition, Vector3.one);
            currentSpawnPosition.z += spaceBetweenPickups;
        }
    }
}