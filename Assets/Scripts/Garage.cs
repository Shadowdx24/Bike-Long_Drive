using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Garage : MonoBehaviour
{
    [SerializeField] private Image bikeSelection;
    [SerializeField] private Sprite[] bikeImage;
    [SerializeField] private GameObject GameSettingObj;
    [SerializeField] private TextMeshProUGUI moneyText;
    private int bikeIndex;
    private int moneyIndex;

    void Start()
    {
        bikeIndex = 0;
        moneyIndex = PlayerPrefs.GetInt("Money");
        moneyText.text = "" + moneyIndex;
    }

    public void BtnNext()
    {
        AudioManager.instance.Play("BtnClick");
        bikeIndex++;
        ShowCar(bikeIndex);
    }

    public void BtnPrev()
    {
        AudioManager.instance.Play("BtnClick");
        bikeIndex--;
        ShowCar(bikeIndex);
    }

    private void ShowCar(int bikeNumber)
    {
        if (bikeNumber > bikeImage.Length - 1)
        {
            bikeNumber = 0;
        }
        else if (bikeNumber < 0)
        {
            bikeNumber = bikeImage.Length - 1;
        }

        bikeIndex = bikeNumber;
        bikeSelection.sprite = bikeImage[bikeIndex];
        PlayerPrefs.SetInt("MainBike", bikeIndex);
    }

    public void GameStart()
    {
        AudioManager.instance.Play("BtnClick");
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
        AudioManager.instance.Stop("Home");
        AudioManager.instance.Play("BikeBg");
    }

    public void GameHome()
    {
        AudioManager.instance.Play("BtnClick");
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void GameSetting()
    {
        AudioManager.instance.Play("BtnClick");
        GameSettingObj.SetActive(true);
    }
}
