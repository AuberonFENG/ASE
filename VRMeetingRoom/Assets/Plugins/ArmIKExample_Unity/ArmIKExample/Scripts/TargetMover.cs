
using UnityEngine;

public class TargetMover : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(x, y, 0) * speed * Time.deltaTime);
    }
}
