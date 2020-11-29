using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	public class AudioManager<T> : MonoBehaviour where T : AudioManager<T>
	{
		public static T Instance { get; private set; }

		private readonly List<AudioSource> musicSources = new List<AudioSource>();
        private bool musicEnabled;
		public bool MusicEnabled
        {
            get => musicEnabled;
            set
            {
                musicEnabled = value;
				musicSources.ForEach(s => s.mute = !value);
            }
        }

		private readonly List<AudioSource> soundSources = new List<AudioSource>();
		private bool soundEnabled;
		public bool SoundEnabled
        {
            get => soundEnabled;
            set
            {
                soundEnabled = value;
				soundSources.ForEach(s => s.mute = !value);
            }
        }

        private void Awake()
		{
			if (Instance == null)
			{
				Instance = (T) this;
				DontDestroyOnLoad(gameObject);
			}
			else if (Instance != this)
			{
				Destroy(gameObject);
			}
		}

		private void Start()
		{
			SoundEnabled = true;
			MusicEnabled = true;
		}

		private void OnDestroy()
		{
			if (Instance == this) Instance = null;
		}

		protected void PlayAudio(AudioClipSettings settings)
		{
			var sourceList = settings.type == AudioType.Music ? musicSources : soundSources;
			var source = default(AudioSource);
			switch (settings.limitBehaviour)
			{
				case AudioLimitBehaviour.DiscardOldInstance:
					source = sourceList.Find(s => s.clip == settings.clip);
					if (source == default) source = sourceList.Find(s => !s.isPlaying);
					else source.Stop();
					break;
				case AudioLimitBehaviour.DiscardNewInstance:
					source = sourceList.Find(s => s.clip == settings.clip);
                    if (source == default) source = sourceList.Find(s => !s.isPlaying);
                    else return;
                    break;
				default:
					source = sourceList.Find(s => !s.isPlaying);
					break;
			}

			if (source == default) 
			{
				var obj = new GameObject($"{nameof(AudioSource)}{sourceList.Count + 1}");
				obj.transform.SetParent(transform);
				source = obj.AddComponent<AudioSource>();
				source.mute = settings.type == AudioType.Music ? !MusicEnabled : !SoundEnabled;
				source.playOnAwake = false;
				sourceList.Add(source);
			}

			source.loop = settings.type == AudioType.Music;
			source.clip = settings.clip;
			source.volume = settings.defaultVolume;
			source.Play();
		}
		
		private readonly Dictionary<AudioClipSettings, Coroutine> fadeCoroutines = new Dictionary<AudioClipSettings, Coroutine>();
		protected void FadeAudio(AudioClipSettings settings, float fadeTo, float fadeDuration)
		{
			var sourceList = settings.type == AudioType.Music ? musicSources : soundSources;
			var source = sourceList.Find(s => s.clip == settings.clip);
			if (source == null) return;

			if (fadeCoroutines.TryGetValue(settings, out var coroutine)) StopCoroutine(coroutine);
			coroutine = StartCoroutine(FadeAudioFlow(source, source.volume, fadeTo, fadeDuration));
			fadeCoroutines[settings] = coroutine;
		}
		private IEnumerator FadeAudioFlow(AudioSource source, float fadeFrom, float fadeTo, float fadeDuration)
		{
			var t = 0f;
			while (t < 1f)
			{
				yield return null;
				t += Time.deltaTime / fadeDuration;
				source.volume = Mathf.Lerp(fadeFrom, fadeTo, t);
			}
		}
	}

	public enum AudioType
	{
		Music = 0,
		Sound = 1,
	}

	public enum AudioLimitBehaviour
	{
		DoNotLimit = 0,
		DiscardOldInstance = 1,
		DiscardNewInstance = 2,
	}

	[Serializable]
	public class AudioClipSettings
	{
		public AudioType type;
		public AudioClip clip;
		[Range(0, 1)] public float defaultVolume = 1;
		public AudioLimitBehaviour limitBehaviour;
	}
}