using System;
using UnityEngine;

namespace Project.Scripts.Menu
{
    [RequireComponent(typeof(Rigidbody))]
    public class CameraMovement : MonoBehaviour
    {
        private Rigidbody myRigidbody;
        [SerializeField] private float speed = 2;

        private void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            myRigidbody.velocity = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"))*  speed;
        }
    }
}
