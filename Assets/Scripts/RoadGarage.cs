using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoadGarage : MonoBehaviour
{
    [SerializeField] private Image roadSelection;
    [SerializeField] private Sprite[] roadImage;
    [SerializeField] private GameObject settingObj;
    [SerializeField] private TextMeshProUGUI moneyText;
    private int roadIndex;
    private int moneyIndex;
    
    void Start()
    {
        roadIndex = 0;
        moneyIndex = PlayerPrefs.GetInt("Money");
        moneyText.text = "" + moneyIndex;
    }

    public void NextRoad()
    {
        AudioManager.instance.Play("BtnClick");
        roadIndex++;
        ShowRoad(roadIndex);
    }

    public void PrevRoad() 
    {
        AudioManager.instance.Play("BtnClick");
        roadIndex--;
        ShowRoad(roadIndex);
    }

    private void ShowRoad(int roadNumber)
    {
        if (roadNumber > roadImage.Length -1)
        {
            roadNumber = 0;
        }
        else if(roadNumber < 0)
        {
            roadNumber=roadImage.Length -1;
        }

        roadIndex = roadNumber;
        roadSelection.sprite = roadImage[roadIndex];
        PlayerPrefs.SetInt("MainRoad", roadIndex);
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
        settingObj.SetActive(true);
    }
}
