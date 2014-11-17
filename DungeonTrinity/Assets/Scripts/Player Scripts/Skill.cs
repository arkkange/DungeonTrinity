using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

    public int              _type;
    public Vector3          _location;
    public Quaternion       _rotation;
    public float            _castTime;
    public int              _powerValue;


    /***********************************************************\
    |       Constructeur par defaut                             |
    \***********************************************************/
    public Skill(int type, Vector3 location, Quaternion rotation, float castTime, int powerValue)
    {
        _type = type;
        _location = location;
        _rotation = rotation;
        _castTime = castTime;
        _powerValue = powerValue;
    }
    

}
