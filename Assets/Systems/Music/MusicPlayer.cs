using System.Collections;
using UnityEngine;

namespace FGJ24
{
    public class MusicPlayer : MonoBehaviour
    {
        public static MusicPlayer Instance { get; private set; }
        
        [SerializeField] private AudioSource _musicSource;
        
        [SerializeField] private AudioClip _menuMusic;
        [SerializeField] private float _fadeinTime = 1f;
        [SerializeField] private float _fadeoutTime = 1f;
        
        [SerializeField] private bool _shouldStopMusic = false;
        [SerializeField] private bool _shouldStartMusic = false;
        
        
        [SerializeField] private AudioClip _gameMusic;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            _musicSource.loop = true;
        }
        
        
        private void Update()
        {
            if (_shouldStopMusic)
            {
                _musicSource.Stop();
                _musicSource.volume = 1f; // Reset volume to original level
                _shouldStopMusic  = false;
            }
            
            if(!_shouldStopMusic && _shouldStartMusic)
            {
                _musicSource.Play();
                _shouldStartMusic = false;
            }
        }
        public void PlayMenuMusic()
        {
            StartCoroutine(PlayMenuMusicWhenReady());
        }
        
        public void PlayGameMusic()
        {
            StartCoroutine(PlayGameMusicWhenReady());
        }
        
        private IEnumerator PlayMenuMusicWhenReady()
        {
            // Wait until the current audio clip has finished playing
            if(_musicSource.isPlaying)
            {
                StartCoroutine(FadeOutMusic());
            }
            while (_musicSource.isPlaying)
            {
                yield return null;
            }

            // Start the new audio clip
            _musicSource.volume = 0;
            _musicSource.clip = _menuMusic;
            _shouldStartMusic = true;
            StartCoroutine(FadeInMusic());
        }
        
        
        private IEnumerator PlayGameMusicWhenReady()
        {
            // Wait until the current audio clip has finished playing
            if(_musicSource.isPlaying)
            {
                StartCoroutine(FadeOutMusic());
            }
            while (_musicSource.isPlaying)
            {
                yield return null;
            }

            // Start the new audio clip
            _musicSource.volume = 0;
            _musicSource.clip = _gameMusic;
            _shouldStartMusic = true;
            StartCoroutine(FadeInMusic());
        }
        
        private IEnumerator FadeInMusic()
        {
            float targetVolume = 1f;

            while (_musicSource.volume < targetVolume)
            {
                _musicSource.volume += Time.deltaTime / _fadeinTime;

                yield return null;
            }
        }
        
        public void StopMusic()
        {
            StartCoroutine(FadeOutMusic());
        }
        
        private IEnumerator FadeOutMusic()
        {
            float startVolume = _musicSource.volume;

            while (_musicSource.volume > 0)
            {
                _musicSource.volume -= startVolume * Time.deltaTime / _fadeoutTime;

                yield return null;
            }

            _shouldStopMusic = true;
        }
        
    }
}
