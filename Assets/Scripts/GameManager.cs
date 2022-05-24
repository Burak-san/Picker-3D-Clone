using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;
    [SerializeField] private int levelCount;

    public int LevelCount
    {
        get => levelCount;
        set
        {
            levelCount = value;
            PlayerPrefs.SetInt(Constants.LEVELCOUNT, levelCount);
            UIManager.UIManagerInstance.SetLevelText(levelCount);
        }
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey(Constants.LEVELCOUNT))
        {
            levelCount = PlayerPrefs.GetInt(Constants.LEVELCOUNT);
        }
        else
        {
            PlayerPrefs.SetInt(Constants.LEVELCOUNT, 1);
            Awake();
        }
        GameManagerInstance = this;

    }

}

public enum GameStates
{
    touch,
    play,
    ready,
    wait,
    finish,
    lose
}
