using UnityEngine;

public class UISettings : MonoBehaviour
{
    [SerializeField] private ToggleButton hapticToggle;
    [SerializeField] private ToggleButton soundToggle;
    [SerializeField] private ToggleButton musicToggle;
    
    private void Start()
    {
        hapticToggle.SetValue(DataManager.Vibration);
        soundToggle.SetValue(DataManager.Sound);
    }

    public void OnSoundToggled(bool value)
    {
        DataManager.Sound = value;
        GameManager.Instance.audioManager.SetAudioActive(value);
    }

    public void OnHapticToggled(bool value)
    {
        DataManager.Vibration = value;
        HapticManager.SetHapticsActive(value);
    }
}
