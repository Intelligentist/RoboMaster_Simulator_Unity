using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]

    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }   


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
 
#if !MOBILE_INPUT
            float brake = CrossPlatformInputManager.GetAxis("Jump");
            bool accelflag = Input.GetKey(KeyCode.LeftShift);  
            m_Car.Move(h, v, v, brake);
            m_Car.Accelerate(accelflag);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
