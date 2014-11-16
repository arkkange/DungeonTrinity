using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerColliderScript : MonoBehaviour {

    List<Transform>     _TransformListOfCollisions = new List<Transform>();

    /***********************************************************\
    |   OnTriggerEnter : recupère la liste des collisions       |
    \***********************************************************/
    void OnTriggerEnter(Collider other)
    {
        _TransformListOfCollisions.Add(other.transform);
    }

    /***********************************************************\
    |   OnTriggerEnter : recupère la liste des collisions       |
    \***********************************************************/
    void OnTriggerExit(Collider other)
    {
        _TransformListOfCollisions.Remove(other.transform);
    }

    /***********************************************************************\
    |   GetListOfCollisions : Donne la liste des objets dans le collider    |
    \***********************************************************************/
    List<Transform> GetListOfCollisions()
    {
        return _TransformListOfCollisions;
    }

}
