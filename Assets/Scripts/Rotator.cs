using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    // Update is called once per frame
    private void Update()
    {
        // multiply by deltaTime to make the action smooth
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
/*Prefab that asset that contains a template or blueprint ofa game object or game object family. Can use the prefab
 into any scene and all of the pickup objects will be updated with the changes*/