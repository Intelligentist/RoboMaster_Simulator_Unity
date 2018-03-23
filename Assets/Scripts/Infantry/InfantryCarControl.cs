using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Vehicles.Car
{


    public class InfantryCarControl : MonoBehaviour
    {
        [SerializeField]
        private CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;
        [SerializeField]
        private WheelCollider[] m_WheelColliders = new WheelCollider[4];
        [SerializeField]
        private GameObject[] m_WheelMeshes = new GameObject[4];
        [SerializeField]
        private Vector3 m_CentreOfMassOffset;
        [SerializeField]
        private float m_MaximumSteerAngle;
        [SerializeField]
        private float m_MaximumSteerSpeed = 2;
        [Range(0, 1)]
        [SerializeField]
        private float m_SteerHelper; // 0 is raw physics , 1 the car will grip in the direction it is facing
        [Range(0, 1)]
        [SerializeField]
        private float m_TractionControl; // 0 is no traction control, 1 is full interference
        [SerializeField]
        private float m_FullTorqueOverAllWheels = 50;
        private float Current_FullTorqueOverAllWheels;
        private float Accel_FullTorqueOverAllWheels;
        [SerializeField]
        private float m_ReverseTorque;
        [SerializeField]
        private float m_MaxHandbrakeTorque = 30;
        [SerializeField]
        private float m_Downforce = 5f;
        [SerializeField]
        private SpeedType m_SpeedType;
        [SerializeField]
        private float m_Topspeed = 20;
        private float Current_Topspeed;
        private float Accel_Topspeed;
        [SerializeField]
        private float m_SlipLimit;
        [SerializeField]
        private float m_BrakeTorque;


        public Transform SteerMove;
        public float timer;
        public float acceltime = 1f;
        private Quaternion[] m_WheelMeshLocalRotations;
        private Vector3 m_Prevpos, m_Pos;
        private float m_SteerAngle;
        private float m_SteerSpeed;
        public bool isGroundedflag;

        private float m_OldRotation;
        public float m_CurrentTorque;
        private Rigidbody m_Rigidbody;
        private const float k_ReversingThreshold = 0.01f;

        public bool Skidding { get; private set; }
        public float BrakeInput { get; private set; }
        public float CurrentSteerAngle { get { return m_SteerAngle; } }
        public float CurrentSpeed;
        public float MaxSpeed { get { return m_Topspeed; } }
        public float Revs { get; private set; }
        public float AccelInput { get; private set; }

        public InfantryLockFire infantrylockfire;

        // Use this for initialization
        private void Start()
        {
            m_WheelMeshLocalRotations = new Quaternion[4];
            for (int i = 0; i < 4; i++)
            {
                m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
            }
            m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;

            m_MaxHandbrakeTorque = float.MaxValue;

            m_Rigidbody = GetComponent<Rigidbody>();
            m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);
            Current_FullTorqueOverAllWheels = m_FullTorqueOverAllWheels;
            Accel_FullTorqueOverAllWheels = 1.5f * Current_FullTorqueOverAllWheels;
            Current_Topspeed = m_Topspeed;
            Accel_Topspeed = 1.5f * Current_Topspeed;

        }




        // simple function to add a curved bias towards 1 for a value in the 0-1 range
        private static float CurveFactor(float factor)
        {
            return 1 - (1 - factor) * (1 - factor);
        }


        // unclamped version of Lerp, to allow value to exceed the from-to range
        private static float ULerp(float from, float to, float value)
        {
            return (1.0f - value) * from + value * to;
        }








        public void Move(float steering, float accel, float footbrake, float handbrake)
        {

            for (int i = 0; i < 4; i++)
            {
                Quaternion quat;
                Vector3 position;
                m_WheelColliders[i].GetWorldPose(out position, out quat);
                m_WheelMeshes[i].transform.position = position;
                m_WheelMeshes[i].transform.rotation = quat;
            }



            //clamp input values
            steering = Mathf.Clamp(steering, -1, 1);
            AccelInput = accel = Mathf.Clamp(accel, 0, 1);
            BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
            handbrake = Mathf.Clamp(handbrake, 0, 1);

            //Set the steer on the front wheels.
            //Assuming that wheels 0 and 1 are the front wheels.
            m_SteerAngle = steering * m_MaximumSteerAngle;
            m_WheelColliders[0].steerAngle = m_SteerAngle;
            m_WheelColliders[1].steerAngle = m_SteerAngle;


            Steer(steering);
            ApplyDrive(accel, footbrake);
            CapSpeed();
            //Set the handbrake.
            //Assuming that wheels 2 and 3 are the rear wheels.
            if (handbrake > 0f)
            {
                var hbTorque = handbrake * m_MaxHandbrakeTorque;
                m_WheelColliders[0].brakeTorque = hbTorque;
                m_WheelColliders[1].brakeTorque = hbTorque;
                m_WheelColliders[2].brakeTorque = hbTorque;
                m_WheelColliders[3].brakeTorque = hbTorque;
            }



            AddDownForce();
            TractionControl();
            if (CurrentSpeed > 3 && accel == 0 && footbrake == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    var hbTorque = m_MaxHandbrakeTorque;
                    m_WheelColliders[0].brakeTorque = hbTorque;
                    m_WheelColliders[1].brakeTorque = hbTorque;
                    m_WheelColliders[2].brakeTorque = hbTorque;
                    m_WheelColliders[3].brakeTorque = hbTorque;
                }
            }

            CurrentSpeed = (m_Rigidbody.velocity -Vector3.left * m_SteerSpeed).magnitude * 2.23693629f;
        }


        private void CapSpeed()
        {
            float speed = m_Rigidbody.velocity.magnitude;
            switch (m_SpeedType)
            {
                case SpeedType.MPH:

                    speed *= 2.23693629f;
                    if (speed > m_Topspeed)
                        m_Rigidbody.velocity = (m_Topspeed / 2.23693629f) * m_Rigidbody.velocity.normalized;
                    break;

                case SpeedType.KPH:
                    speed *= 3.6f;
                    if (speed > m_Topspeed)
                        m_Rigidbody.velocity = (m_Topspeed / 3.6f) * m_Rigidbody.velocity.normalized;
                    break;
            }
        }


        private void ApplyDrive(float accel, float footbrake)
        {

            float thrustTorque;
            switch (m_CarDriveType)
            {
                case CarDriveType.FourWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 4f);
                    for (int i = 0; i < 4; i++)
                    {
                        m_WheelColliders[i].brakeTorque = 0f;
                        m_WheelColliders[i].motorTorque = thrustTorque;
                    }
                    break;

                case CarDriveType.FrontWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 2f);
                    m_WheelColliders[0].motorTorque = m_WheelColliders[1].motorTorque = thrustTorque;
                    break;

                case CarDriveType.RearWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 2f);
                    m_WheelColliders[2].motorTorque = m_WheelColliders[3].motorTorque = thrustTorque;
                    break;

            }
            for (int i = 0; i < 4; i++)
            {
                if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.velocity) < 50f)
                {
                    m_WheelColliders[i].brakeTorque = m_BrakeTorque * footbrake;
                }
                else if (footbrake > 0)
                {
                    m_WheelColliders[i].brakeTorque = 0f;
                    m_WheelColliders[i].motorTorque = -m_ReverseTorque * footbrake;
                }
            }
        }





        // this is used to add more grip in relation to speed
        private void AddDownForce()
        {
            m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up * m_Downforce *
                                                         m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
        }


        // crude traction control that reduces the power to wheel if the car is wheel spinning too much
        private void TractionControl()
        {
            WheelHit wheelHit;
            switch (m_CarDriveType)
            {
                case CarDriveType.FourWheelDrive:
                    // loop through all wheels
                    for (int i = 0; i < 4; i++)
                    {
                        m_WheelColliders[i].GetGroundHit(out wheelHit);
                        AdjustTorque(wheelHit.forwardSlip);
                    }
                    break;

                case CarDriveType.RearWheelDrive:
                    m_WheelColliders[2].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);

                    m_WheelColliders[3].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);
                    break;

                case CarDriveType.FrontWheelDrive:
                    m_WheelColliders[0].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);

                    m_WheelColliders[1].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);
                    break;
            }
        }


        private void AdjustTorque(float forwardSlip)
        {
            if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
            {
                m_CurrentTorque -= 10 * m_TractionControl;
            }
            else
            {
                m_CurrentTorque += 10 * m_TractionControl;
                if (m_CurrentTorque > m_FullTorqueOverAllWheels)
                {
                    m_CurrentTorque = m_FullTorqueOverAllWheels;
                }
            }
        }



        public void Steer(float steering)
        {
            timer += Time.deltaTime;
            if (timer > acceltime)
                timer = acceltime;
            if (steering == 0f)
                timer = 0;
            steering = timer / acceltime * steering;
            m_SteerSpeed = steering * m_MaximumSteerSpeed;
            //m_Rigidbody.velocity = m_Rigidbody.velocity +(transform.right)* m_SteerSpeed * Time.deltaTime;
            if (m_WheelColliders[0].isGrounded || m_WheelColliders[1].isGrounded || m_WheelColliders[2].isGrounded || m_WheelColliders[3].isGrounded)
            {
                isGroundedflag = true;
            }
            else
            {
                isGroundedflag = false;
            }
            if (isGroundedflag && !infantrylockfire.lockfireflag)
            {
                transform.Translate(-Vector3.left * m_SteerSpeed * Time.deltaTime, SteerMove);
            }          
        }

        public void Accelerate(bool accelflag)
        {
            if (accelflag)
            {
                m_FullTorqueOverAllWheels = Accel_FullTorqueOverAllWheels;
                m_Topspeed = Accel_Topspeed;
            }
            if (!accelflag)
            {
                m_FullTorqueOverAllWheels = Current_FullTorqueOverAllWheels;
                m_Topspeed = Current_Topspeed;
            }

        }


    }
}
