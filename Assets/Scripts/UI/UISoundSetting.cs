using UnityEngine;

public class UISoundSetting : MonoBehaviour
{
    private static readonly string BackgroundPref = "BackgroundPref";
    private static readonly string SoundEffectPref = "SoundEffectPref";
    private float backgroundFloat, soundEffectFloat;
    public AudioSource backgroundAudio;
    public AudioSource[] soundEffectAudio;

    void Awake()
    {
        ContinueSetting();
    }

    private void ContinueSetting()
    {
        backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
        soundEffectFloat = PlayerPrefs.GetFloat(SoundEffectPref);

        backgroundAudio.volume = backgroundFloat;

        for (int i = 0; i < soundEffectAudio.Length; i++)
        {
            soundEffectAudio[i].volume = soundEffectFloat;
        }
    }

}
