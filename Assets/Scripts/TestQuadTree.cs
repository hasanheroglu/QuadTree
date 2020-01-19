using System.Collections;
using Manager;
using Tree;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ColliderType{Rectangle, Circle}
public class TestQuadTree : MonoBehaviour
{
    private QuadTree _quadTree;
    
    private bool _canAdd;
    private bool _started;
    
    private int _entityCount;
    private int _maxEntityCount;
    private float _spawnTimer;
    
    private ColliderType _colliderType;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private GameObject _spherePrefab;
    
    public static TestQuadTree Instance { get; set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _canAdd = true;
        _quadTree = new QuadTree(new Rectangle(0f, 0f, 50f, 50f), 4);
        UIManager.Instance.SetMaxEntityCount(_maxEntityCount);
    }

    private void Update()
    {
        if (_canAdd && _started)
        {
            InsertEntity();
        }
    }

    public void StartSimulation()
    {
        _started = true;
        UIManager.Instance.SetSimulationStatus("Running");
        UIManager.Instance.SetEntityCount(_entityCount);
    }

    public void StopSimulation()
    {
        _started = false;
        UIManager.Instance.SetSimulationStatus("Stopped");
        UIManager.Instance.SetEntityCount(_entityCount);
    }

    public void Clear()
    {
        _started = false;
        _quadTree.Clear();
        LineDrawer.Instance.Clear();
        _entityCount = 0;
        UIManager.Instance.SetSimulationStatus("Cleared");
        UIManager.Instance.SetEntityCount(_entityCount);
    }

    public void AdjustMaxEntityCount(float count)
    {
        _started = false;
        _maxEntityCount = (int) count;
        UIManager.Instance.SetSimulationStatus("Stopped");
        UIManager.Instance.SetMaxEntityCount(_maxEntityCount);
    }

    public void AdjustSpawnTimer(float timer)
    {
        _started = false;
        _spawnTimer = timer;
        UIManager.Instance.SetSimulationStatus("Stopped");
        UIManager.Instance.SetSpawnTimer(_spawnTimer);
    }

    public void SetColliderType(int option)
    {
        Clear();
        
        switch (option)
        {
            case 0:
                _colliderType = ColliderType.Rectangle;
                break;
            case 1:
                _colliderType = ColliderType.Circle;
                break;
        }
    }

    private void InsertEntity()
    {
        var pos = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f));
        var width = Random.Range(1f, 5f);
        var height = Random.Range(1f, 5f);
        
        if (_entityCount < _maxEntityCount)
        {
            GameObject gameObject;
            if (_colliderType == ColliderType.Rectangle)
            {
                gameObject = Instantiate(_cubePrefab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                gameObject = Instantiate(_spherePrefab, Vector3.zero, Quaternion.identity);
            }
            
            gameObject.SetActive(false);
            var entity = gameObject.GetComponent<Entity.Entity>();
            
            if(_quadTree.Insert(pos, entity))
            {
                _entityCount++;
                entity.SetPosition(pos);
                gameObject.SetActive(true);
                entity.SetSize(width, height);
            }  
        }
        
        UIManager.Instance.SetEntityCount(_entityCount);
        StartCoroutine(Wait(_spawnTimer));
        _quadTree.FindCollisions(_quadTree);
    }

    public void RemoveEntity()
    {
        _entityCount--;
        UIManager.Instance.SetEntityCount(_entityCount);
    }

    private IEnumerator Wait(float seconds)
    {
        _canAdd = false;
        yield return new WaitForSeconds(seconds);
        _canAdd = true;
    }
}
