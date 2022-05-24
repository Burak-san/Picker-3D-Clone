using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager UIManagerInstance;

    [SerializeField] private GameObject touchPanel, deadPanel, nextLevelPanel;
    [SerializeField] private Text scoreText, currentLevelText, nextLevelText, highScoreText;
    [SerializeField] private Image dot1, dot2, dot3;

    private Hareket _hareket;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(Constants.BESTSCORE))
        {
            highScoreText.text = "Best Score : " + PlayerPrefs.GetInt(Constants.BESTSCORE);
        }
        UIManagerInstance = this;
    }

    void Start()
    {
        _hareket = Hareket.HareketInstance;
        if (PlayerPrefs.HasKey(Constants.CHECKPOÝNTCOUNT) && PlayerPrefs.GetInt(Constants.CHECKPOÝNTCOUNT) % 3 != 0)
        {
            SetUI(PlayerPrefs.GetInt(Constants.CHECKPOÝNTCOUNT));
        }
        else
        {
            SetLevelText(GameManager.GameManagerInstance.LevelCount);
            SetDots(0, 0, 0);
        }
    }

    public void SetUI(int _checkpoint)
    {
        SetLevelText(GameManager.GameManagerInstance.LevelCount);
        if (_checkpoint % 3 == 1)
        {
            SetDots(1, 0, 0);
        }
        else if (_checkpoint % 3 == 2)
        {
            SetDots(1, 1, 0);
        }
        else
        {
            SetDots(1, 1, 1);
            NextLevel();
        }
    }
    public void SetDots(int x, int y, int z)
    {
        dot1.DOFillAmount(x, 1f);
        dot2.DOFillAmount(y, 1f);
        dot3.DOFillAmount(z, 1f);
    }
    public void SetLevelText(int level)
    {
        currentLevelText.text = level.ToString();
        nextLevelText.text = (level + 1).ToString();
    }

    public void NextLevel()
    {
        nextLevelPanel.SetActive(true);
        _hareket.MyState = GameStates.finish;
    }
    public void NextLevelButton()
    {
        SetDots(0, 0, 0);
        nextLevelPanel.SetActive(false);
        _hareket.MyState = GameStates.ready;

    }

    public void CloseTouchPanel()
    {
        touchPanel.SetActive(false);
        _hareket.MyState = GameStates.ready;
    }

    public void Dead()
    {
        deadPanel.SetActive(true);
        int levelCount = PlayerPrefs.GetInt(Constants.LEVELCOUNT);
        int bestScore = PlayerPrefs.GetInt(Constants.BESTSCORE);
        scoreText.text = "Best Score : " + bestScore;

        if (levelCount > bestScore)
        {
            
            bestScore = levelCount;
            PlayerPrefs.SetInt(Constants.BESTSCORE, bestScore);
        }

        PlayerPrefs.DeleteKey(Constants.CHECKPOÝNTCOUNT);
        PlayerPrefs.DeleteKey(Constants.LEVELCOUNT);

    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
