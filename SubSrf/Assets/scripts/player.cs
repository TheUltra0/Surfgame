using UnityEngine;

public class player : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    public Transform player2;

    float xRotation;

    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
}