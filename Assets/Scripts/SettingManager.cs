using EasyUI.Toast;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    private int controls;
    [SerializeField] private Button Music;
    [SerializeField] private Sprite SoundOn;
    [SerializeField] private Sprite SoundOff;
    
    private void OnEnable()
    {
        UpdateSoundUI();
    }
    
    public void BtnKeys()
    {
        AudioManager.instance.Play("BtnClick");
        controls = 1;
        PlayerPrefs.SetInt("CurrControls", controls);
        Toast.Show("<color=white>Enable Keys</color>",3f,ToastColor.Purple);
    }

    public void BtnButtons()
    {
        AudioManager.instance.Play("BtnClick");
        controls = 2;
        PlayerPrefs.SetInt("CurrControls", controls);
        Toast.Show("<color=white>Enable Keys</color>", 3f, ToastColor.Purple);
    }

    public void BtnTouch()
    {
        AudioManager.instance.Play("BtnClick");
        controls = 3;
        PlayerPrefs.SetInt("CurrControls", controls);
        Toast.Show("<color=white>Enable Keys</color>", 3f, ToastColor.Purple);
    }

    public void BtnSensor()
    {
        AudioManager.instance.Play("BtnClick");
        controls = 4;
        PlayerPrefs.SetInt("CurrControls", controls);
        Toast.Show("<color=white>Enable Keys</color>", 3f, ToastColor.Purple);
    }

    public void BtnMusic()
    {
        AudioManager.instance.ToggleMusic();
        UpdateSoundUI();
    }

    public void UpdateSoundUI()
    {
        if (AudioManager.instance.isMuted)
        {
            Music.image.sprite = SoundOff;
        }
        else
        {
            Music.image.sprite = SoundOn;
        }
    }
}
