using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcDialog : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject toActivate;

    [SerializeField] private Transform standingPoint;

    private Transform avatar;
    private int x=0;
    
    private async void OnTriggerEnter(Collider other)
    {
        print(other);
        print(x);
        if (other.CompareTag("Player") && x==0)
        {
            //avatar = other.transform;
            //print("ddd");
            // disable player input
            //avatar.GetComponent<PlayerInput>().enabled = false;
            x=x+1;
            await Task.Delay(50);
            
            // teleport the avatar to standing point
            //avatar.position = standingPoint.position;
            //avatar.rotation = standingPoint.rotation;

            // disable main cam, enable dialog cam
            //mainCamera.SetActive(false);
            toActivate.SetActive(true);

            // dısplay cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Recover()
    {
        print("enter npc dialog");
        //avatar.GetComponent<PlayerInput>().enabled = true;
        print("first line true");

        mainCamera.SetActive(true);
        toActivate.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
