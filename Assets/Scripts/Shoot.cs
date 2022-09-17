using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Shoot : MonoBehaviour
{
    public Transform spawnPoint;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] public float shotDistance;
    [SerializeField] private enum GunType { Pistol, Auto, Shotgun };
    [SerializeField] private float rpm = 600f;
    [SerializeField] private float secondsBetweenShots;
    [SerializeField] private float nextShootTime;
    [SerializeField] private GunType gunType;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private float cameraShakeIntensity = 2f;
    [SerializeField] private float CameraShakeTimer = 0.1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem bulletParticleSystem;

    void Start()
    {
        secondsBetweenShots = 60 / rpm;
    }
    public void Shooting()
    {
        if (CanShoot())
        {
            Ray ray = new Ray(spawnPoint.position, spawnPoint.forward);
            RaycastHit hit;
            shotDistance = 20f;
            if (Physics.Raycast(ray, out hit, shotDistance, collisionMask))
            {
                shotDistance = hit.distance;
                Debug.Log(hit.transform.name);
                if (hit.collider.GetComponent<HealthandDamage>())
                {
                    hit.collider.GetComponent<HealthandDamage>().TakeDamage(damage);
                }

                
            }
            // else
            // {
            //     TrailRenderer trail = Instantiate(bulletTrail, spawnPoint.position, Quaternion.identity);

            //     StartCoroutine(SpawnTrail(trail, transform.forward * shotDistance));
            // }
            bulletParticleSystem.Play();
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 1);
            nextShootTime = Time.time + secondsBetweenShots;
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            muzzleFlash.Play();
            CameraShake.Instance.ShakeCamera(cameraShakeIntensity, CameraShakeTimer);
        }
    }
    bool CanShoot()
    {
        bool canShoot = true;
        if (Time.time < nextShootTime)
        {
            canShoot = false;
        }
        return canShoot;
    }
    public void Autoshoot()
    {
        if (gunType == GunType.Auto)
        {
            Shooting();
        }
    }
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= bulletSpeed * Time.deltaTime;

            yield return null;

        }
        Destroy(Trail.gameObject, Trail.time);
    }
}