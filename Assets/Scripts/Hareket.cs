using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hareket : MonoBehaviour
{
    public static Hareket HareketInstance;

    [SerializeField] private PlayerSettingsScriptable settings;
    [SerializeField] private Camera uiCamera;

    private GameStates myState = GameStates.touch;
    private float _playerSpeedValue = 0;
    private Rigidbody _playerRigidbody;
    private Vector3 _firstPos;
    private Vector3 _mousePos;
    private Vector3 _diff;

    public GameStates MyState { get => myState; set => myState = value; }

    private void Awake()
    {
        HareketInstance = this;
    }
    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (MyState == GameStates.play)
        {
            _playerRigidbody.velocity = Vector3.Lerp(_playerRigidbody.velocity, new Vector3(_diff.x, _playerRigidbody.velocity.y, _playerSpeedValue), .1f);
        }
    }

    void Update()
    {
        if (myState == GameStates.ready || myState == GameStates.play)
        {
            TouchListener();
        }
    }

    public void TouchListener()
    {
        _firstPos = Vector3.Lerp(_firstPos, _mousePos, .1f);

        if (Input.GetMouseButtonDown(0))
            MouseDown(Input.mousePosition);

        else if (Input.GetMouseButtonUp(0))
            MouseUp();

        else if (Input.GetMouseButton(0))
            MouseHold(Input.mousePosition);
    }

    private void MouseDown(Vector3 inputPos)
    {
        _playerSpeedValue = settings.PlayerSpeedValue;
        _mousePos = uiCamera.ScreenToWorldPoint(inputPos);
        _firstPos = _mousePos;
    }

    private void MouseHold(Vector3 inputPos)
    {
        myState = GameStates.play;
        _mousePos = uiCamera.ScreenToWorldPoint(inputPos);
        _diff = _mousePos - _firstPos;
        _diff *= settings.Sensitivity;
    }

    private void MouseUp()
    {
        myState = GameStates.ready;
        _playerSpeedValue = 0;
        _diff = Vector3.zero;
        _playerRigidbody.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<ILevelTrigger>()?.NextLevel();

        if (other.TryGetComponent(out IWaitable waitable))
        {
            Invoke(nameof(CheckBalls), 4f);

            Invoke(nameof(Wait), .5f);

            waitable.Wait();

        }
    }

    public void Wait()
    {
        myState = GameStates.wait;
        _playerRigidbody.velocity = Vector3.zero;
    }
    public void CheckBalls()
    {
        Pool.PoolInstance.FirstCheck();
    }
}
