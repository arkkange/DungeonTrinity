using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TimeLine : MonoBehaviour {

    int                     _maxTime;
    int                     _timeSpend;
    List<Skill>             _skillList = new List<Skill>();
    List<RectTransform>     _Panels    = new List<RectTransform>();

    [SerializeField]
    RectTransform           _timeLinePanel;
    float                   _actualAnchorX = 0;

    [SerializeField]
    RectTransform           _imgPrefab1;
    [SerializeField]
    RectTransform           _imgPrefab2;
    [SerializeField]
    RectTransform           _imgPrefab3;
    [SerializeField]
    RectTransform           _imgPrefab4;  

   /***********************************************************\
   |   Start : Fonction d'initialisation                        |
   \***********************************************************/
   void Start () {
       Skill s = new Skill(1, new Vector3(0, 0, 0), Quaternion.identity, 2, 20);
       addSkill(s);
       s = new Skill(2, new Vector3(0, 0, 0), Quaternion.identity, 3, 20);
       addSkill(s);
       s = new Skill(3, new Vector3(0, 0, 0), Quaternion.identity, 2, 20);
       addSkill(s);
       s = new Skill(4, new Vector3(0, 0, 0), Quaternion.identity, 3, 20);
       //addSkill(s);
	}

    /***********************************************************\
    |   Update : Fonction apellée une fois par frame             |
    \***********************************************************/
    void Update () {
	
	}


    /***********************************************************\
    |   addSkill : ajoute un skill a la liste                   |
    \***********************************************************/
    void addSkill(Skill newSkill)
    {
        // TODO ajouter les contraintes pour pouvoir ajouter un skill

        //add the skill to action list
        _skillList.Add( newSkill );

        RectTransform _newPortion;
        //create the skill as a image in the bar according to his color type
        switch (newSkill._type)
        {
            case 1:
                _newPortion = Instantiate(_imgPrefab1) as RectTransform;
                break;

            case 2:
                _newPortion = Instantiate(_imgPrefab2) as RectTransform;
                break;

            case 3:
                _newPortion = Instantiate(_imgPrefab3) as RectTransform;
                break;

            case 4:
                _newPortion = Instantiate(_imgPrefab4) as RectTransform;
                break;
            
            default:
                _newPortion = Instantiate(_imgPrefab4) as RectTransform;
                break;
        }

        //fill the image to the bar
        _newPortion.parent = _timeLinePanel;
        _newPortion.active = true;
        _newPortion.localScale = new Vector3(1, 1, 1);

        _newPortion.anchoredPosition = new Vector2(0.5f, 0.5f);

        //set the position
        _newPortion.localPosition = new Vector3(0, 0, 0);
        _newPortion.sizeDelta = new Vector2(0, 0);          //The normalized position in the parent RectTransform that the lower left corner is anchored to.
        _newPortion.offsetMax = new Vector2(0, 0);          //The offset of the upper right corner of the rectangle relative to the upper right anchor.
        _newPortion.offsetMin = new Vector2(0, 0);          //The size of this RectTransform relative to the distances between the anchors.

        //anchors positions according to the actual X anchor
        _newPortion.anchorMin = new Vector2(_actualAnchorX, 0);
        _newPortion.anchorMax = new Vector2(_actualAnchorX + (newSkill._castTime/10) , 1);
        _actualAnchorX = _actualAnchorX + (newSkill._castTime/10);
        
    }
    /***********************************************************\
    |   removeLastSkill : supprimer le dernier skill ajouté     |
    \***********************************************************/
    void removeLastSkill(Skill newSkill)
    {
        // TODO vérifier la supression correcte
        _skillList.RemoveAt(_skillList.Count - 1);
    }

    // TODO ajouter une fonction anulation à l'aide d'un boutton

}
