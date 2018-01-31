using System;
using System.Runtime.InteropServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]

    public class CarUserControl : MonoBehaviour
    {
        public Text m_SpeedData;
        public Text m_GearData;
        public Text m_DebugData;
        public CarController m_Car;
        public AutoCarGears m_AutoGear;
        public ManualCarGears m_ManGear;
        public string m_CurrentGear; 

        private float gas = 0f;
        private float steering = 0f;
        private float brake = 0f;
        private float clutch = 0f;
        private float handBrake = 0f;
        private float cacheTopSpeed;

        public bool m_UseRacingWheel;
        public bool m_UseKeyboard;
        public bool m_UseXboxController;
        public bool m_AutomaticTransmission;
        public bool m_ManualTransmission;
        private float m_Topspeed;
        private float gearSpeed;


        public enum AutoCarGears
        {
            Reverse = 0,
            Drive = 1
        }

        public enum ManualCarGears
        {
            Reverse = 0,
            Neutral = 1,
            First = 2,
            Second = 3,
            Third = 4,
            Fourth = 5,
            Fifth = 6,
            Six = 7
        }

        private void Start()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            
            if (m_AutomaticTransmission)
            {
                m_AutoGear = AutoCarGears.Drive;
            }
            else if (m_ManualTransmission)
            {
                m_ManGear = ManualCarGears.First;
            }

            cacheTopSpeed = m_Car.m_Topspeed;
            gearSpeed = 0f;

            SetGearSpeeds();
            ConfigureCameraHeight(true);
        }


        private void Update()
        {
            // pass the input to the car!
            if (m_UseRacingWheel)
            {
                LogitechControl();
            }
            if(m_UseKeyboard)
            {
                KeyboardControl();
            }
            if (m_UseXboxController)
            {
                XboxController();
            }

            m_Car.Move(steering, gas, brake, handBrake);

            DashBoardDataControl();
        }

        public void ConfigureCameraHeight(bool enabled)
        {
            if (enabled)
            {
                XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
                InputTracking.disablePositionalTracking = true;
            }
        }
        public void KeyboardControl()
        {
            SteeringCar();
            gas = Input.GetAxis("Vertical");

            if (gas > 0)
            {
                m_CurrentGear = "Drive";
            }
            else if (gas < 0)
            {
                m_CurrentGear = "Reverse";
            }

            handBrake = Input.GetAxis("Jump");
        }

        public void XboxController()
        {
            steering = Input.GetAxis("Horizontal");
            gas = Input.GetAxis("XboxGas");
            brake = Input.GetAxis("XboxBrake");

            if (gas > 0)
            {
                m_CurrentGear = "Drive";
            }
            else if (gas < 0)
            {
                m_CurrentGear = "Reverse";
            }
        }

        public void LogitechControl()
        {
            SteeringCar();
            DriveDirection();
            Braking();
            Clutching();
            GearSwitching();        
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

            handBrake = Input.GetAxis("Jump");
        }

        public void Clutching()
        {
            clutch = Input.GetAxis("Clutch") * 0.5f;
            var normlizeClutch = clutch + 0.5f;
            clutch = normlizeClutch;

            if (clutch > 0)
            {
                if (gas > 0)
                {
                    gas = gas - clutch;
                }
                else if (gas < 0)
                {
                    gas = gas + clutch;
                }
            }
        }

        public void DriveDirection()
        {
            if (m_AutomaticTransmission)
            {
                if (m_AutoGear == AutoCarGears.Drive)
                {
                    gas = Input.GetAxis("Gas") * 0.5f;
                    var normlizeGas = gas + 0.5f;
                    gas = normlizeGas;
                }
                else if (m_AutoGear == AutoCarGears.Reverse)
                {
                    gas = Input.GetAxis("Gas") * -0.5f;
                    var normlizeGas = gas - 0.5f;
                    gas = normlizeGas;
                }
            }

            if (m_ManualTransmission)
            {
                if (m_ManGear == ManualCarGears.Neutral)
                {
                    gas = 0f;
                }

                if (m_ManGear == ManualCarGears.Reverse)
                {
                    gas = Input.GetAxis("Gas") * -0.5f;
                    var normlizeGas = gas - 0.5f;
                    gas = normlizeGas;
                }

                if (m_ManGear >= ManualCarGears.First)
                {
                    gas = Input.GetAxis("Gas") * 0.5f;
                    var normlizeGas = gas + 0.5f;
                    gas = normlizeGas;
                }
            }
        }

        public void GearSwitching()
        {
            if (m_UseRacingWheel)
            {
                if (m_AutomaticTransmission)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        m_AutoGear = AutoCarGears.Drive;
                    }

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        m_AutoGear = AutoCarGears.Reverse;
                    }

                    m_CurrentGear = m_AutoGear.ToString();
                }

                if (m_ManualTransmission)
                {
                    if (clutch > 0f)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            if (m_ManGear == ManualCarGears.Six)
                            {
                                m_ManGear = ManualCarGears.Six;
                            }
                            else
                            {
                                m_ManGear += 1;
                            }
                            SetGearSpeeds();
                        }
                        if (Input.GetKeyDown(KeyCode.Q))
                        {
                            if (m_ManGear == ManualCarGears.Reverse)
                            {
                                m_ManGear = ManualCarGears.Reverse;
                            }
                            else
                            {
                                m_ManGear -= 1;
                            }
                            SetGearSpeeds();
                        }
                    }

                    m_CurrentGear = m_ManGear.ToString();
                }
            }
        }

        public void SetGearSpeeds()
        {
            if (m_ManualTransmission)
            {
                switch (m_ManGear)
                {
                    case ManualCarGears.Reverse:
                        gearSpeed = 10f;
                        break;

                    case ManualCarGears.Neutral:
                        gearSpeed = 0f;
                        break;

                    case ManualCarGears.First:
                        gearSpeed = cacheTopSpeed / 3f;
                        break;

                    case ManualCarGears.Second:
                        gearSpeed = cacheTopSpeed / 2.5f;
                        break;

                    case ManualCarGears.Third:
                        gearSpeed = cacheTopSpeed / 2f;
                        break;

                    case ManualCarGears.Fourth:
                        gearSpeed = cacheTopSpeed / 1.5f;
                        break;

                    case ManualCarGears.Fifth:
                        gearSpeed = cacheTopSpeed / 1.2f;
                        break;

                    case ManualCarGears.Six:
                        gearSpeed = cacheTopSpeed;
                        break;
                }
                m_Car.m_Topspeed = gearSpeed;
            }
        }

        public void DashBoardDataControl()
        {
            m_Topspeed = m_Car.m_Topspeed;
            var speed = m_Car.CurrentSpeed;
            var line = Environment.NewLine;

            if (speed <= 1f)
            {
                speed = 0f;
            }

            m_SpeedData.text = "Speed Data" +
                               line + "MPH - " + speed;

            m_GearData.text = "Gear Data" +
                              line + "Gear - " + m_CurrentGear;

            m_DebugData.text = "Debug Data" +
                               line + "gas - " + gas +
                               line + "brake - " + brake +
                               line + "clutch - " + clutch +
                               line + "steering - " + steering +
                               line + "hand brake - " + handBrake + 
                               line + "Top Speed - " + m_Topspeed;
        }
    }

}
