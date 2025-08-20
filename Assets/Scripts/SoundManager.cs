using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    // MP3 Player   -> AudioSource
    // MP3 음원     -> AudioClip
    // 관객(귀)     -> AudioListener
    [Range(0f, 1f)] public float BgmVolume = 1f;
    [Range(0f, 1f)] public float SfxVolume = 1f;

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound_Root");
        if (root == null)
        {
            root = new GameObject { name = "@Sound_Root" };
            root.transform.SetParent(GameObject.Find("@Managers").transform);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Define.Sound.Bgm].loop = true;

            
            Debug.Log("SoundManager Init!");

        }
    }

    

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path);
        Play(audioClip, type, pitch);
    }

	public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
	{
        if (audioClip == null)
            return;

		if (type == Define.Sound.Bgm)
		{
			AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
			if (audioSource.isPlaying)
				audioSource.Stop();

			audioSource.pitch = pitch;
			audioSource.clip = audioClip;
			audioSource.Play();
		}
		else
		{
			AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];

            if (audioSource.isPlaying)
            {
                Debug.Log("이미 재생중인 사운드!");
                return;
            }


            audioSource.pitch = pitch;
			audioSource.PlayOneShot(audioClip);
		}
	}

    private AudioClip GetOrAddAudioClip(string soundName)
    {
        if (_audioClips.ContainsKey(soundName))
            return _audioClips[soundName];

        AudioClip clip = Resources.Load<AudioClip>($"Sounds/{soundName}");
        if (clip != null)
        {
            _audioClips[soundName] = clip;
        }
        else
        {
            Debug.LogError($"AudioClip Missing: {soundName}");
        }
        return clip;
    }



    //MonoBehaviour + GameObject에 붙여야함
//    private IEnumerator LoadClipCoroutine(string fileName, System.Action<AudioClip> onLoaded)
//    {
//        if (_audioClips.ContainsKey(fileName))
//        {
//            onLoaded?.Invoke(_audioClips[fileName]);
//            yield break;
//        }

//        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

//#if UNITY_ANDROID && !UNITY_EDITOR
//        // Android는 StreamingAssets 접근 방식이 다릅니다.
//        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, GetAudioType(fileName)))
//#else
//        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, GetAudioType(fileName)))
//#endif
//        {
//            yield return www.SendWebRequest();

//            if (www.result == UnityWebRequest.Result.Success)
//            {
//                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
//                _audioClips[fileName] = clip;
//                onLoaded?.Invoke(clip);
//            }
//            else
//            {
//                Debug.LogError($"사운드 로드 실패: {filePath} ({www.error})");
//                onLoaded?.Invoke(null);
//            }
//        }
//    }

//    private AudioType GetAudioType(string fileName)
//    {
//        string ext = Path.GetExtension(fileName).ToLower();
//        switch (ext)
//        {
//            case ".wav": return AudioType.WAV;
//            case ".mp3": return AudioType.MPEG;
//            case ".ogg": return AudioType.OGGVORBIS;
//            default: return AudioType.UNKNOWN;
//        }
//    }

//    public void PlayBGM(string fileName)
//    {
//        StartCoroutine(LoadClipCoroutine(fileName, clip =>
//        {
//            if (clip != null)
//            {
//                _audioSources[(int)Define.Sound.Bgm].clip = clip;
//                _audioSources[(int)Define.Sound.Bgm].Play();
//            }
//        }));
//    }

//    public void PlaySFX(string fileName)
//    {
//        StartCoroutine(LoadClipCoroutine(fileName, clip =>
//        {
//            if (clip != null)
//            {
//                _audioSources[(int)Define.Sound.Effect].PlayOneShot(clip);
//            }
//        }));
//    }

    public void SetBgmVolume(float volume)
    {
        BgmVolume = Mathf.Clamp01(volume);
        _audioSources[(int)Define.Sound.Bgm].volume = BgmVolume;
    }

    public void SetSfxVolume(float volume)
    {
        SfxVolume = Mathf.Clamp01(volume);
    }

}
