using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using System;

public class PinConstraintGrabber : MonoBehaviour
{
    // Start is called before the first frame update
    ObiCloth currentCloth = null;
    GameObject sphere = null;
    public float sphereSize;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) { Grab(); }
        else if (Input.GetKeyUp(KeyCode.G)) { Release(); }
    }

    private ObiCloth GetCurrentCloth()
    {
        ObiCloth closestCloth = null;
        float closestDistance = float.MaxValue;

        foreach (ObiCloth cloth in FindObjectsOfType<ObiCloth>())
        {
            float distance = Vector3.Distance(cloth.transform.position, transform.position);
            if (distance < closestDistance) 
            {
                closestDistance = distance;
                closestCloth = cloth;
            }
        }
        return closestCloth;

    }

    ObiConstraints<ObiPinConstraintsBatch> pinConstraints;
    Dictionary<GameObject, ObiPinConstraintsBatch> currentPinConstraints = new Dictionary<GameObject, ObiPinConstraintsBatch>();
    public GameObject robotHand;

    public GameObject AddAttach(GameObject robotHand)
    {
        currentCloth = GetCurrentCloth();
        if (currentCloth == null)
            return null;
        Debug.Log("AddAttach");
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        
        sphere.transform.position = robotHand.transform.position;
        sphere.transform.SetParent(robotHand.transform);
        sphere.transform.localScale = Vector3.one * sphereSize;
        ObiCollider collider = sphere.AddComponent<ObiCollider>();
        sphere.GetComponent<MeshRenderer>().enabled = false;

        List<int> collisionParticleSolverIndex = new List<int>();

        for (int i = 0; i < currentCloth.particleCount; ++i)
        {
            Vector3 particlePosition = currentCloth.GetParticlePosition(i);
            float dis = Vector3.Distance(particlePosition, robotHand.transform.position);
            if (dis < 0.03f)
            {
                collisionParticleSolverIndex.Add(i);
            }
        }
        if(collisionParticleSolverIndex.Count ==0)return null;
        Debug.Log(string.Join(", ", collisionParticleSolverIndex));

        pinConstraints = currentCloth.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
        var batch = new ObiPinConstraintsBatch();
        foreach (var particleIndex in collisionParticleSolverIndex)
        {
            Vector3 particlePosition = currentCloth.GetParticlePosition(particleIndex);
            Vector3 colliderPosition = collider.transform.InverseTransformPoint(particlePosition);
            Quaternion particleRotation = currentCloth.GetParticleOrientation(particleIndex);
            Quaternion colliderRotation = Quaternion.Inverse(collider.transform.rotation) * particleRotation;
            batch.AddConstraint(particleIndex, collider, colliderPosition, particleRotation, 0, 0, float.PositiveInfinity);
            batch.activeConstraintCount++;
        }
        pinConstraints.AddBatch(batch);
        currentCloth.SetConstraintsDirty(Oni.ConstraintType.Pin);

        currentPinConstraints.Add(sphere, batch);
        return sphere;
    }
    public void RemoveAttach(GameObject sphere)
    {
        Debug.Log("RemoveAttach");
        if (currentPinConstraints.TryGetValue(sphere, out var obiPinConstraintsBatch))
            pinConstraints.RemoveBatch(obiPinConstraintsBatch);
        currentCloth.SetConstraintsDirty(Oni.ConstraintType.Pin);
        currentPinConstraints.Remove(sphere);
        Destroy(sphere);
    }
    private void Grab()
    {
        Debug.Log("Grab");
        if (robotHand != null ) { sphere = AddAttach(robotHand); }
        
    }

    private void Release()
    {
        Debug.Log("Release");
        if (sphere !=null ) { RemoveAttach(sphere); }

        
    }

}
