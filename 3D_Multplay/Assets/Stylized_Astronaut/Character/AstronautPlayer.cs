using UnityEngine;
using System.Collections;
using Photon.Pun;
using ExitGames.Client.Photon;

namespace AstronautPlayer
{

    public class AstronautPlayer : MonoBehaviourPun
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
            if (photonView.IsMine)
            {
                anim = gameObject.GetComponentInChildren<Animator>();
                rb = gameObject.GetComponent<Rigidbody>();

                isGround = false;
                isJump = false;
            }
        }

        private void FixedUpdate()
        {
            if (photonView.IsMine)
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
            if (photonView.IsMine)
            {
                if (collision.collider != null)
                {
                    if (isGround == false)
                    {
                        anim.SetTrigger("Land");
                        isGround = true;
                    }
                }
            }
        }
    }
}
