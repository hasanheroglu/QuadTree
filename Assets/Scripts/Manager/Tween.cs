using System.Collections;
using UnityEngine;

namespace Manager
{
    public class Tween : MonoBehaviour
    {
        public static Tween Instance { get; set; }
        // Start is called before the first frame update
        void Awake()
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

        public IEnumerator Shrink(GameObject gameObject, Vector3 startSize, Vector3 endSize, float duration)
        {
            float startTime = Time.time;
            while (Time.time < startTime + duration)
            {
                gameObject.transform.localScale = Vector3.Lerp(startSize, endSize, (Time.time - startTime)/duration);
                yield return null;
            }

            gameObject.transform.localScale = endSize;
        }

        public IEnumerator Grow(GameObject gameObject, Vector3 startSize, Vector3 endSize, float duration)
        {
            float startTime = Time.time;
            while (Time.time < startTime + duration)
            {
                gameObject.transform.localScale = Vector3.Lerp(startSize, endSize, (Time.time - startTime)/duration);
                yield return null;
            }

            gameObject.transform.localScale = endSize;
        }
    }
}
