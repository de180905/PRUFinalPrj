using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    public AudioMixer audioMixer; // Tham chi?u ??n AudioMixer
    private UIDocument uiDocument;

    private VisualElement mainMenu;
    private VisualElement settingsPanel;
    private Slider volumeSlider;
    private Label volumeLabel;
    private Label lastScoreLabel;
    private Label highestScoreLabel;
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
        lastScoreLabel = root.Q<Label>("last-score-label");
        highestScoreLabel = root.Q<Label>("highest-score-label");
        // Gán s? ki?n cho các nút
        startButton.clicked += StartGame;
        settingsButton.clicked += OpenSettings;
        quitButton.clicked += QuitGame;
        backButton.clicked += CloseSettings;

        // S? ki?n thay ??i âm l??ng
        volumeSlider.RegisterValueChangedCallback(evt => ChangeVolume(evt.newValue));
        LoadAndDisplayScores();
    }

    private void StartGame()
    {
        Debug.Log("Start Game!");
        SceneManager.LoadScene(0);
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
    private void LoadAndDisplayScores()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        int highestScore = PlayerPrefs.GetInt("HighestScore", 0);

        lastScoreLabel.text = $"Last Score: {lastScore}";
        highestScoreLabel.text = $"Highest Score: {highestScore}";
    }
}
