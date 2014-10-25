using UnityEngine;
using System.Collections;

/* 
 * Released under the creative commons attribution license.
 * Do whatever you like with the code, just give credit to Stuart Spence.
 * https://creativecommons.org/licenses/by/3.0/
 */

public class Portal : MonoBehaviour
{
    private Vector3 portalNormal;
    private Portal otherPortal;
    private GameObject player;
    private CharacterMotor cm;
    private CharacterController cc;

    private AudioSource audioSource;
    private AudioClip shootSound, teleportSound;

    public bool hasMoved = false;

    //this portal is temporarily disabled when there is time left on the disableTimer - when it is greater than zero.
    float disableTimer = 0;

    void Start()
    {
        cm = GameObject.FindObjectOfType<CharacterMotor>();
        cc = GameObject.FindObjectOfType<CharacterController>();
        player = cc.gameObject;

        SetupAudio();
        SetOtherPortal();
    }

    private void SetupAudio()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        PortalGun pg = GameObject.FindObjectOfType<PortalGun>();
        audioSource.playOnAwake = false;
        shootSound = pg.shootSound;
        teleportSound = pg.teleportSound;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == player && disableTimer <= 0 && hasMoved && otherPortal.hasMoved)
            otherPortal.MovePlayerToThisPortal();
    }

    void Update()
    {
        if (disableTimer > 0)
            disableTimer -= Time.deltaTime;
    }

    private void SetOtherPortal()
    {
        //find the portal that is not this one.
        if (otherPortal == null)
            foreach (Portal portal in FindObjectsOfType<Portal>())
                if (portal != this)
                    otherPortal = portal;
    }

    public void MovePlayerToThisPortal()
    {
        disableTimer = 1;

        //change the direction of the player's velocity to be the same as the portalNormal direction
        Vector3 exitVelocity = portalNormal * cc.velocity.magnitude;
        cm.SetVelocity(exitVelocity);
        //set the player position to just in front of the portal.
        Vector3 exitPosition = transform.position + otherPortal.portalNormal * 2;
        player.transform.position = exitPosition;
        //disable player movement while in the air. This is turned back on when the player hits the ground.
        //cm.isInputEnabled = false;
        //play the teleport sound
        audioSource.clip = teleportSound;
        audioSource.Play();

    }

    public void MovePortal(RaycastHit raycastHit)
    {
        hasMoved = true;
        transform.position = raycastHit.point;
        portalNormal = raycastHit.normal;

        //play the portal shoot sound
        audioSource.clip = shootSound;
        audioSource.Play();
    }
}
