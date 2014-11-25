using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackResolver : MonoBehaviour {

    List<Transform> _TransformsHits = new List<Transform>();

    //[SerializeField]
    //Transform _myAttacks;   //to change the direction where the skill is casting

    [SerializeField]
    Transform _myCircle;

    [SerializeField]
    Transform _myCone;

    [SerializeField]
    Transform _myLine;

    [SerializeField]
    string _playersTag;

    [SerializeField]
    string _minionsTag;


    /******************************************************************************************\
    |   ResolveSkill : creation de la coroutine associée a la resolution du skill en question  |
    \******************************************************************************************/
    public void ResolveSkill(Skill mySkill ){

        switch (mySkill._type)
        {
            case 1: //circle
                StartCoroutine(resolveCircle(mySkill));
                break;

            case 2: //cone
                StartCoroutine(resolveCone(mySkill));
                break;

            case 3: //Line
                StartCoroutine(resolveLine(mySkill));
                break;

            case 4: //move
                
                break;

            case 5: //special action
                
                break;

            default:
                Debug.Log("skill type error");
                break;
        }

    }

    IEnumerator resolveCircle(Skill mySkill)
    {
        //Circle
        Vector3 myPosition = this.transform.position;

        //repositionement du cercle (sous forme de transformation vectorielle pour que soit detecté le deplacement dans le trigger)
        _myCircle.Translate( (mySkill._location - _myCircle.position)  , Space.World);  // TODO : peut etre une erreur de translate (a verifier)
        _myCircle.localScale = new Vector3(mySkill._scaleModifier, mySkill._scaleModifier, mySkill._scaleModifier);

        _myCircle.GetComponentInChildren<MeshRenderer>().enabled = true;

        //waiting for cast Time
        yield return new WaitForSeconds( mySkill._castTime );

        //get the collisions
        _TransformsHits = _myCircle.GetComponentInChildren<PlayerColliderScript>()._TransformListOfCollisions;


        //resolution concrete
        if (mySkill._affectMinions == false)
        {
            removeMinionsFromTransformsHitsList();
        }

        if (mySkill._affectPlayers == false)
        {
            removePlayersFromTransformsHitsList();
        }

        if (mySkill._isDamage)
        {
            //case damage
            //foreach (Transform T in _TransformsHits)
            //{
            //    T.GetComponent<HealthManager>().damage(mySkill._damageValue);
                
            //}
            for (int i = 0; i < _TransformsHits.Count; i++)
            {
                _TransformsHits[i].GetComponent<HealthManager>().damage(mySkill._damageValue);
            }
        }
        else
        {
            if (mySkill._isResurection)
            {
                //case resurection
                for (int i = 0; i < _TransformsHits.Count; i++)
                {
                    _TransformsHits[i].GetComponent<HealthManager>().revive(mySkill._healValue);
                }
            }
            else
            {
                //case heal
                for (int i = 0; i < _TransformsHits.Count; i++)
                {
                    _TransformsHits[i].GetComponent<HealthManager>().heal(mySkill._healValue);
                }
            }
        }

        //masque du visiuel
        _myCircle.GetComponentInChildren<MeshRenderer>().enabled = false;

    }

    IEnumerator resolveCone(Skill mySkill)
    {   
        //Cone
        // TODO : coder resolveCone


        //waiting for cast Time
        yield return new WaitForSeconds(mySkill._castTime);

    }

    IEnumerator resolveLine(Skill mySkill)
    {
        //Line
        // TODO : coder resolveLine


        //waiting for cast Time
        yield return new WaitForSeconds(mySkill._castTime);
    }


    /******************************************************************************************\
    |   removeMinionsFromTransformsHitsList                                                    |
    \******************************************************************************************/
    private void removeMinionsFromTransformsHitsList()
    {

        //foreach (Transform T in _TransformsHits)
        //{
        //    if (T.tag == _minionsTag)
        //    {
        //        _TransformsHits.Remove(T);
        //    }
        //}

        for(int i=0; i < (_TransformsHits.Count -1) ; i++){
            if (_TransformsHits[i].tag == _minionsTag)
            {
                _TransformsHits.RemoveAt(i);
            }
        }

    }


    /******************************************************************************************\
    |   removePlayersFromTransformsHitsList                                                    |
    \******************************************************************************************/
    private void removePlayersFromTransformsHitsList()
    {

        //foreach (Transform T in _TransformsHits)
        //{
        //    if (T.tag == _playersTag)
        //    {
        //        _TransformsHits.Remove(T);
        //    }
        //}

        for (int i = 0; i < (_TransformsHits.Count - 1); i++)
        {
            if (_TransformsHits[i].tag == _playersTag)
            {
                _TransformsHits.RemoveAt(i);
            }
        }

    }



}
