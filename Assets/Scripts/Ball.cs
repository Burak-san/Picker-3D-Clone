using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IPool>()?.Count();
    }
}
