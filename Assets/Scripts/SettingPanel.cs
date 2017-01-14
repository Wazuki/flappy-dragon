using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour {

    public Image soundIcon, musicIcon;
    public Sprite soundIconSprite, musicIconSprite;
    public Sprite soundMuteSprite, musicMuteSprite;

    public Slider soundSlider, musicSlider;

    public AudioSource musicAudioSource;

    private float soundSliderPrevVolume, musicSliderPrevVolume;

    private bool resetHighScoreAttempt;

	// Use this for initialization
	void Start ()
    {
        gameObject.SetActive(false);

        soundSlider.onValueChanged.AddListener(delegate { SoundSliderValueChangeCheck(); });
        musicSlider.onValueChanged.AddListener(delegate { MusicSliderValueChangeCheck(); });

        musicAudioSource.ignoreListenerVolume = true;

        if (PlayerPrefs.HasKey("MusicVol")) musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        if (PlayerPrefs.HasKey("SoundVol")) soundSlider.value = PlayerPrefs.GetFloat("SoundVol");

        resetHighScoreAttempt = false;

        GameManager gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        Button[] buttons = gameObject.GetComponentsInChildren<Button>();

        foreach (Button b in buttons)
        {
            b.onClick.AddListener(gameManagerScript.PlaySoundOnClick);
        }
    }

    public void ResetHighScore()
    {
        if(!resetHighScoreAttempt)
        {
            GameObject.Find("Reset High Score Button Text").GetComponent<Text>().text = "TAP TO CONFIRM";
            resetHighScoreAttempt = true;
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.Save();

            GameObject.Find("High Score").GetComponent<Text>().text = PlayerPrefs.GetInt("HighScore").ToString();
            GameObject.Find("Reset High Score Button Text").GetComponent<Text>().text = "RESET HIGH SCORE";
            resetHighScoreAttempt = false;
        }

    }

    public void ShowSettingsButton()
    {
        gameObject.SetActive(true);

        //Disable Play and Settings button while the settings pane is open
        GameObject.Find("Play Button").GetComponent<Button>().enabled = false;
        GameObject.Find("Settings Button").GetComponent<Button>().enabled = false;
    }

    public void CloseSettingsButton()
    {
        //Save the settings made to PlayerPrefs
        PlayerPrefs.SetFloat("SoundVol", soundSlider.value);
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);

        GameObject.Find("Play Button").GetComponent<Button>().enabled = true;
        GameObject.Find("Settings Button").GetComponent<Button>().enabled = true;

        GameObject.Find("Reset High Score Button Text").GetComponent<Text>().text = "RESET HIGH SCORE";

        resetHighScoreAttempt = false;
        gameObject.SetActive(false);
    }

    public void SoundMuteButton()
    {
        //Flip the state based on the value of the slider
        if(soundSlider.value != 0f)
        {
            //Set the old position in case the button gets clicked again
            soundSliderPrevVolume = soundSlider.value;
            soundIcon.sprite = soundMuteSprite;
            soundSlider.value = 0f;
        }
        else
        {
            soundIcon.sprite = soundIconSprite;
            soundSlider.value = soundSliderPrevVolume;
        }
    }

    public void MusicMuteButton()
    {
        //Flip the state based on the value of the slider
        if (musicSlider.value != 0f)
        {
            //Set the old position in case the button gets clicked again
            musicSliderPrevVolume = musicSlider.value;
            musicIcon.sprite = musicMuteSprite;
            musicSlider.value = 0f;
        }
        else
        {
            musicIcon.sprite = musicIconSprite;
            musicSlider.value = musicSliderPrevVolume;
        }
    }

    public void SoundSliderValueChangeCheck()
    {
        AudioListener.volume = soundSlider.value;
        
        if (soundSlider.value == 0)
        {
            soundIcon.sprite = soundMuteSprite;
        }
        else
        {
            soundIcon.sprite = soundIconSprite;
        }
    }

    public void MusicSliderValueChangeCheck()
    {
        musicAudioSource.volume = musicSlider.value;

        if (musicSlider.value == 0)
        {
            musicIcon.sprite = musicMuteSprite;
        }
        else
        {
            musicIcon.sprite = musicIconSprite;
        }
    }

}
