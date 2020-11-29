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

        public void FadeInCombatMusic()
        {
            FadeAudio(combatMusic, 1, musicFadeDuration);
        }

        public void FadeOutCombatMusic()
        {
            FadeAudio(combatMusic, 0, musicFadeDuration);
        }
    }
}
