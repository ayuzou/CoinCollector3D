using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // To stop camera from falling player without rotating

    public GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called once per frame, but will run after all items have been processed and update
    // Use for: follow cameras, procedural animation, gathering last known states
    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
