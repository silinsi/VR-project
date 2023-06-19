using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamekit3D.GameCommands
{
    
        public class collision_trans : TriggerCommand
        {
            //public LayerMask layers;
            //public GameCommandType interactionType;
            void OnTriggerEnter(Collider other)
            {
                
                Debug.Log("trans");
                SceneManager.LoadScene("Scenes/clothscene");
            }
        }

}

