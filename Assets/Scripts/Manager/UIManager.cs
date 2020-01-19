using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _simulationStatusText;
        [SerializeField] private GameObject _entityCountText;
        [SerializeField] private GameObject _maxEntityCountText;
        [SerializeField] private GameObject _spawnTimerText;
    
        public static UIManager Instance { get; set; }

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

        public void SetSimulationStatus(string status)
        {
            _simulationStatusText.GetComponent<Text>().text = status;
        }

        public void SetEntityCount(int entityCount)
        { 
            _entityCountText.GetComponent<Text>().text = "Entity Count: " + entityCount;
        }

        public void SetMaxEntityCount(int maxEntityCount)
        {
            _maxEntityCountText.GetComponent<Text>().text = "Max Entity Count: " + maxEntityCount;
        }

        public void SetSpawnTimer(float spawnTimer)
        {
            _spawnTimerText.GetComponent<Text>().text = "Entity will spawn in every " + spawnTimer + " seconds";
        }
    }
}
