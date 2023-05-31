using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ArcherWeapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject arrowPrefab;
    public GameObject buttonCanvas;
    public Arrow arrow;
    public Animator animator;
    public GameObject player;

    private float fireRate;
    private float nextFire = 0f;
    private Vector3 mousePos;
    private bool isShootingNormal = false;

    //Unlimted shots ability
    private bool isUnlimitedShooting = false;
    private float timerUnlimitedShooting = 3f;
    private float timerUnlimitedShootingCoolDown;
    //



    // Spreadshot ability
    private bool isMultiShooting = false;
    private float timerMultiShooting = 10.0f;
    private float timerMultiShootingSkillCoolDown;
    //
    private int plusAD = 50;
    private bool attacking;


    private void Update()
    {
        attacking= false;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ShootNormal();
        SpreadshotAbility();
        UnlimitedShotsAbility();
        Debug.Log(timerMultiShootingSkillCoolDown);
        Debug.Log(timerUnlimitedShootingCoolDown);
    }
    public void IncreaseDMG()
    {
        buttonCanvas.SetActive(false);
        arrow.IncreaseAD();
    }
    private void ShootNormal()
    {
        if (Input.GetButtonDown("Fire1") && !isShootingNormal)
        {
            isShootingNormal = true;
        }
        if (isShootingNormal && !isMultiShooting && !isUnlimitedShooting)
        {
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                AttackDirection();
                FindObjectOfType<AudioManager>().Play("Arrow");
                Debug.Log("DU skjuter normalt");
                fireRate = 0.6f;
                nextFire = Time.time + fireRate;
                CreateArrow(new Vector3(0, 0f, 0f));
            }
        }
    }
    private void AttackDirection()
    {
        if (!attacking)
        {

        animator.SetTrigger("AttackFront");
        attacking = true;
        }
        else
        {
            attacking= false;
            animator.SetTrigger("FrontIdle");
        }

    }
    private void SpreadshotAbility()
    {
        if (Input.GetKey(KeyCode.E) && !isMultiShooting && timerMultiShootingSkillCoolDown <= 0 && !isUnlimitedShooting)
        {
            isMultiShooting = true;
            isShootingNormal = false;
            isUnlimitedShooting = false;
        }
        if (isMultiShooting)
        {
            timerMultiShooting -= Time.deltaTime;
            if (timerMultiShooting <= 0)
            {
                isMultiShooting = false;
                timerMultiShootingSkillCoolDown = 30.0f;
                timerMultiShooting = 4.0f;
                return; // skipping code
            }
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                FindObjectOfType<AudioManager>().Play("MultiShot");
                Debug.Log("DU multiskjuter");
                fireRate = 1.0f;
                nextFire = Time.time + fireRate;
                CreateArrow(new Vector3(0, 0f, 20f));
                CreateArrow(new Vector3(0, 0f, 10f));
                CreateArrow(new Vector3(0, 0f, 0f));
                CreateArrow(new Vector3(0f, 0f, -10f));
                CreateArrow(new Vector3(0, 0f, -20f));
            }
        }
        else
        {
            if (timerMultiShootingSkillCoolDown <= 0) return; // skip since we don't want to have -1 timer. 
            timerMultiShootingSkillCoolDown -= Time.deltaTime;
            // Update UI or something...
        }

    }
    private void UnlimitedShotsAbility()
    {
        if (Input.GetKey(KeyCode.Q) && !isUnlimitedShooting && timerUnlimitedShootingCoolDown <= 0 && !isMultiShooting)
        {
            isUnlimitedShooting = true;
            isMultiShooting = false;
            isShootingNormal = false;
        }
        if (isUnlimitedShooting)
        {
            timerUnlimitedShooting -= Time.deltaTime;
            if (timerUnlimitedShooting <= 0)
            {
                isUnlimitedShooting = false;
                timerUnlimitedShootingCoolDown = 60.0f;
                timerUnlimitedShooting = 3.0f;
                return;
            }

            if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
            {
                FindObjectOfType<AudioManager>().Play("Arrow");
                Debug.Log("Du Unlimited skjuter");
                fireRate = 0.1f;
                nextFire = Time.time + fireRate;
                CreateArrow(new Vector3(0f, 0f, 0f));
            }
        }
        else
        {
            if (timerUnlimitedShootingCoolDown <= 0) return;
            timerUnlimitedShootingCoolDown -= Time.deltaTime;
        }
    }
    void CreateArrow(Vector3 offsetRotation)
    {
        var obj = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        obj.transform.Rotate(offsetRotation);
    }
}