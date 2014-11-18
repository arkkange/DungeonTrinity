using UnityEngine;
using System.Collections;

public class HealthBarManager : MonoBehaviour {

    [SerializeField]
    RectTransform _myImage;

    [SerializeField]
    Transform _myHealthManager;

    /***********************************************************\
    |   Update : Fonction apellée une fois par frame            |
    \***********************************************************/
    void Update () {

        UpdateBar();

        // TODO : enlever la serie de tests une fois fonctionel

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _myHealthManager.GetComponent<HealthManager>().damage(100.0f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            _myHealthManager.GetComponent<HealthManager>().heal(100.0f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _myHealthManager.GetComponent<HealthManager>().revive(100.0f);
        }
       
	}


    void UpdateBar()
    {

            float lifepercentage =  (_myHealthManager.GetComponent<HealthManager>()._ActualLife / _myHealthManager.GetComponent<HealthManager>()._MaxLife);
            _myImage.anchorMax = new Vector2(lifepercentage, 1);

    }

}
