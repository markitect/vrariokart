using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]

    public class CarUserControl : MonoBehaviour
    {
        public CarController m_Car;
        public AutoCarGears CurrentGear;

        private float gas = 0f;
        private float steering = 0f;
        private float brake = 0f;
        private float clutch = 0f;

        public bool m_UseRacingWheel;
        public bool m_AutomaticTransmission;
        public bool m_ManualTransmission;

        public enum AutoCarGears
        {
            Reverse = 0,
            Drive = 1
        }

        public enum ManualCarGears
        {
            Reverse = 0,
            First = 1,
            Second = 2,
            Third = 3,
            Fourth = 4,
            Fith = 5,
            Six = 6
        }

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            CurrentGear = AutoCarGears.Drive;
        }


        private void Update()
        {
            // pass the input to the car!
            SteeringCar();
            if (m_UseRacingWheel)
            {
                LogitechControl();
                GearSwitching();
            }
            else
            {
                KeyboardControl();
            }

            m_Car.Move(steering, gas, brake, 0f);       
        }

        public void KeyboardControl()
        {
            gas = Input.GetAxis("Vertical");
            brake = Input.GetAxis("Vertical");
        }

        public void LogitechControl()
        {
            DriveDirection();
            Braking();
            Clutching();
        }

        public void SteeringCar()
        {
            steering = Input.GetAxis("Horizontal");
        }

        public void Braking()
        {
            brake = Input.GetAxis("Brake") * 0.5f;
            var normlizeBrake = brake + 0.5f;
            brake = normlizeBrake;
        }

        public void Clutching()
        {
            clutch = Input.GetAxis("Clutch") * 0.5f;
            var normlizeClutch = clutch + 0.5f;
            clutch = normlizeClutch;

            if (clutch > 0)
            {
                gas = 0f;
            }
        }

        public void DriveDirection()
        {
            if (CurrentGear == AutoCarGears.Drive)
            {
                gas = Input.GetAxis("Gas") * 0.5f;
                var normlizeGas = gas + 0.5f;
                gas = normlizeGas;
            }
            else if (CurrentGear == AutoCarGears.Reverse)
            {
                gas = Input.GetAxis("Gas") * -0.5f;
                var normlizeGas = gas - 0.5f;
                gas = normlizeGas;
            }
        }

        public void GearSwitching()
        {
            if (m_AutomaticTransmission)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    CurrentGear = AutoCarGears.Drive;
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    CurrentGear = AutoCarGears.Reverse;
                }
            }

        }

        void OnGUI()
        {
            GUI.TextArea
                (new Rect(400, 100, 250, 50), "steering : " + steering);
            GUI.TextArea
                (new Rect(400, 200, 250, 50), "gas : " + gas);
            GUI.TextArea
                (new Rect(400, 300, 250, 50), "brake : " + brake);
            GUI.TextArea
                (new Rect(400, 400, 250, 50), "clutch : " + clutch);
            GUI.TextArea
                (new Rect(400, 500, 250, 50), "Gear : " + CurrentGear);
        }
    }

}
