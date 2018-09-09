using UnityEngine;

public class DragMouseOrbit : MonoBehaviour
{
    public float speed = 2;
    public GameObject target;

    public Transform StartPos;
    
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(target.transform.position, target.transform.up, Input.GetAxis("Mouse X") * speed);
        }
        
    }

    void OnEnable()
    {
        transform.position = StartPos.position;
        transform.rotation = StartPos.rotation;
    }

}
