using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public void Start()
    {
        //fetch all resolutions available for the player's screen and store it in the array resolutions (no duplicates)
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        //Clear options A, B and C from the resolution dropdown
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        //index of the player's default resolution in the array of resolutions
        int currentResolutionIndex = 0;
        int i = 0;
        foreach (Resolution resolution in resolutions) {
            string option = resolution.width + "x" + resolution.height;
            options.Add(option);
            //test if resolution is the actual current resolution of the player
            if (resolution.width == Screen.width && resolution.height == Screen.height)
            {
                currentResolutionIndex = i;
            }
            i++;
        }

        resolutionDropdown.AddOptions(options);
        //set default resolution in the dropdown to player's current resolution
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue() ;

        //fullscreen mode by default    
        Screen.fullScreen = true;
    }

    //called whenever the user wants to change the volume with the slider
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);  
    }

    //activates or desactivates the fullscreen wether the checkbox is checked or not
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
