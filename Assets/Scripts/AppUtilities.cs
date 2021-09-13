using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppUtilities : MonoBehaviour
{
	public static AppUtilities instance;
    [SerializeField] private TMP_Dropdown _res;
    [SerializeField] private Toggle _fs;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _quality;
    [SerializeField] private TMP_Dropdown _difficulty;
    private void Awake()
	{
		if (!instance)
		{
			instance = this;
			// DontDestroyOnLoad(instance);
		}
		else
			Destroy(gameObject);

		// force res at 1920x1080
		Screen.SetResolution(1920, 1080, false);
	}

    void Start()
    {
        _slider.value = QualitySettings.GetQualityLevel();
        _quality.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        _slider.onValueChanged.AddListener(delegate { ChangeQuality(); });
        _fs.isOn = Screen.fullScreen;
        _fs.onValueChanged.AddListener(delegate { ChangeRes(_res, _fs.isOn); });
        _res.onValueChanged.AddListener(delegate { ChangeRes(_res, _fs.isOn); });
        _difficulty.onValueChanged.AddListener(delegate { ChangeDifficulty(_difficulty); });
        int i = 0;
        foreach (TMP_Dropdown.OptionData option in _res.options)
        {
            if (option.text == $"{Screen.width}x{Screen.height}")
            {
                _res.value = i;
                break;
            }
            i++;
        }
        Debug.Log(_res.itemText.text);
    }

    public void GoToScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void GoToScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void QuitApp()
	{
		Application.Quit();
	}

    private void ChangeRes(TMP_Dropdown res, bool fullscreen)
    {
        string[] splitted = res.options[res.value].text.Split('x');
        Debug.Log($"{splitted[0]}");
        Screen.SetResolution(int.Parse(splitted[0]), int.Parse(splitted[1]), fullscreen);
    }

    private void ChangeDifficulty(TMP_Dropdown difficulty)
    {
        Debug.Log("@@@@");
        Debug.Log(difficulty.value);

        DifficultyManager.inst.difficulty = eDifficulty.easy;

        if (difficulty.value == 1)
        {
            DifficultyManager.inst.difficulty = eDifficulty.medium;
        }
        else if (difficulty.value == 2)
        {
            DifficultyManager.inst.difficulty = eDifficulty.hard;
        }


    }

    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel((int)_slider.value);
        _quality.text = QualitySettings.names[(int)_slider.value];
    }
}
