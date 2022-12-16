using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;




public class PlayerScript : MonoBehaviour
{
    public WheelCollider[] wheels;
    public GameObject centerOfMass;
    public Rigidbody rigidbody;
    public GameObject car;
    public Text speedTxt;
    public float motorPower = 100;
    public float steerPower = 200;
    public float maxSpeed = 100f;
    private float _speed;
    private bool impulsActive;
    private bool canStop;
    private bool canDrive;
    private bool isMaxSpeed;

    public GameObject cam1;
    public GameObject cam2;

    private void OnTriggerEnter(Collider other)
    {
        if (cam1.active == true)
        {
            cam1.SetActive(false);
            cam2.SetActive(true);
        }
        if (cam2.active == true)
        {
            cam1.SetActive(true);
            cam2.SetActive(false);
        }
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }
    void Update()
    {

        _speed = rigidbody.velocity.magnitude;
        StopCheck();
        SpeedCheck();
        //DriveCheck();
        SpeedDown();

        MaxSpeedCheck();
        AutoStop();
    }

    private void FixedUpdate()
    {
        if (isMaxSpeed == true)
        {
            foreach (var wheel in wheels)
            {
                wheel.motorTorque = Input.GetAxis("Vertical") * ((motorPower * 5) / 4);
            }
        }
        for (int i = 0; i < wheels.Length; i++)
        {
            if (i < 2)
            {
                wheels[i].steerAngle = Input.GetAxis("Horizontal") * steerPower;
                wheels[i].transform.Rotate(0f, -3f, 0f);
            }
        }

        //ускорение на шифте
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ImpulsUse();
        }

        //метод дл€ торможени€ (обратный импульс)
        if (Input.GetKey(KeyCode.Space))
        {
            if (canStop == true)
            {
                BackImpuls();
            }
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            DownImpuls();
        }
    }

    //триггер авто тормозов дл€ проверки в методе AutoStop
    private void MaxSpeedCheck()
    {
        if (_speed < maxSpeed)
        {
            isMaxSpeed = true;
        }
        if (_speed > maxSpeed)
        {
            isMaxSpeed = false;
        }

    }

    //автоматически тормозит если скорость превышает максимум, удерживает на уровне макс. скорости
    private void AutoStop()
    {
        if (isMaxSpeed == false)
        {
            if (_speed > maxSpeed)
            {
                rigidbody.AddForce(-car.transform.forward * 100, ForceMode.Impulse);
            }
        }
    }

    //дл€ регулировани€ ручного тормоза
    private void StopCheck()
    {
        if (_speed > 1)
        {
            canStop = true;
        }
        if (_speed <= 1)
        {
            canStop = false;
        }
    }

    private void ImpulsUse()
    {
        if (_speed <= 20)
        {
            impulsActive = true;
        }
        if (_speed > 41)
        {
            impulsActive = false;
        }
        if (impulsActive == true)
        {
            Impuls();
        }

    }

    //»мпульс в сторону движени€, нужен дл€ ускорени€, работает единоразово по нажатию
    private void Impuls()
    {
        rigidbody.AddForce(car.transform.forward * 100, ForceMode.Impulse);
    }

    //импульс в обратную сторону от направлени€. Ќужен дл€ тормозов, работает от апдейта
    private void BackImpuls()
    {
        rigidbody.AddForce(-car.transform.forward * 300, ForceMode.Impulse);
    }

    //¬ывод значени€ скорости в UI
    public void SpeedCheck()
    {
        speedTxt.text = "Km/h: " + Mathf.Round(rigidbody.velocity.magnitude).ToString();
    }

    private void RayCastUsing()
    {

    }
    private void DownImpuls()
    {
        rigidbody.AddForce(car.transform.up * 10000, ForceMode.Impulse);
    }

    private void SpeedDown()
    {
        if (_speed != 0)
        {
            rigidbody.AddForce(-car.transform.forward * 10, ForceMode.Impulse);
        }
       
    }
}
