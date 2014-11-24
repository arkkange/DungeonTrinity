using UnityEngine;
using System.Collections;

public class Coroutines : MonoBehaviour {

    void Start()
    {
        //StartCoroutine(TimerExemple(3));
    }

    /*********************************************************************\
    |   TimerExemple : exemple de coroutine                               |
    \*********************************************************************/
    IEnumerator TimerExemple(int seconds)
    {
        yield return new WaitForSeconds(seconds);

        //code something

        //yield return null;    ->  sert a attendre la frame suivant avant d'executerla suite du code
        //yield return new WaitForSeconds(3f);  -> sert a faire une pause dans le code de xf secondes
    }

}
