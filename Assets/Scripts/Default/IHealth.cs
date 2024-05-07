using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public Transform transform { get; }
    public GameObject gameObject { get; }
    public int Health { get; set; }
    public bool IsAlive => Health > 0;
    public void TakeDamage(float amount);
    public void Die();
}
