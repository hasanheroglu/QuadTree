using UnityEngine;

namespace Manager
{
    public class LineDrawer : MonoBehaviour
    {
        public static LineDrawer Instance { get; set; }
    
        public GameObject line;
        public GameObject lineParent;

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
            lineParent.SetActive(false);
        }

        public void DrawLine(Vector3 position, Vector3 startPoint, Vector3 endPoint)
        {
            var gameObject = Instantiate(line, position, Quaternion.identity, lineParent.transform);
            gameObject.transform.SetPositionAndRotation(position, Quaternion.identity);
            var lineRenderer = gameObject.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }

        public void ToggleTree()
        {
            lineParent.SetActive(!lineParent.activeSelf);
        }

        public void Clear()
        {
            for (int i = 0; i < lineParent.transform.childCount; i++)
            {
                Destroy(lineParent.transform.GetChild(i).gameObject);
            }
        }
    }
}
