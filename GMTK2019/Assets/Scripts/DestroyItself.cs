using UnityEngine;

public class DestroyItself : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
}
