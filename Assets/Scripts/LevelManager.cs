using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Elements")]
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private Transform _wallContainer;
    [SerializeField] private GameObject _obstaclePrefab;

    private Transform _currentWallUp;
    private Transform _currentWallDown;
    private Transform _nextWallUp;
    private Transform _nextWallDown;

    private float _nextWallBorder;
    private float _wallHorSize;

    private float _vertScreenExtent;
    private float _horScreenExtent;

    private void Start()
    {
        _wallHorSize = _wallPrefab.GetComponent<Renderer>().bounds.size.x;

        var vertScreenExtent = Camera.main.orthographicSize;
        var horScreenExtent = vertScreenExtent * Screen.width / Screen.height;
        _vertScreenExtent = vertScreenExtent;
        _horScreenExtent = horScreenExtent;
    }

    public void ClearLevel()
    {
        foreach (Transform wall in _wallContainer)
            if (wall.gameObject != null) Destroy(wall.gameObject);
    }

    public void CreateLevel()
    {
        var wallUp = Instantiate(_wallPrefab, _wallContainer);
        wallUp.transform.position = new Vector2(0, _vertScreenExtent);
        _currentWallUp = wallUp.transform;

        var wallDown = Instantiate(_wallPrefab, _wallContainer);
        wallDown.transform.position = new Vector2(0, -_vertScreenExtent);
        _currentWallDown = wallDown.transform;

        var nextWallUp = Instantiate(_wallPrefab, _wallContainer);
        nextWallUp.transform.position = new Vector2(_wallHorSize, _vertScreenExtent);
        _nextWallUp = nextWallUp.transform;

        var nextWallDown = Instantiate(_wallPrefab, _wallContainer);
        nextWallDown.transform.position = new Vector2(_wallHorSize, -_vertScreenExtent);
        _nextWallDown = nextWallDown.transform;
    }

    public void MoveWall(float speed)
    {
        var translation = new Vector3(-speed, 0, 0);
        _wallContainer.Translate(translation);

        if (_nextWallUp.position.x <= 0)
        {
            RecreateWalls();
        }
    }

    private void RecreateWalls()
    {
        _currentWallUp.position = new Vector2(_wallHorSize, _vertScreenExtent);
        _currentWallDown.position = new Vector2(_wallHorSize, -_vertScreenExtent);

        var wallUpBuffer = _currentWallUp;
        var wallDownBuffer = _currentWallDown;

        _currentWallUp = _nextWallUp;
        _currentWallDown = _nextWallDown;

        _nextWallUp = wallUpBuffer;
        _nextWallDown = wallDownBuffer;
    }

    public void AddObstacle()
    {
        var obstacle = Instantiate(_obstaclePrefab, _wallContainer);
        obstacle.transform.position = new Vector2(_horScreenExtent * 1.2f, Random.Range(_vertScreenExtent * 0.45f, -_vertScreenExtent * 0.45f));
    }
}
