using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BikeController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float rotationspeed = 1.0f;
    [SerializeField] private float rotationAngle = 1.0f;
    [SerializeField] private float currBikeSpeed = 0.5f;
    private bool goingLeft;
    private bool goingRight;
    private float currHealth;
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private Slider healthSlider;
    private int score = 0;
    private int highScore = 0;
    private int money = 0;
    private float gameTime = 0;
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private GameObject pauseObj;
    [SerializeField] private GameObject contorlsObj;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private SpriteRenderer bikeSelection;
    [SerializeField] private Sprite[] bikeImages;
    private int currBike;
    private int currControls;
    [SerializeField] private Button BtnLeft;
    [SerializeField] private Button BtnRight;
    [SerializeField] private Button BtnPause;
    [SerializeField] private Road BikeRoad;
    [SerializeField] private ObjGenerator objGenerator;
    [SerializeField] private float maxSpeed = 2.5f;
    [SerializeField] private float minSpeed = 0.5f;

    void Start()
    {
        score = PlayerPrefs.GetInt("Score");
        highScore = PlayerPrefs.GetInt("HighScore");
        currHealth = maxHealth;
        setHealth(currHealth);
        currBike = PlayerPrefs.GetInt("MainBike");
        currControls = PlayerPrefs.GetInt("CurrControls");

        if (currControls == 2)
        {
            BtnLeft.gameObject.SetActive(true);
            BtnRight.gameObject.SetActive(true);
        }
        else if(currControls == 1) 
        {
            contorlsObj.SetActive(false);
        }
        else
        {
            BtnLeft.gameObject.SetActive(false);
            BtnRight.gameObject.SetActive(false);
            contorlsObj.SetActive(true);
        }

        bikeSelection.sprite = bikeImages[currBike];
        AudioManager.instance.Play("Bike");

        BikeRoad.SetSpeed(currBikeSpeed);
        objGenerator.SetSpeed(currBikeSpeed);
        SetMaxSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        DetectInput();

        if (goingLeft)
        {
            LeftSide();
        }
        else if (goingRight)
        {
           RightSide();
        }
        else
        {
            resetRotation();
        }

        gameTime += Time.deltaTime;
        score = (int)gameTime;
        Debug.Log(score);
        money = (int)(score/5);
        PlayerPrefs.SetInt("Money", money);
    }

    private void CheckHighScore()
    {
        if (highScore < score)
        {
            highScore=score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    private void SetMaxSpeed()
    {
        switch(BikeRoad.currRoad)
        {
            case 0:
            {
                maxSpeed = 2.5f;
                break;
            }
            case 1:
            {
                maxSpeed = 2f;
                break;
            }
            case 2:
            {
                maxSpeed = 1.5f;
                break;
            }
        }
    }

    private void DetectInput()
    {
        switch (currControls)
        {
            case 1:
            {
                MoveOnKeys();
                break;
            }
            case 2: 
            {
                break; 
            }
            case 3:
            {
                MoveOnTouch();
                break;
            }
            case 4:
            {
                MoveOnSensor();
                break;
            }
        }
    }

    private void LeftSide()
    {
        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotationAngle), rotationspeed * Time.deltaTime);
    }
    private void RightSide()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -rotationAngle), rotationspeed * Time.deltaTime);
    }

    private void MoveOnKeys()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            goingLeft = true;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            goingRight = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Decelerate();
        }
        else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Accelerate();
        }
        else 
        { 
            goingLeft = false; 
            goingRight= false;
        }
    }

    private void MoveOnTouch()
    {
        if (Input.touchCount > 0)
        {
            Vector2 pos=Input.GetTouch(0).position;
            float middle=Screen.width / 2;

            if(pos.x < middle)
            {
                goingLeft=true;
            }
            else if (pos.x > middle)
            {
                goingRight=true;
            }   
        }
        else
        {
            goingLeft = false;
            goingRight = false;
        }
    }

    private void MoveOnSensor()
    {
        if (Input.acceleration.x < -0.1f)
        {
            goingLeft=true;
        }
        else if (Input.acceleration.x > 0.1f)
        {
            goingRight = true;
        }
        else
        {
            goingLeft = false;
            goingRight=false;
        }
    }

    private void resetRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), rotationspeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            currHealth--;
            setHealth(currHealth);
            AudioManager.instance.Play("Crash");

            Destroy(collision.gameObject);

            if (currHealth <= 0)
            {
                Debug.Log("Game Over");
                GameOver();
            }
        }
        else if (collision.gameObject.CompareTag("fuel"))
        {
            if(currHealth < maxHealth)
            {
                currHealth++;
                setHealth(currHealth);
                AudioManager.instance.Play("Fuel");
            }

            Destroy(collision.gameObject);
            AudioManager.instance.Play("Fuel");
        }
        else if (collision.gameObject.CompareTag("bumper"))
        {
            score -= 5;
            Debug.Log(score);
            currHealth--;
            setHealth(currHealth);
            AudioManager.instance.Play("Crash");
            Destroy(collision.gameObject);
        }
    }

    private void setHealth(float val)
    {
        healthSlider.value = val;   
    }

    private void GameOver()
    {
        gameOverObj.SetActive(true);
        Time.timeScale = 0f;
        scoreText.text = "Score :" + score;

        CheckHighScore();

        highScoreText.text = "HighScore :" + highScore;
        moneyText.text = "Money :" + money;
        AudioManager.instance.Play("Game Over");
        AudioManager.instance.Stop("Bike");
        AudioManager.instance.Stop("BikeBg");
        BtnPause.gameObject.SetActive(false);
        contorlsObj.SetActive(false);
    }

    public void GamePause()
    {
        AudioManager.instance.Play("BtnClick");
        pauseObj.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.instance.Stop("Bike");
        AudioManager.instance.Stop("BikeBg");
        BtnPause.gameObject.SetActive(false);
        contorlsObj.SetActive(false);
    }

    public void Resume()
    {
        AudioManager.instance.Play("BtnClick");
        pauseObj.SetActive(false);
        Time.timeScale = 1.0f;
        AudioManager.instance.Play("Bike");
        AudioManager.instance.Play("BikeBg");
        BtnPause.gameObject.SetActive(true);
        if (currControls == 1)
        {
            contorlsObj.SetActive(false);
        }
        else
        {
            contorlsObj.SetActive(true);
        }
    }

    public void Restart()
    {
        AudioManager.instance.Play("BtnClick");
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
        gameOverObj.SetActive(false);
        score = 0;
        money = 0;
        PlayerPrefs.SetInt("Score",score);
        AudioManager.instance.Stop("Game Ovar");
        AudioManager.instance.Play("BikeBg");
    }

    public void GameHome()
    {
        AudioManager.instance.Play("BtnClick");
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
        AudioManager.instance.Play("Home");
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void RightBtnDown()
    {
        goingRight=true;
    }

    public void RightBtnUp()
    {
        goingRight=false;
    }

    public void LeftBtnDown()
    {
        goingLeft=true;
    }

    public void LeftBtnUp()
    {
        goingLeft=false;
    }

    public void Accelerate()
    {
        currBikeSpeed += 0.1f;
        SpeedLimit();
        BikeRoad.SetSpeed(currBikeSpeed);
        objGenerator.SetSpeed(currBikeSpeed);
    }

    public void Decelerate()
    {
        currBikeSpeed -= 0.1f;
        SpeedLimit();
        BikeRoad.SetSpeed(currBikeSpeed);
        objGenerator.SetSpeed(currBikeSpeed);
    }

    private void SpeedLimit()
    {
        currBikeSpeed=Mathf.Clamp(currBikeSpeed,minSpeed,maxSpeed);
    }
}
