using UnityEngine;
using System.Collections;

/* 
 * Released under the creative commons attribution license.
 * Do whatever you like with the code, just give credit to Stuart Spence.
 * https://creativecommons.org/licenses/by/3.0/
 */ 

public class PortalGun : MonoBehaviour
{
    public float gunRange = 1000;
    public AudioClip shootSound, teleportSound;
    private Portal[] portals;
	private float currentCooldownA = 0.0f;
	private float currentCooldownB = 0.0f;
	private float cooldown = 3.0f;

    void Start()
    {
        portals = GameObject.FindObjectsOfType<Portal>();
        if (portals.Length != 2)
            Debug.LogError("You need to attach the Portal script to exactly two gameobjects in the world.");
    }

    void OnGUI()
    {
		Event e = Event.current;
		if (e.isKey) {
			if((e.keyCode == KeyCode.Q && currentCooldownA <= 0.0f) || (e.keyCode == KeyCode.E && currentCooldownB <= 0.0f)) {
				if(e.keyCode == KeyCode.Q) {
					currentCooldownA = cooldown;
				} else if (e.keyCode == KeyCode.E) {
					currentCooldownB = cooldown;
				}
				Vector3 position = Camera.main.transform.position;
	            RaycastHit rayCastHit = new RaycastHit();
	            if (Physics.Linecast(position, position + Camera.main.transform.forward * gunRange, out rayCastHit, 1))
	            {
					int index = e.keyCode == KeyCode.Q ? 0 : 1;
	                portals[index].MovePortal(rayCastHit);
	            }
	        }
		}
    }

	void Update() {
		if(currentCooldownA > 0.0f) {
			currentCooldownA -= 0.1f;
		}
		if(currentCooldownA < 0.0f) {
			currentCooldownA = 0.0f;
		}
		if(currentCooldownB > 0.0f) {
			currentCooldownB -= 0.1f;
		}
		if(currentCooldownB < 0.0f) {
			currentCooldownB = 0.0f;
		}
	}
}
