using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTrackGenerator : MonoBehaviour {

    [SerializeField] private PlayerController player;
    [SerializeField] private TrackSegment[] segmentsPrefabs;
    [SerializeField] private TrackSegment firstTrackPrefab;

    [Header("Endless Generation Parameters")]
    [SerializeField] private int initialTrackCount = 10;
    [SerializeField] private int minTracksInFrontOfPlayer = 3;
    [SerializeField] private List<TrackSegment> currentSegments = new();
    [SerializeField] private float minDistanceToConsiderInsideTrack = 3;


    private void Start() {
        SpawnTrackSegment(firstTrackPrefab, null);
        SpawnTracks(initialTrackCount);
    }

    

    private void Update() {
        ControlTracks();
    }

    private void SpawnTracks(int trackCount) {
        TrackSegment previousTrack = currentSegments.Count > 0 ? currentSegments[currentSegments.Count - 1] : null;
        for (int i = 0; i < trackCount - 1; i++) {
            int index = Random.Range(0, segmentsPrefabs.Length);
            TrackSegment track = segmentsPrefabs[index];
            previousTrack = SpawnTrackSegment(track, previousTrack);
        }
    }

    private TrackSegment SpawnTrackSegment(TrackSegment track, TrackSegment previousTrack) {
        TrackSegment trackInstance = Instantiate(track, transform);

        if (previousTrack != null) {
            trackInstance.transform.position = previousTrack.End.position + (trackInstance.transform.position - trackInstance.Start.position);
        }
        else {
            trackInstance.transform.localPosition = Vector3.zero;
        }

        foreach (var obstacleSpawner in trackInstance.ObstacleSpawners) {
            obstacleSpawner.SpawnObstacle();
        }

        currentSegments.Add(trackInstance);

        return trackInstance;
    }

    private void ControlTracks() {
        //Em qual track o player esta?
        int playerTrackIndex = FindTrackIndexWithPlayer();

        if (playerTrackIndex < 0) { return; }

        //Instancia tracks a frente do player
        InstantiateTracksFrontPlayer(playerTrackIndex);

        //Remover tracks atras do player
        //1 - Destruir os objetos
        //2 - Remover eles da lista do currentTracks
        RemoveTracksAfterPlayer(playerTrackIndex);
    }

    private int FindTrackIndexWithPlayer() {
        int playerTrackIndex = -1;
        for (int i = 0; i < currentSegments.Count; i++) {
            TrackSegment track = currentSegments[i];
            if (player.transform.position.z >= track.Start.position.z + minDistanceToConsiderInsideTrack && player.transform.position.z <= track.End.position.z) {
                playerTrackIndex = i;
                break;
            }
        }
        return playerTrackIndex;
    }

    private void InstantiateTracksFrontPlayer(int playerTrackIndex) {
        int tracksInFrontOfPlayer = currentSegments.Count - (playerTrackIndex + 1);
        if (tracksInFrontOfPlayer < minTracksInFrontOfPlayer) {
            SpawnTracks(minTracksInFrontOfPlayer - tracksInFrontOfPlayer);
        }
    }
    private void RemoveTracksAfterPlayer(int playerTrackIndex) {
        for (int i = 0; i < playerTrackIndex; i++) {
            TrackSegment track = currentSegments[i];
            Destroy(track.gameObject);
        }

        currentSegments.RemoveRange(0, playerTrackIndex);
    }
}
