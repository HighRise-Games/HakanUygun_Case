using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip up, down, pop, confetti, tap, slap;
    private AudioSource _standard, _increasing;

    public AudioManager Initialize()
    {
        var mainCameraGameObject = GameManager.Instance.cameraManager.mainCameraBrain.gameObject;
        
        _standard = mainCameraGameObject.AddComponent<AudioSource>();
        _increasing = mainCameraGameObject.gameObject.AddComponent<AudioSource>();
        
        SetAudioActive(DataManager.Sound);

        return this;
    }

    public void SetAudioActive(bool status)
    {
        _increasing.volume = _standard.volume = status ? 1 : 0;
    }

    public void PlayPop()
    {
        Play(pop);
    }

    public void PlayConfetti()
    {
        Play(confetti);
    }

    [ContextMenu("Play Tap")]
    public void PlayTap()
    {
        Play(tap);
    }
    
    public void PlayUIButtonClick()
    {
        Play(tap);
    }

    public void PlaySlap(int count)
    {
        PlayWithPitch(slap, 1 + (count * 0.1f));
    }

    public void PlayUp(int count)
    {
        PlayWithPitch(up, 1 + (count * 0.1f));
    }

    public void PlayDown(int count)
    {
        PlayWithPitch(down, 1 + (count * 0.1f));
    }

    private void Play(AudioClip clip)
    {
        if (!DataManager.Sound)
            return;
        
        _standard.PlayOneShot(clip);
    }

    private void PlayWithPitch(AudioClip clip, float pitch)
    {
        if (!DataManager.Sound)
            return;
        
        _increasing.pitch = pitch;
        _increasing.PlayOneShot(clip);
    }
}