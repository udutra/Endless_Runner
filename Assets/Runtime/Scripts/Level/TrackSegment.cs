using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSegment : MonoBehaviour {
    [SerializeField] private Transform start, end;

    private ObstacleSpawner[] obstacleSpawners;

    public Transform Start => start;
    public Transform End => end;

    public ObstacleSpawner[] ObstacleSpawners => obstacleSpawners == null ? obstacleSpawners = GetComponentsInChildren<ObstacleSpawner>() : obstacleSpawners;
}