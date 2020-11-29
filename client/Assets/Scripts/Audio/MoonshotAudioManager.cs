using UnityEngine;

namespace Audio
{
    public class MoonshotAudioManager : AudioManager<MoonshotAudioManager>
    {
        [Header("Music")]
        public float musicFadeDuration = 1;
        public AudioClipSettings ambientMusic;
        public AudioClipSettings combatMusic;

        public void StartMusic()
        {
            PlayAudio(ambientMusic);
            PlayAudio(combatMusic);
        }

        public void FadeCombatMusic(bool state)
        {
            FadeAudio(combatMusic, state ? 1 : 0, musicFadeDuration);
        }
    }
}
