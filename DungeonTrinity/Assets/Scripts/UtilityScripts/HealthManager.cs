using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {


    public float _ActualLife;
    [SerializeField]
    public float _MaxLife;

    public bool _invincible = false;
    public bool _isDead = false;

    /***********************************************************\
    |   Start : Fonction d'initialisation                       |
    \***********************************************************/
    void Start()
    {
        _ActualLife = _MaxLife;
    }

    /***********************************************************\
    |   damage : diminue  l'actual life                         |
    \***********************************************************/
    public void damage(float value)
    {
        if (!_invincible)
        {
            float newLife = _ActualLife - value;
            if (newLife <= 0)
            {
                newLife = 0;
                _isDead = true;
            }
            if (newLife > _MaxLife)
            {
                newLife = _MaxLife;
            }
            _ActualLife = newLife;
        }
    }

    /***********************************************************\
    |   heal : augmente l'actual life si non mort               |
    \***********************************************************/
   public void heal(float value)
    {
       if(! _isDead)
       {
            float newLife = _ActualLife + value;

            if (newLife > _MaxLife)
            {
                newLife = _MaxLife;
            }
            _ActualLife = newLife;
        }

    }

   /***********************************************************\
   |   revive : augmente l'actual life  meme si mort           |
   \***********************************************************/
   public void revive(float value)
    {
       if(_isDead)
       {
            float newLife = _ActualLife + value;

            if (newLife > _MaxLife)
            {
                newLife = _MaxLife;
            }
            _ActualLife = newLife;
            _isDead = false;
        }

    }

    

}
