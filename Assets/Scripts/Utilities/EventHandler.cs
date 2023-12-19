using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//委任イベントのスクリプトについて
public static class EventHandler
{
    #region BGMとSE関連
    //シーンロード後のトリガーイベント
    public static event Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }

    //サウンドオブジェクトプールから再生イベントを生成し、サウンドを初期化する
    public static event Action<SoundDetails> InitSoundEffect;
    public static void CallInitSoundEffect(SoundDetails soundDetails)
    {
        InitSoundEffect?.Invoke(soundDetails);
    }

    //コール再生イベント、サウンドの再生
    public static event Action<SoundName> PlaySoundEvent;
    public static void CallPlaySoundEvent(SoundName soundName)
    {
        PlaySoundEvent?.Invoke(soundName);
    }
    #endregion
}
