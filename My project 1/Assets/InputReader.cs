using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputReader : MonoBehaviour
{
	List<InputDevice> inputDevices = new List<InputDevice>();
    // Start is called before the first frame update
    void Start()
    {
        InitializeInputReader();
    }
	
	void InitializeInputReader()
	{
		InputDevices.GetDevices(inputDevices);
		InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, inputDevices);
		
		foreach (var inputDevice in inputDevices)
		{
			Debug.Log(inputDevice.name + "" + inputDevice.characteristics);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
