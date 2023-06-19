using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class meshCutter : MonoBehaviour
{
    public List<GameObject> fruitPrefabs;
    public List<GameObject> halfFruitPrefabs;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        Debug.Log(collidedObject);

        // check if the collided object is cuttable
        if (fruitPrefabs.Contains(collidedObject))
        {
            int fruitIndex = fruitPrefabs.IndexOf(collidedObject);

            if (fruitIndex != -1 && fruitIndex < halfFruitPrefabs.Count)
            {
                Vector3 postion = collidedObject.transform.position;
                Quaternion rotation = collidedObject.transform.rotation;
                Vector3 scale = collidedObject.transform.localScale;

                // destroy the collided object
                Destroy(collidedObject);

                // Instantiate two half fruits at the same position
                GameObject HalfFront = Instantiate(halfFruitPrefabs[2 * fruitIndex], postion, rotation);
                GameObject HalfBack = Instantiate(halfFruitPrefabs[2 * fruitIndex + 1], postion, rotation);
                HalfFront.gameObject.layer=28;
                HalfBack.gameObject.layer=28;
                HalfFront.transform.localScale = 0.1f * scale;
                HalfBack.transform.localScale = 0.1f * scale;
                HalfFront.gameObject.AddComponent<MeshCollider>();
                HalfFront.gameObject.AddComponent<Rigidbody>();
                HalfFront.GetComponent<MeshCollider>().convex = true;
                HalfFront.GetComponent<MeshCollider>().sharedMesh = HalfFront.GetComponent<MeshFilter>().mesh;
                HalfFront.AddComponent<XRGrabInteractable>();
                HalfBack.gameObject.AddComponent<MeshCollider>();
                HalfBack.gameObject.AddComponent<Rigidbody>();
                HalfBack.GetComponent<MeshCollider>().convex = true;
                HalfBack.GetComponent<MeshCollider>().sharedMesh = HalfBack.GetComponent<MeshFilter>().mesh;
                HalfBack.AddComponent<XRGrabInteractable>();
            }
        }
    }

}
