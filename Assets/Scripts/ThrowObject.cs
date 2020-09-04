using UnityEngine;
using System.Collections;

public class ThrowObject : MonoBehaviour
{
    private Player targetedByPlayer;
    public float throwForce = 10f;
    bool playerInRange = false;
    bool beingCarried = false;
    // public AudioClip[] soundToPlay;
    // private AudioSource audio;
    public int dmg;
    private bool touched = false;

    void Start()
    {
        // audio = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("hand"))
        {
            targetedByPlayer = other.gameObject.GetComponentInParent<Player>();
            // Debug.Log(targetedByPlayer);
        }
    }

    // private void OnTriggerExit(Collider other) {
    //     if (other.CompareTag("hand")) {
    //         Debug.Log("exit");
    //         targetedByPlayer = null;
    //     }  
    // }

    void Update()
    {
        if (targetedByPlayer != null)
        {
            float dist = Vector3.Distance(gameObject.transform.position, targetedByPlayer.transform.position);
            if (dist <= 2.5f)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }

            if (!beingCarried)
            {
                if (playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Yoink");
                    GetComponent<Rigidbody>().isKinematic = true;
                    transform.parent = targetedByPlayer.playerCamera.transform;
                    beingCarried = true;
                }
            }
            else
            {
                // if (touched)
                // {
                //     GetComponent<Rigidbody>().isKinematic = false;
                //     transform.parent = null;
                //     beingCarried = false;
                //     touched = false;
                // }

                if (Input.GetMouseButtonDown(0))
                {
                    var rigidBody = GetComponent<Rigidbody>();
                    rigidBody.isKinematic = false;
                    transform.parent = null;
                    beingCarried = false;
                    rigidBody.AddForce(targetedByPlayer.playerCamera.transform.forward * throwForce, ForceMode.Impulse);
                    // RandomAudio();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    transform.parent = null;
                    beingCarried = false;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Knioy");
                    GetComponent<Rigidbody>().isKinematic = false;
                    transform.parent = null;
                    beingCarried = false;
                }
            }
        }
    }

    // void RandomAudio()
    // {
    //     if (audio.isPlaying)
    //     {
    //         return;
    //     }
    //
    //     audio.clip = soundToPlay[Random.Range(0, soundToPlay.Length)];
    //     audio.Play();
    // }

    // void OnTriggerEnter()
    // {
    //     if (beingCarried)
    //     {
    //         touched = true;
    //     }
    // }
}