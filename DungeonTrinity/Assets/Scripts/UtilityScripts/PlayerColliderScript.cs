using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerColliderScript : MonoBehaviour {

    public List<Transform> _TransformListOfCollisions = new List<Transform>();

    /***********************************************************\
    |   OnTriggerEnter : recupère la liste des collisions       |
    \***********************************************************/
    void OnCollisionEnter(Collision collision)
    {
        if (!_TransformListOfCollisions.Contains(collision.gameObject.transform))
        {
            _TransformListOfCollisions.Add(collision.gameObject.transform);
            if (collision.gameObject.tag != "Player")
            {
                Debug.Log("enter : " + collision.gameObject.tag);
            }
            
         }
    }

    /***********************************************************\
    |   OnTriggerEnter : recupère la liste des collisions       |
    \***********************************************************/
    void OnCollisionExit(Collision collision)
    {
        _TransformListOfCollisions.Remove(collision.gameObject.transform);
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log("Exit : " + collision.gameObject.tag);
        }
    }

    /***********************************************************************\
    |   GetListOfCollisions : Donne la liste des objets dans le collider    |
    \***********************************************************************/
    List<Transform> GetListOfCollisions()
    {
        return _TransformListOfCollisions;
    }

}
