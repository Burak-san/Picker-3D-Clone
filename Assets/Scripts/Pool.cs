using UnityEngine;
using DG.Tweening;

public class Pool : MonoBehaviour, IPool
{
    public static Pool PoolInstance;

    [SerializeField] private Color platformColor, poolColor;
    [SerializeField] private Transform leftBarrier, rightBarrier;
    [SerializeField] private Level level;

    private UIManager _uiManager;
    private Hareket _hareket;
    private TextMesh _textMesh;
    private int _ballInPool = 0;
    private Material _material;
    private bool _lockObj = true;

    public int BallInPool { get => _ballInPool; set => _ballInPool = value; }

    private void Awake()
    {
        PoolInstance = this;
    }

    void Start()
    {
        _uiManager = UIManager.UIManagerInstance;
        _hareket = Hareket.HareketInstance;
        _material = GetComponent<MeshRenderer>().material;
        _textMesh = GetComponentInChildren<TextMesh>();
        _textMesh.text = "0" + "/" + (level.CheckPointIndex * 4);
    }


    public void Count()
    {
        _ballInPool++;
        _textMesh.text = _ballInPool + "/" + (level.CheckPointIndex*4);
        Invoke(nameof(CheckBalls), 2f);
    }

    public void CheckBalls()
    {
        if (_hareket.MyState != GameStates.lose && _lockObj)
        {
            if (_ballInPool >= (level.CheckPointIndex * 4))
            {
                _lockObj = false;
                level.Finish.Wait();
                _material.DOColor(platformColor, .5f);
                transform.DOMoveY(0, .5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    for (int i = 0; i < level.BallList.Count; i++)
                    {
                        Destroy(level.BallList[i]);
                    }
                    
                    int levelindex = level.CheckPointIndex;
                    if (levelindex % 3 == 0)
                    {
                        GameManager.GameManagerInstance.LevelCount++;
                    }
                    _textMesh.text = "";
                    _hareket.MyState = GameStates.ready;
                    PlayerPrefs.SetInt(Constants.CHECKPOÝNTCOUNT, levelindex);
                    _uiManager.SetUI(levelindex);

                });
                leftBarrier.DOLocalRotate(new Vector3(0, 0, 75), 1f);
                rightBarrier.DOLocalRotate(new Vector3(0, 0, -75), 1f);
            }
            else if (_hareket.MyState == GameStates.wait)
            {
                _hareket.MyState = GameStates.lose;
                _uiManager.Dead();
            }
        }

    }

    public void FirstCheck()
    {
        if (_hareket.MyState == GameStates.wait && _ballInPool == 0)
        {
            _hareket.MyState = GameStates.lose;
            _uiManager.Dead();
        }
    }

    public void Revert()
    {
        leftBarrier.DOLocalRotate(new Vector3(0, 0, 0), 0f);
        rightBarrier.DOLocalRotate(new Vector3(0, 0, 0), 0f);

        _material.color = poolColor;
        transform.DOMoveY(-2, 0f);
        _ballInPool = 0;
        _lockObj = true;
        _textMesh.text = _ballInPool + "/" + (level.CheckPointIndex * 4);
    }
}
