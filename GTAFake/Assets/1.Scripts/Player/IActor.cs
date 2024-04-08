
using UnityEngine;

public interface IActor
{
    public void TakeDmg(float dmg);
    public float GetHealth();
    public void OnDeath();
}
