using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/Managers/AudioManager")]
    public class AudioManager : BaseManager<AudioManager>
    {
        [Tooltip("背景音乐集")]
        public List<AudioClip> BackgroundMusics;

        [Tooltip("音乐会")]
        public List<AudioClip> VocalConcert;

        [Tooltip("音效集")]
        public List<AudioClip> SoundEffects;

        [Tooltip("Boss战")]
        public AudioClip Boss;

        [Tooltip("购物")]
        public AudioClip Shopping;

        [Tooltip("禅境花园")]
        public AudioClip Garden;

        [Tooltip("菜单")]
        public AudioClip Menu;

        [Tooltip("僵尸音效集合，僵尸只使用这些AudioSource进行播放，以免声音过于杂乱")]
        public List<AudioSource> ZombieSounds;

        [Tooltip("其他音效")]
        public List<AudioSource> AudioLists { get; set; } = new List<AudioSource>();

        /// <summary>
        /// 背景音
        /// </summary>
        public AudioSource BackmusicPlayer { get; private set; }
        /// <summary>
        /// 音效
        /// </summary>
        public AudioSource EffectPlayer { get; private set; }

        private float fadeBeforeVolume;

        protected override void Initialize()
        {
            base.Initialize();
            InitAudio();
        }

        private void InitAudio()
        {
            BackmusicPlayer = gameObject.AddComponent<AudioSource>();
            BackmusicPlayer.loop = true;
            BackmusicPlayer.playOnAwake = false;
            EffectPlayer = gameObject.AddComponent<AudioSource>();
            EffectPlayer.playOnAwake = false;
            AudioLists.AddRange(ZombieSounds);
        }

        public void ChangeMusicVolume(float volume)
        {
            BackmusicPlayer.volume = volume;
        }

        public void ChangeEffectVolume(float volume)
        {
            EffectPlayer.volume = volume;
            foreach (var item in AudioLists)
            {
                item.volume = volume;
            }
        }

        public void PlayBackMusic(float delay)
        {
            int index = Random.Range(0, BackgroundMusics.Count);
            BackmusicPlayer.clip = BackgroundMusics[index];
            BackmusicPlayer.PlayDelayed(delay);
        }

        public void StopBackMusic()
        {
            BackmusicPlayer.Stop();
        }

        public void FadeOutInBackMusic()
        {
            if (fadeBeforeVolume != 0)
                return;
            fadeBeforeVolume = BackmusicPlayer.volume;
            if (fadeBeforeVolume != 0)
                StartCoroutine(Fade());
        }

        System.Collections.IEnumerator Fade()
        {
            for (int i = 0; i < 35; i++)
            {
                BackmusicPlayer.volume = fadeBeforeVolume - i / (fadeBeforeVolume * 35);
                yield return new WaitForSeconds(0.1f);
            }
            BackmusicPlayer.volume = fadeBeforeVolume;
            fadeBeforeVolume = 0;
        }

        public void PlayVocalConcertMusic(float delay)
        {
            int index = Random.Range(0, VocalConcert.Count);
            BackmusicPlayer.clip = VocalConcert[index];
            BackmusicPlayer.PlayDelayed(delay);
        }

        public void PlayShoppingMusic(float delay)
        {
            BackmusicPlayer.clip = Shopping;
            BackmusicPlayer.PlayDelayed(delay);
        }

        public void PlayGardenMusic()
        {
            BackmusicPlayer.clip = Garden;
            BackmusicPlayer.Play();
        }

        public void PlayEffectSound(AudioClip clip)
        {
            EffectPlayer.clip = clip;
            EffectPlayer.Play();
        }

        public void PlayEffectSoundByName(string effectName, float pitch = 1)
        {
            AudioClip clip = null;
            foreach (var item in SoundEffects)
            {
                if (item.name == effectName)
                    clip = item;
            }
            if (clip != null)
            {
                EffectPlayer.clip = clip;
                EffectPlayer.pitch = pitch;
                EffectPlayer.Play();
            }
            else
            {
                Debug.LogError("AudioManager中没有" + effectName + "这个音效");
            }
        }

        public void PlayEffectSoundByIndex(int index)
        {
            if (index >= SoundEffects.Count)
            {
                Debug.LogError("请在AudioManager增加音效，大于" + index);
                return;
            }
            EffectPlayer.clip = SoundEffects[index];
            EffectPlayer.Play();
        }

        public AudioSource RandomPlayZombieSounds()
        {
            int index = Random.Range(0, ZombieSounds.Count);
            var zombieSound = ZombieSounds[index];
            if (!zombieSound.isPlaying)
            {
                zombieSound.pitch = Random.Range(1.2f, 1.5f);
                zombieSound.Play();
                return zombieSound;
            }
            return null;
        }
    }
}