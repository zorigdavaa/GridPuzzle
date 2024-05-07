using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZPackage;

public abstract class Character : Mb, IHealth
{
    public MovementForgeRun movement;
    public LeaderBoardData data;
    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            healthBar.fillAmount = value * 0.01f;
        }
    }

    public AnimationController animationController;
    public Inventory inventory;

    int IHealth.Health { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Renderer rend;
    [SerializeField] Image healthBar;

    public virtual void TakeDamage(float amount)
    {
        Health += (int)amount;
        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // animationController.Die();
        gameObject.layer = 2;
        movement.Cancel();
        rb.isKinematic = true;
        healthBar.transform.parent.gameObject.SetActive(false);
    }
    public float speed = 5;
    public void MoveTo(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            // rb.velocity = dir * speed;
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
            rb.velocity += Vector3.down * 0.3f;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
    public void SetColor(Color col)
    {
        // transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.color = col;
        rend.material.color = col;
    }
    public Color GetColor()
    {
        // transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.color = col;
        return rend.material.color;
    }
}