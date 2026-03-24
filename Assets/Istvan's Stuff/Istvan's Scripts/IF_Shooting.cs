using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class IF_Shooting : MonoBehaviour
{
    [SerializeField] float shootingInterval = 1f;

    private bool cooldownCanShoot = true;

    [SerializeField] GameObject projectile;
    [SerializeField] GameObject shootingParticleEffect;
    [SerializeField] Transform shootingPoint;
    private IF_ThirdPersonController tpsController;

    [SerializeField] float shootForce;

    [Header("Input Actions")]
    public InputActionReference shootAction;

    void Start()
    {
        tpsController = GetComponent<IF_ThirdPersonController>();
    }

    void Update()
    {
        if (shootAction.action.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(CanShoot() == false)
        {
            return;
        }

        GameObject shootingProjectile = Instantiate(projectile, shootingPoint.position, shootingPoint.rotation);

        if (shootingParticleEffect != null)
        {
            Instantiate(shootingParticleEffect, shootingPoint.position, shootingPoint.rotation);
        }

        Rigidbody projectileRigidbody = shootingProjectile.GetComponent<Rigidbody>();

        if (tpsController.targetObject != null)
        {
            projectileRigidbody.AddForce((tpsController.targetObject.position - shootingPoint.position).normalized * shootForce, ForceMode.Impulse);
        }
        else
        {
            projectileRigidbody.AddForce(shootingPoint.forward * shootForce, ForceMode.Impulse);
        }

        IF_GameManager.Instance.Ammunition--;

        StartCoroutine(ShootingCooldown());
    }

    private bool CanShoot()
    {
        if(cooldownCanShoot == false)
        {
            return false;
        }

        if(IF_GameManager.Instance.Ammunition > 0)
        {
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        shootAction.action.Enable();
    }

    private void OnDisable()
    {
        shootAction.action.Disable();
    }

    IEnumerator ShootingCooldown()
    {
        cooldownCanShoot = false;
        yield return new WaitForSeconds(shootingInterval);
        cooldownCanShoot = true;
    }
}
