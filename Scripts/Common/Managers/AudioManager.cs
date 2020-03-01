using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TableConfigs;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("AudioManager").AddComponent<AudioManager>();
        }
        return _instance;
    }
    private Dictionary<int, string> audioPathDict;      // 存放音频文件路径
    private AudioSource musicAudioSource;
    private List<AudioSource> unusedSoundAudioSourceList;   // 存放可以使用的音频组件
    private List<AudioSource> usedSoundAudioSourceList;     // 存放正在使用的音频组件
    private Dictionary<int, AudioClip> audioClipDict;       // 缓存音频文件
    private float bgSoundVolume = 0.5f;
    private float effectSoundVolume = 0.5f;
    private int poolCount = 3;         // 对象池数量
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        InitAudioPath();
        //初始的时候就加一个音源上去对象上播放背景音，特效的音效什么时候有了再动态加
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        unusedSoundAudioSourceList = new List<AudioSource>();
        usedSoundAudioSourceList = new List<AudioSource>();
        audioClipDict = new Dictionary<int, AudioClip>();
    }
    void Start()
    {
        // 从本地缓存读取声音音量
        if (GameDataManager.GetInstance().GetAllAccount().Count <= 0)
        {
            bgSoundVolume = 0.5f;
            effectSoundVolume = 0.5f;
        }
        else
        {
            bgSoundVolume = GetLocalSoundVolume();
            effectSoundVolume = GetLocalSoundVolume();
        }

    }
    /// <summary>
    /// 初始化音源的路径
    /// </summary>
    public void InitAudioPath()
    {
        audioPathDict = new Dictionary<int, string>();
        List<int> ids = TableManager.GetInstance().GetAllIds("AudioBase");
        for (int i = 0; i < ids.Count; i++)
        {
            string soundPath = GetSoundPathById(ids[i]);
            audioPathDict.Add(ids[i], soundPath);
        }
    }
    /// <summary>
    /// 保存音频的音量到本地
    /// </summary>
    /// <param name="value"></param>
    public void SaveSoundVolumeToLocal(float value)
    {
        GameDataManager.GetInstance().SaveUserSoundValue(GameDataManager.UserAccount + "", value);
    }
    /// <summary>
    /// 得到本地保存的音频音量
    /// </summary>
    /// <returns></returns>
    public float GetLocalSoundVolume()
    {
        int lastId = GameDataManager.GetInstance().GetAllAccount()[GameDataManager.GetInstance().GetAllAccount().Count - 1];
        return GameDataManager.GetInstance().LoadUserData(lastId + "").SoundValue;
    }

    /// <summary>
    /// 返回音频的路径 如果以后改路径的话需要改这里的代码
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetSoundPathById(int id)
    {
        string soundName = GetSoundNameById(id);
        //规定:一百以下为背景音 一百以上为音效
        string path = "Sound/";
        if (id >= 1 && id < 100)
        {
            path += "Background/";
        }
        else
        {
            path += "EffectSound/";
        }
        return path + soundName;

    }
    /// <summary>
    /// 通过音频的Id得到音频的名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetSoundNameById(int id)
    {
        Table_AudioBase_DefineData data = TableManager.GetInstance().GetDataById("AudioBase", id) as Table_AudioBase_DefineData;

        string name = data.Name;

        return name;
    }

    /// <summary>
    /// 这个音频是否循环
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool SoundIsLoop(int id)
    {
        Table_AudioBase_DefineData data = TableManager.GetInstance().GetDataById("AudioBase", id) as Table_AudioBase_DefineData;
        bool bResult = true;
        bResult = data.loop == 0 ? false : true;  //0-不循环 1-循环
        return bResult;
    }
    
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="id"></param>
    /// <param name="loop"></param>
    public void PlayBgSound(int id)
    {
        // 通过Tween将声音淡入淡出
        DOTween.To(() => musicAudioSource.volume, value => musicAudioSource.volume = value, 0, 0.5f).OnComplete(() =>
        {
            musicAudioSource.clip = GetAudioClip(id);
            if (!musicAudioSource.clip)
                return;
            musicAudioSource.clip.LoadAudioData();
            musicAudioSource.loop = SoundIsLoop(id);
            musicAudioSource.volume = bgSoundVolume;
            musicAudioSource.Play();
            DOTween.To(() => musicAudioSource.volume, value => musicAudioSource.volume = value, bgSoundVolume, 0.5f);
        });
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="id"></param>
    public void PlayEffectSound(int id, Action action = null)
    {
        //先查看有没有闲置的音源
        if (unusedSoundAudioSourceList.Count != 0)
        {
            //有闲置的音源从那里面拿然后放到正在使用的里面
            AudioSource audioSource = UnusedToUsed();
            audioSource.clip = GetAudioClip(id);
            if (!audioSource.clip)
                return;
            audioSource.clip.LoadAudioData();
            audioSource.Play();
            StartCoroutine(WaitPlayEnd(audioSource, action));
        }
        else
        {
            AddAudioSource();
            AudioSource audioSource = UnusedToUsed();
            audioSource.clip = GetAudioClip(id);
            if (!audioSource.clip)
                return;
            audioSource.clip.LoadAudioData();
            audioSource.volume = effectSoundVolume;
            audioSource.loop = false;   //音效设死为不能循环
            audioSource.Play();
            StartCoroutine(WaitPlayEnd(audioSource, action));
        }
    }
    /// <summary>
    /// 当播放音效结束后，将其移至未使用集合
    /// </summary>
    /// <param name="audioSource"></param>
    /// <returns></returns>
    IEnumerator WaitPlayEnd(AudioSource audioSource, Action action)
    {
        yield return new WaitUntil(() => { return !audioSource.isPlaying; });
        UsedToUnused(audioSource);
        if (action != null)
        {
            //加上一个播放完成后的回调吧，不知道会不会用得上
            action();
        }
    }
    /// <summary>
    /// 获取音频文件，获取后会缓存一份
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private AudioClip GetAudioClip(int id)
    {
        if (!audioClipDict.ContainsKey(id))
        {
            if (!audioPathDict.ContainsKey(id))
            {
                Debug.LogError("未成功添加音频对应的Id和路径:"+id);
                return null;
            }
            //加载音频剪辑
            AudioClip audio = ResourcesMgr.GetInstance().LoadResource<AudioClip>(audioPathDict[id], false);
            //加到缓存中
            audioClipDict.Add(id, audio);
        }
        return audioClipDict[id];
    }
    /// <summary>
    /// 添加音频组件
    /// </summary>
    /// <returns></returns>
    private AudioSource AddAudioSource()
    {
        if (unusedSoundAudioSourceList.Count != 0)
        {
            return UnusedToUsed();
        }
        else
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            unusedSoundAudioSourceList.Add(audioSource);
            return audioSource;
        }
    }
    /// <summary>
    /// 将未使用的音频组件移至已使用集合里
    /// </summary>
    /// <returns></returns>
    private AudioSource UnusedToUsed()
    {
        AudioSource audioSource = unusedSoundAudioSourceList[0];
        unusedSoundAudioSourceList.RemoveAt(0);
        usedSoundAudioSourceList.Add(audioSource);
        return audioSource;
    }
    /// <summary>
    /// 将使用完的音频组件移至未使用集合里
    /// </summary>
    /// <param name="audioSource"></param>
    private void UsedToUnused(AudioSource audioSource)
    {
        if (usedSoundAudioSourceList.Contains(audioSource))
        {
            usedSoundAudioSourceList.Remove(audioSource);
        }
        if (unusedSoundAudioSourceList.Count >= poolCount)
        {
            Destroy(audioSource);
        }
        else if (audioSource != null && !unusedSoundAudioSourceList.Contains(audioSource))
        {
            unusedSoundAudioSourceList.Add(audioSource);
        }
    }
    /// <summary>
    /// 修改背景音乐音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeBgSoundVolume(float volume)
    {
        bgSoundVolume = volume;
        musicAudioSource.volume = volume;
        SaveSoundVolumeToLocal(volume);
    }
    /// <summary>
    /// 修改音效音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeEffectSoundVolume(float volume)
    {
        effectSoundVolume = volume;
        for (int i = 0; i < unusedSoundAudioSourceList.Count; i++)
        {
            unusedSoundAudioSourceList[i].volume = volume;
        }
        for (int i = 0; i < usedSoundAudioSourceList.Count; i++)
        {
            usedSoundAudioSourceList[i].volume = volume;
        }
        SaveSoundVolumeToLocal(volume);
    }
    /// <summary>
    /// 静音
    /// </summary>
    public void MuteSound()
    {
        ChangeBgSoundVolume(0);
        ChangeEffectSoundVolume(0);
    }
}
