using UnityEngine;
using System.Collections;

namespace AstronautPlayer
{

    public class AstronautPlayer : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody rb;

        private bool isGround;
        private bool isJump;

        public float input_Vertical;
        public float input_Horizontal;
        public float speed = 10.0f;
        public float turnSpeed = 10.0f;
        public float jumpForce = 10.0f;
        public float gravity = 20.0f;

        void Start()
        {
            anim = gameObject.GetComponentInChildren<Animator>();
            rb = gameObject.GetComponent<Rigidbody>();

            isGround = false;
            isJump = false;
        }

        void Update()
        {
            Move();
            Turn();

            if (Input.GetKeyUp(KeyCode.Space) && isGround)
            {
                isJump = true;
            }

            if (isJump)
            {
                isJump = false;

                Jump();
            }

            anim.SetBool("IsGround", isGround);
        }

        private void Move()
        {
            input_Vertical = Input.GetAxis("Vertical");
            anim.SetFloat("Input", input_Vertical);

            Vector3 movement = transform.forward * input_Vertical;
            rb.AddForce(movement * speed);
        }

        private void Turn()
        {
            input_Horizontal = Input.GetAxis("Horizontal");

            transform.Rotate(0, input_Horizontal * turnSpeed * Time.deltaTime, 0);
        }

        private void Jump()
        {
            isGround = false;
            anim.SetTrigger("Jump");

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("ºÎµúÈû °¨Áö");

            if (collision.collider != null)
            {
                if (isGround == false)
                {
                    anim.SetTrigger("Land");
                    isGround = true;
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider != null)
            {
                if (isJump == false)
                {
                    anim.SetTrigger("Fall");
                    isGround = false;
                }
            }
        }
    }
}
