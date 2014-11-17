using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

    public int _ActualLife;
    public int _MaxLife;

    public bool _invincible = false;
    public bool _isDead = false;

    void start()
    {
        _ActualLife = _MaxLife;
    }

    void damage(int value)
    {
        int newLife = _ActualLife - value;
        if (newLife < 0)
        {
            _ActualLife = 0;
            _isDead = true;
        }
        if (newLife > _MaxLife)
        {
            _ActualLife = _MaxLife;
        }
    }

}
