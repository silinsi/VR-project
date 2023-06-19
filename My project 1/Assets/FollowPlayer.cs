using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();


        
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 randomVector = new Vector3(Random.Range(-3f,3f), Random.Range(-3f, 3f), 0.0f); 
        nav.SetDestination(target.position+randomVector);
        
    }
}
