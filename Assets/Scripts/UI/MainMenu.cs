using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    public AudioMixer audioMixer; // Tham chi?u ??n AudioMixer
    private UIDocument uiDocument;

    private VisualElement mainMenu;
    private VisualElement settingsPanel;
    private Slider volumeSlider;
    private Label volumeLabel;

    void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // L?y UI Elements
        mainMenu = root.Q<VisualElement>("menu-container");
        settingsPanel = root.Q<VisualElement>("settings-panel");

        var startButton = root.Q<Button>("start-button");
        var settingsButton = root.Q<Button>("settings-button");
        var quitButton = root.Q<Button>("quit-button");
        var backButton = root.Q<Button>("back-button");

        volumeSlider = root.Q<Slider>("volume-slider");
        volumeLabel = root.Q<Label>("volume-label");

        // Gán s? ki?n cho các nút
        startButton.clicked += StartGame;
        settingsButton.clicked += OpenSettings;
        quitButton.clicked += QuitGame;
        backButton.clicked += CloseSettings;

        // S? ki?n thay ??i âm l??ng
        volumeSlider.RegisterValueChangedCallback(evt => ChangeVolume(evt.newValue));
    }

    private void StartGame()
    {
        Debug.Log("Start Game!");
        // Load scene game (ví d?: SceneManager.LoadScene("GameScene");)
    }

    private void OpenSettings()
    {
        mainMenu.style.display = DisplayStyle.None;
        settingsPanel.style.display = DisplayStyle.Flex;
    }

    private void CloseSettings()
    {
        settingsPanel.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.Flex;
    }

    private void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    private void ChangeVolume(float value)
    {
        int volumePercent = Mathf.RoundToInt(value * 100);
        volumeLabel.text = $"Volume: {volumePercent}%";
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20); // Chuy?n ??i giá tr? thành dB
    }
}
