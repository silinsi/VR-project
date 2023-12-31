using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiCollider))]
public class ObiClothGrapper : MonoBehaviour
{
    public bool canGrab = true;
    ObiSolver solver;
    ObiCollider obiCollider;
    public ObiCloth cloth;
    ObiSolver.ObiCollisionEventArgs collisionEvent;
    ObiPinConstraintsBatch newBatch;
    ObiConstraints<ObiPinConstraintsBatch> pinConstriants;

    void Awake()
    {
        obiCollider = GetComponent<ObiCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        pinConstriants = cloth.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;   
        
    }
    private void OnEnable()
    {
        solver = cloth.solver;
        if (solver != null)
        {
            solver.OnCollision += Solver_OnCollision;
        }
    }
    private void OnDisable()
    {
        if (solver != null)
        {
            solver.OnCollision -= Solver_OnCollision;
        }
    }

    private void Solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
    {
        collisionEvent = e;
    }

    public void Grab()
    {
        var world = ObiColliderWorld.GetInstance();
        Debug.Log(pinConstriants);
        Debug.Log(solver + " " + collisionEvent);

        if (solver != null && collisionEvent != null)
        {
            Debug.Log("Collision");
            foreach (Oni.Contact contact in collisionEvent.contacts)
            {
                Debug.Log(contact.distance);
                if (contact.distance < 0.01f)
                {
                    var contactCollider = world.colliderHandles[contact.bodyB].owner;
                    ObiSolver.ParticleInActor pa = solver.particleToActor[contact.bodyA];

                    Debug.Log(pa + "hit" + contactCollider);
                    if (canGrab)
                    {
                        if (contactCollider == obiCollider)
                        {
                            Debug.Log("Hand Collision");
                            var batch = new ObiPinConstraintsBatch();
                            int solverIndex = contact.bodyA;
                            Vector3 positionWS = solver.transform.TransformPoint(solver.positions[solverIndex]);
                            Vector3 positionCS = obiCollider.transform.InverseTransformPoint(positionWS);
                            batch.AddConstraint(solverIndex, obiCollider, positionCS, Quaternion.identity, 0, 0, float.PositiveInfinity);
                            batch.activeConstraintCount = 1;
                            newBatch = batch;
                            pinConstriants.AddBatch(newBatch);

                            canGrab = false;

                            cloth.SetConstraintsDirty(Oni.ConstraintType.Pin);
                        }
                    }
                }
            }
        }
    }

    public void Release()
    {
        if (!canGrab)
        {
            Debug.Log("Release");
            pinConstriants.RemoveBatch(newBatch);
            cloth.SetConstraintsDirty(Oni.ConstraintType.Pin);
            canGrab = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Grab");
            Grab();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Debug.Log("Release");
            Release();
        }
    }
}
