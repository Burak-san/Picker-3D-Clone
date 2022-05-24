using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour, ILevelTrigger
{
    [SerializeField] private GameObject ballGameObject;
    [SerializeField] private Pool pool;
    [SerializeField] private Finish finish;
    [SerializeField] private int checkPointIndex;
    [SerializeField] private List<GameObject> ballList = new List<GameObject>();

    private List<Vector3> positionList = new List<Vector3>();
    private float x, y, z;

    public List<GameObject> BallList { get => ballList; set => ballList = value; }
    public int CheckPointIndex { get => checkPointIndex; }
    public Finish Finish { get => finish; set => finish = value; }

    private void Awake()
    {
        if (PlayerPrefs.HasKey(Constants.CHECKPOÝNTCOUNT))
        {
            checkPointIndex += PlayerPrefs.GetInt(Constants.CHECKPOÝNTCOUNT);
        }
    }
    void OnEnable()
    {
        x = transform.position.x - 3f;
        y = transform.position.y;
        z = transform.position.z + 30f;
        positionList.Clear();
        ballList.Clear();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                positionList.Add(new Vector3(x, y, z));
                z += 5;
            }
            x += 3;
        }

        for (int i = 0; i < checkPointIndex + 1; i++)
        {
            int random = Random.Range(0, positionList.Count);
            GameObject ball = Instantiate(ballGameObject, positionList[random], Quaternion.identity);
            positionList.RemoveAt(random);
            ball.transform.SetParent(transform);
            ballList.Add(ball);
        }
    }
    public void NextLevel()
    {
        gameObject.SetActive(false);
        Finish.ColliderEnable();
        transform.position += new Vector3(0f, 0f, 330f);
        checkPointIndex += 3;
        Invoke(nameof(Active), 3f);
    }

    public void Active()
    {
        gameObject.SetActive(true);
        pool.Revert();
    }

}
