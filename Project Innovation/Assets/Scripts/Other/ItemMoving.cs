using UnityEngine;

public class ItemMoving : MonoBehaviour
{
    public Vector3 Force;

    private void Update()
    {
        GetComponent<Rigidbody>().velocity = Force;
    }
}