using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_Dropdown screenModeDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Toggle vSyncToggle;
    public Slider brightnessSlider;
    public Image brightnessOverlay;

    private const string PREF_SCREEN_MODE = "ScreenMode";
    private const string PREF_RESOLUTION = "ResolutionIndex";
    private const string PREF_VSYNC = "VSync";
    private const string PREF_BRIGHTNESS = "Brightness";

    private Resolution[] resolutions;

    void Start()
    {
        SetupScreenModeDropdown();
        SetupResolutionDropdown();
        SetupVSyncToggle();
        SetupBrightnessSlider();
    }

    // ======================
    //  SCREEN MODE
    // ======================
    void SetupScreenModeDropdown()
    {
        screenModeDropdown.ClearOptions();
        screenModeDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "Full Screen",
            "Window",
            "Screen with borders"
        });

        int savedMode = PlayerPrefs.GetInt(PREF_SCREEN_MODE, -1);
        int current = 0;

        if (savedMode != -1)
        {
            current = savedMode;
            ApplyScreenMode(current);
        }
        else
        {
            switch (Screen.fullScreenMode)
            {
                case FullScreenMode.Windowed: current = 0; break;
                case FullScreenMode.ExclusiveFullScreen: current = 1; break;
                case FullScreenMode.FullScreenWindow: current = 2; break;
            }
        }

        screenModeDropdown.value = current;
        screenModeDropdown.RefreshShownValue();
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
    }

    public void SetScreenMode(int index)
    {
        ApplyScreenMode(index);
        PlayerPrefs.SetInt(PREF_SCREEN_MODE, index);
        PlayerPrefs.Save();
    }

    private void ApplyScreenMode(int index)
    {
        switch (index)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.Windowed; break;
            case 1: Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
            case 2: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
        }
    }

    // ======================
    //  RESOLUTIONS
    // ======================
    void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        resolutions = Screen.resolutions;
        var options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (!options.Contains(option)) // evitar duplicados
            {
                options.Add(option);
            }

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = options.IndexOf(option);
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedResolutionIndex = PlayerPrefs.GetInt(PREF_RESOLUTION, -1);
        if (savedResolutionIndex != -1 && savedResolutionIndex < options.Count)
        {
            currentResolutionIndex = savedResolutionIndex;
            ApplyResolution(currentResolutionIndex);
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int index)
    {
        ApplyResolution(index);
        PlayerPrefs.SetInt(PREF_RESOLUTION, index);
        PlayerPrefs.Save();
    }

    private void ApplyResolution(int index)
    {
        string[] dims = resolutionDropdown.options[index].text.Split('x');
        int width = int.Parse(dims[0]);
        int height = int.Parse(dims[1]);

        Screen.SetResolution(width, height, Screen.fullScreenMode);
    }

    // ======================
    //  V-SYNC
    // ======================
    void SetupVSyncToggle()
    {
        int savedVSync = PlayerPrefs.GetInt(PREF_VSYNC, -1);

        if (savedVSync != -1)
        {
            ApplyVSync(savedVSync == 1);
            vSyncToggle.isOn = savedVSync == 1;
        }
        else
        {
            bool current = QualitySettings.vSyncCount > 0;
            vSyncToggle.isOn = current;
        }

        vSyncToggle.onValueChanged.AddListener(SetVSync);
    }

    public void SetVSync(bool enabled)
    {
        ApplyVSync(enabled);
        PlayerPrefs.SetInt(PREF_VSYNC, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyVSync(bool enabled)
    {
        QualitySettings.vSyncCount = enabled ? 1 : 0;
    }

    // ======================
    //  BRIGHTNESS
    // ======================
    void SetupBrightnessSlider()
    {
        float savedBrightness = PlayerPrefs.GetFloat(PREF_BRIGHTNESS, -1);

        if (savedBrightness != -1)
        {
            ApplyBrightness(savedBrightness);
            brightnessSlider.value = savedBrightness;
        }
        else
        {
            // Mitad del slider por defecto
            brightnessSlider.value = 0.5f;
            ApplyBrightness(0.5f);
        }

        brightnessSlider.onValueChanged.AddListener(SetBrightness);
    }

    public void SetBrightness(float value)
    {
        ApplyBrightness(value);
        PlayerPrefs.SetFloat(PREF_BRIGHTNESS, value);
        PlayerPrefs.Save();
    }

    private void ApplyBrightness(float value)
    {
        // value: 0 = oscuro, 1 = claro
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1f - value; // invertir (slider 1 = overlay invisible, slider 0 = overlay negro)
            brightnessOverlay.color = c;
        }
    }
}