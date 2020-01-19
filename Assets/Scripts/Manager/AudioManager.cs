using UnityEngine;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        public AudioClip buttonClick;
        public AudioClip collision;

        public static AudioManager Instance { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }    }

        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayButtonClick()
        {
            _audioSource.PlayOneShot(buttonClick);
        }

        public void PlayCollision()
        {
            _audioSource.PlayOneShot(collision);
        }
    
    }
}
