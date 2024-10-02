using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDiatance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBGM;
    public int bgmIndex;

    private bool canPlaySFX;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke("AllowSFX", 1f);
    }

    private void Update()
    {
        if (!playBGM)
            StopALLBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                bgm[bgmIndex].Play();
        }
    }

    public void PlaySFX(int _sfxIndex, Transform _source, bool isPitch = true)
    {
        if (!canPlaySFX)
            return;

        if (sfx[_sfxIndex].isPlaying)
            return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDiatance)
            return;

        if(_sfxIndex < sfx.Length)
        {
            if(isPitch)
                sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex]?.Stop();

    public void StopSFXWithEase(int _sfxIndex) => StartCoroutine(SFXVolumeDecreaseStop(_sfxIndex));

    private IEnumerator SFXVolumeDecreaseStop(int _sfxIndex)
    {
        AudioSource audio = sfx[_sfxIndex];
        float defaultVolume = audio.volume;
        while(audio.volume > 0.1f)
        {
            audio.volume -= audio.volume * 0.2f;
            yield return new WaitForSeconds(0.25f);
            if(audio.volume <= 0.1f)
            {
                audio.Stop();
                audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopALLBGM();
        bgm[bgmIndex].Play();
    }

    public void PlayRandomBGM()
    {
        int index = Random.Range(0, bgm.Length);
        PlayBGM(index);
    }

    public void StopALLBGM()
    {
        for(int i=0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;
}
