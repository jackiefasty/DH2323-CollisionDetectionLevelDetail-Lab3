using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryGunShoot : MonoBehaviour
{
    public int damagePerShot = 20;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;        // The time between each shot.
    public float range = 100f;                      // The distance the gun can fire.

    float timer;                                    // A timer to determine when to fire.
    Ray shootRay = new Ray();                       // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    ParticleSystem gunParticles;                    // Reference to the particle system.
    LineRenderer gunLine;                           // Reference to the line renderer.
    float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.


    void Awake()
    {
        // Set up the references.
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire...
        if (Input.GetMouseButton(1) && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            // ... shoot the gun.
            Shoot();
        }

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            // ... disable the effects.
            DisableEffects();
        }
    }


    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
    }


    void Shoot() //use raycast to determine where the ray ends
    {
        // Reset the timer.
        timer = 0f;

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.Play();

        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...

        // If it hits something, set the second position of the line renderer to the point the raycast hit, otherwise, 
        // set the second position of the line renderer to the maximal raycast range.


        //Perform the raycast against gameobjects on the shootable layer...
        LayerMask shootable_layerMask = LayerMask.GetMask("Default"); //if not working, try PostProcessing layer
        Debug.Log(shootable_layerMask);
        Debug.Log(shootHit);
        if (Physics.Raycast(shootRay.origin, shootRay.direction, out shootHit, Mathf.Infinity, shootable_layerMask)) //if there is a rock in the front (...i.e. +if it hits something)
        {
            //gunLine = LineRenderer.SetPosition(0, shootHit.point); //ray should be stopped by the rock
            gunLine.SetPosition(0, shootHit.point);
            //set the second position of the line renderer to the point the raycast hit
        }
        else
        {
            //gunLine = LineRenderer.SetPosition(0, Mathf.Infinity); //otherwise, it will reach to the maximal raycast range (set the second position of the line renderer to the maximal raycast range.)
            gunLine.SetPosition(0, Vector3.positiveInfinity);
        }
    }
}
