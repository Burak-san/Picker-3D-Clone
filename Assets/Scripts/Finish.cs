using UnityEngine;

public class Finish : MonoBehaviour,IWaitable
{
    private BoxCollider _finishTrigger;
    public BoxCollider FinishTrigger { get => _finishTrigger; set => _finishTrigger = value; }

    private void Start()
    {
        _finishTrigger = GetComponent<BoxCollider>();
    }
    public void Wait()
    {
        _finishTrigger.enabled = false;
    }

    public void ColliderEnable()
    {
        _finishTrigger.enabled = true;
    }

}
