using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cameraPosition;


    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position - new Vector3(0, -2f, 6f);
    }
}