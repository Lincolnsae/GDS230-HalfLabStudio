﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    
    public GameObject[] upgradeButtons;
    public Text[] upgradeText;

    public CircleCollider2D cC2D;
    public int rangeIncreaseCost = 200;

    public GameController gC;

    //bool to check if the enemy target is set currently if not resets the target
    public bool enemySet;
    public GameObject towerSpawnPoint;

    public GameObject projectile;
    public GameObject firePoint;
    public float fireTimer = 3f;

    //the transform of the enemy within range
    public GameObject enemy;

    public float damage = 2.5f;
    public int damageIncreaseCost = 100;

    public int sellTurret = 50;

    public Animator anim;

    //the speed that the turret turns towards the enemy in range
    public float turnSpeed = 10f;

    


    // Start is called before the first frame update
    void Start()
    {
        
        gC = FindObjectOfType<GameController>();
      
        

    }

    void Update()
    {
        
        

    }



    


    //only deal damage to enemy objects if they are your target
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //if enemy is not yet set, set it to the one that is currently inside your collider
            if (enemy == null)
            {
                enemy = collision.gameObject;
                
            }


            Turn();
            Fire();

        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if enemy leaves your range stop targeting them
        if (collision.gameObject == enemy)
        {
            enemy = null;
        }
    }

    //follow the enemy target
    void Turn()
    {
        Vector2 direction = enemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }

    void Fire()
    {
        fireTimer -= 0.8f * Time.deltaTime;

        if (fireTimer <= 2f)
        {
            anim.SetBool("Fire", true);
        }
        if (fireTimer <= 0.25f)
        {
            Instantiate(projectile, firePoint.transform.position, transform.rotation);
            fireTimer = 3f;
        }
        if (fireTimer >= 2f)
        {
            anim.SetBool("Fire", false);
        }
        
      
        
        
    }
    
    public void CloseUpgradeWindow() 
    {
        upgradeButtons[0].SetActive(false);
        upgradeButtons[1].SetActive(false);
        upgradeButtons[2].SetActive(false);
        upgradeButtons[3].SetActive(false);
        upgradeButtons[4].SetActive(false);
        gC.upgradeWindow = false;
    }

    public void IncreaseDamage() 
    { 
            damage += 0.25f;
            gC.cashMoney -= damageIncreaseCost;
            damageIncreaseCost += 100;
            sellTurret += 5;
        
    }

    public void IncreaseRange() 
    {
        
            cC2D.radius += 0.05f;
            gC.cashMoney -= rangeIncreaseCost;
            rangeIncreaseCost += 200;
            sellTurret += 10;
        
    }

    public void DestroyTower() 
    {
        Instantiate(towerSpawnPoint, new Vector3(transform.position.x, transform.position.y, 40), Quaternion.identity);
        gC.cashMoney += sellTurret;
        Destroy(upgradeButtons[0]);
        Destroy(upgradeButtons[1]);
        Destroy(upgradeButtons[2]);
        Destroy(upgradeButtons[3]);
        gC.upgradeWindow = false;
        Destroy(gameObject);
    }

    

}
