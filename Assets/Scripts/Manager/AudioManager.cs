using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class AudioManager : Singleton<AudioManager>
{
    [Header("音乐SO")]
    public SoundDetailsList_SO soundDetailsData;
    public SceneSoundList_SO sceneSoundData;
    [Header("Audio Source")]
    public AudioSource ambientSource;
    public AudioSource gameSource;

    //随机播放时间
    //public float MusicStartSecond => Random.Range(5f,15f);
    //private Coroutine soundRoutine;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    //AudioMixer状态
    [Header("Snapshots")]
    public AudioMixerSnapshot normalSnapShot;
    public AudioMixerSnapshot muteSnapShot;

    //音乐结束时间
    private float musicTransitionSecond = 3f;


    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.PlaySoundEvent += OnPlaySoundEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.PlaySoundEvent -= OnPlaySoundEvent;
    }

    /// <summary>
    /// 播放事件设定
    /// </summary>
    /// <param name="soundName"></param>
    private void OnPlaySoundEvent(SoundName soundName)
    {
        var soundDetails = soundDetailsData.GetSoundDetails(soundName);
        if (soundDetails != null)
            EventHandler.CallInitSoundEffect(soundDetails);
    }

    /// <summary>
    /// 场景BGM播放设置
    /// </summary>
    private void OnAfterSceneLoadedEvent()
    {
        //播放相关BGM
        string currentScene = SceneManager.GetActiveScene().name;

        //获得对应场景BGM
        SceneSoundItem sceneSound = sceneSoundData.GetSceneSoundItem(currentScene);

        //防止BUG
        if (sceneSound == null)
            return;

        //获取对应环境音和BGM
        SoundDetails ambient = soundDetailsData.GetSoundDetails(sceneSound.ambient);
        SoundDetails music = soundDetailsData.GetSoundDetails(sceneSound.music);

        //音乐渐入渐出
        PlayerAmbientClip(ambient,0.5f);
        PlayerMusicClip(music,musicTransitionSecond);

        ////延迟播放
        //if (soundRoutine != null)
        //{
        //    StopCoroutine(soundRoutine);
        //}
        //else
        //{
        //    soundRoutine = StartCoroutine(PlaySoundRoutine(music, ambient));
        //}
    }

    /// <summary>
    /// 音乐播放
    /// </summary>
    /// <param name="soundDetails"></param>
    private void PlayerMusicClip(SoundDetails soundDetails , float transitionTime)
    {
        //获取音频混合器的相应轨迹
        audioMixer.SetFloat("MusicVolume", ConertSoundVolume(soundDetails.soundVolume));
        gameSource.clip = soundDetails.soundClip;
        if (gameSource.isActiveAndEnabled)
            gameSource.Play();

        normalSnapShot.TransitionTo(transitionTime);
    }

    /// <summary>
    /// 播放环境音
    /// </summary>
    /// <param name="soundDetails"></param>
    private void PlayerAmbientClip(SoundDetails soundDetails, float transitionTime)
    {
        audioMixer.SetFloat("AmbientVolume", ConertSoundVolume(soundDetails.soundVolume));
        ambientSource.clip = soundDetails.soundClip;
        if (gameSource.isActiveAndEnabled)
            gameSource.Play();

        normalSnapShot.TransitionTo(transitionTime);
    }

    //遅延播放
    //private IEnumerator PlaySoundRoutine(SoundDetails music, SoundDetails ambient)
    //{
    //    if (music != null && ambient != null)
    //    {
    //        PlayerAmbientClip(ambient);
    //        yield return new WaitForSeconds(MusicStartSecond);
    //        PlayerMusicClip(music);
    //    }
    //}

    //将音量调整到（-80,20）的范围区间
    private float ConertSoundVolume(float amount)
    {
        return (amount * 100 - 80);
    }
}
