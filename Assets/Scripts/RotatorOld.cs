using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Rotate coin object that player must collect to gain points
 * Prefab for the asset that contains a template or blueprint of a game object or game object family
 * Can use the prefab into any scene and all of the pickup objects will be updated with the changes
 */

public class RotatorOld : MonoBehaviour {
    public float speed;

    // Update is called once per frame
    private void Update() {
        // multiply by deltaTime to make the action smooth
        transform.Rotate(new Vector3(15, 30, 45) * speed * Time.deltaTime);
    }
}
