using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _musicAudioSource;
    [SerializeField]
    private AudioSource _sfxAudioSource;

    [SerializeField]
    private Image _musicCheckbox;
    [SerializeField]
    private Image _sfxCheckbox;

    [SerializeField]
    private AudioClip _ballOnRacketBounceClip;
    [SerializeField]
    private AudioClip _ballOnAnySurfaceBounceClip;
    [SerializeField]
    private AudioClip _uiButtonClip;
    [SerializeField]
    private AudioClip _gameLoseClip;


    private void Awake()
    {
        ActualizeMusicActivation();
        ActualizeSFXActivation();
    }

    private void ActualizeMusicActivation()
    {
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        if (_musicCheckbox != null) _musicCheckbox.enabled = musicEnabled;
        if (_musicAudioSource != null) _musicAudioSource.mute = !musicEnabled;
    }

    private void ActualizeSFXActivation()
    {
        bool sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        if (_sfxCheckbox != null) _sfxCheckbox.enabled = sfxEnabled;
        if (_sfxAudioSource != null) _sfxAudioSource.mute = !sfxEnabled;
    }

    public void SwitchMusicActivation()
    {
        int currentActivationValue = PlayerPrefs.GetInt("MusicEnabled", 1);
        int newActivationValue = currentActivationValue == 1 ? 0 : 1;

        PlayerPrefs.SetInt("MusicEnabled", newActivationValue);
        ActualizeMusicActivation();
    }

    public void SwitchSFXActivation()
    {
        int currentActivationValue = PlayerPrefs.GetInt("SFXEnabled", 1);
        int newActivationValue = currentActivationValue == 1 ? 0 : 1;

        PlayerPrefs.SetInt("SFXEnabled", newActivationValue);
        ActualizeSFXActivation();
    }

    public void PlayBallBounceSound(bool onRacket)
    {
        if(onRacket)
        {
            _sfxAudioSource.PlayOneShot(_ballOnRacketBounceClip);
        }
        else
        {
            _sfxAudioSource.PlayOneShot(_ballOnAnySurfaceBounceClip);
        }
    }

    public void PlayUISound()
    {
        _sfxAudioSource.PlayOneShot(_uiButtonClip);
    }

    public void PlayGameLoseSound()
    {
        _sfxAudioSource.PlayOneShot(_gameLoseClip);
    }

}