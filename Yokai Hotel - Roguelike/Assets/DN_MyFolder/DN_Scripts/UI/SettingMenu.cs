using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

    Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    private void Start()
    {
       resolutions = Screen.resolutions;

        if (resolutionDropdown)
        {
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
       
    }

    public void SetVolumeSFX(float volume)
    {
        audioMixer.SetFloat("sfx", volume);
    }

    public void SetVolumeMUSIC(float volume)
    {
        audioMixer.SetFloat("music", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

    }
}
