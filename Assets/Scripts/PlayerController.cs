using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float impulseForce  = 170000.0f;
    public float impulseTorque = 3000.0f;
    public float kickForce = 500f;

    public GameObject hero;

    Animator animController;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        // get references to the animation controller of hero
        // character and player's corresponding rigid body
        animController = hero.GetComponent<Animator> ();
        rigidBody      = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // W/A/S/D input as a combined rotation and movement vector
        Vector3 input = new Vector3(0, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        // allow movement when input detected and not crouching
        if (input.magnitude > 0.001 && !animController.GetBool ("Crouch"))
        {
            // rotations are about y axis
            rigidBody.AddRelativeTorque(new Vector3(0, input.y * impulseTorque * Time.deltaTime, 0));
            // motion is forward/backward (about z axis)
            rigidBody.AddRelativeForce(new Vector3(0, 0, input.z * impulseForce * Time.deltaTime));

            animController.SetBool("Walk", true);
        }
        else
        {
            animController.SetBool("Walk", false);

            // crouching with 'C' key (only when not moving)
            if (Input.GetKey(KeyCode.C))
                animController.SetBool("Crouch", true);
            else
                animController.SetBool("Crouch", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int r = Random.Range(0, 3);
                Debug.Log(r);
                if (r == 0)
                    animController.SetTrigger("Kick1");
                else if (r == 1)
                    animController.SetTrigger("Kick2");
                else if(r == 2)
                    animController.SetTrigger("Kick3");

                RaycastHit hit;
                if (Physics.Raycast(hero.transform.position + Vector3.up / 2, hero.transform.forward, out hit, 2f))
                {
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddForce(transform.forward * kickForce);
                    }
                }

            }
            
        }


    }
}
