using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTrackGenerator : MonoBehaviour {

    [Header("Player")]
    [SerializeField] private PlayerController player;

    [Header("Endless Generation Prefabs")]
    [SerializeField] private TrackSegment firstTrackPrefab;
    [SerializeField] private TrackSegment[] easyTrackPrefabs;
    [SerializeField] private TrackSegment[] hardTrackPrefabs;
    [SerializeField] private TrackSegment[] rewardTrackPrefabs;

    [Header("Endless Generation Parameters")]
    [SerializeField] private int initialTrackCount = 5;
    [SerializeField] private int minTracksInFrontOfPlayer = 3;
    [SerializeField] private float minDistanceToConsiderInsideTrack = 3f;

    [Header("Level Difficulty Parameters")]
    [Range(0, 1)] [SerializeField] private float hardTrackChance = 0.2f;
    [SerializeField] private int minTracksBeforeReward = 10;
    [SerializeField] private int maxTracksBeforeReward = 20;
    [SerializeField] private int minRewardTrackCount = 1;
    [SerializeField] private int maxRewardTrackCount = 2;

    private List<TrackSegment> currentSegments = new();
    [SerializeField] private bool isSpawningRewardTracks = false;
    [SerializeField] private int rewardTracksLeftToSpawn = 0;
    [SerializeField] private int trackSpawnedAfterLastReward = 0;

    private void Start() {
        SpawnTrackSegment(firstTrackPrefab, null);
        SpawnTracks(initialTrackCount);
    }

    private void Update() {
        UpdateTracks();
    }

    private void SpawnTracks(int trackCount) {
        TrackSegment previousTrack = currentSegments.Count > 0 ? currentSegments[currentSegments.Count - 1] : null;
        for (int i = 0; i < trackCount - 1; i++) {
            var track = GetRandomTrack();
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

        UpdateRewardTracking();

        return trackInstance;
    }

    private void UpdateTracks() {
        //Em qual track o player esta?
        int playerTrackIndex = FindTrackIndexWithPlayer();

        if (playerTrackIndex < 0) { return; }

        //Instancia tracks a frente do player
        SpawnNewTracksIfNeeded(playerTrackIndex);

        //Remover tracks atras do player
        //1 - Destruir os objetos
        //2 - Remover eles da lista do currentTracks
        DespawnTracksBehindPlayer(playerTrackIndex);
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

    private void SpawnNewTracksIfNeeded(int playerTrackIndex) {
        int tracksInFrontOfPlayer = currentSegments.Count - (playerTrackIndex + 1);
        if (tracksInFrontOfPlayer < minTracksInFrontOfPlayer) {
            SpawnTracks(minTracksInFrontOfPlayer - tracksInFrontOfPlayer);
        }
    }

    private void DespawnTracksBehindPlayer(int playerTrackIndex) {
        for (int i = 0; i < playerTrackIndex; i++) {
            TrackSegment track = currentSegments[i];
            Destroy(track.gameObject);
        }

        currentSegments.RemoveRange(0, playerTrackIndex);
    }

    private TrackSegment GetRandomTrack() {
        TrackSegment[] trackList = null;

        if (isSpawningRewardTracks) {
            trackList = rewardTrackPrefabs;
        }
        else {
            trackList = Random.value <= hardTrackChance ? hardTrackPrefabs : easyTrackPrefabs;
        }

        return trackList[Random.Range(0, trackList.Length)];
    }

    private void UpdateRewardTracking() {
        if (isSpawningRewardTracks) {
            rewardTracksLeftToSpawn--;
            if (rewardTracksLeftToSpawn <= 0) {
                isSpawningRewardTracks = false;
                trackSpawnedAfterLastReward = 0;
            }
        }
        else {
            trackSpawnedAfterLastReward++;
            int requiredTracksBeforeReward = Random.Range(minTracksBeforeReward, maxTracksBeforeReward + 1);
            if (trackSpawnedAfterLastReward >= requiredTracksBeforeReward) {
                isSpawningRewardTracks = true;
                rewardTracksLeftToSpawn = Random.Range(minRewardTrackCount, maxRewardTrackCount + 1);
            }
        }
    }
}
