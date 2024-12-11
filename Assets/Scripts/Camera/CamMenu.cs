using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMenu : MonoBehaviour
{
    public Transform target;
    public float spinSpeed = 15f, height = 1f, bobSpeed = 1f;

    void Update()
    {
        Vector3 pos = transform.position;
        float newY = Mathf.Sin(Time.time * bobSpeed) * height;
        transform.position = new Vector3(pos.x, newY + 200, pos.z);

        transform.LookAt(target);
        transform.Translate(Vector3.right * Time.deltaTime * spinSpeed);
    }
}
