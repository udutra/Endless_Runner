using UnityEngine;

public class TrackSegment : MonoBehaviour {
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    [SerializeField] private ObstacleSpawner[] obstacleSpawners;
    [SerializeField] DecorationSpawner decorationSpawner;

    [Header("Pickups")]
    [SerializeField, Range(0f, 1f)] private float pickupSpawnChance = 0.5f;
    [SerializeField] private PickupLineSpawner[] pickupLineSpawners;

    public Transform Start => start;
    public Transform End => end;

    public float Length => Vector3.Distance(End.position, Start.position);
    public float SqrLength => (End.position - Start.position).sqrMagnitude;

    public ObstacleSpawner[] ObstacleSpawners => obstacleSpawners;
    public DecorationSpawner DecorationSpawner => decorationSpawner;

    public void SpawnObstacles() {
        foreach (var obstacleSpawner in obstacleSpawners) {
            obstacleSpawner.SpawnObstacle();
        }
    }
    public void SpawnDecorations() {
        DecorationSpawner.SpawnDecorations();
    }

    public void SpawnPickups() {
        if (pickupLineSpawners.Length > 0 && Random.value <= pickupSpawnChance) {
            int randomIndex = Random.Range(0, pickupLineSpawners.Length);
            PickupLineSpawner pickupSpawner = pickupLineSpawners[randomIndex];
            pickupSpawner.SpawnPickupLine();
        }
    }
}
