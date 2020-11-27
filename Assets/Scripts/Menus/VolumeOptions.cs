using UnityEngine;
using UnityEngine.UI;

public class VolumeOptions : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    
    public void OnMasterVolumeChanged()
    {
        PlayerPrefs.SetFloat("volume", masterVolumeSlider.value);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    public void OnMusicVolumeChanged()
    {
        
    }

    public void OnSfxVolumeChanged()
    {
        
    }
}
