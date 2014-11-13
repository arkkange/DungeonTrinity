using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TimeLine : MonoBehaviour {

    [SerializeField]
    int                     _maxTime = 10;
    int                     _actualTime = 0;

    List<Skill>             _skillList = new List<Skill>();
    List<RectTransform>     _portionsTimeLine   = new List<RectTransform>();

    [SerializeField]
    RectTransform           _timeLinePanel;
    float                   _actualAnchorX = 0;


    //bar panels

    [SerializeField]
    RectTransform           _panelPrefabForComp1;
    [SerializeField]
    RectTransform           _panelPrefabForComp2;
    [SerializeField]
    RectTransform           _panelPrefabForComp3;
    [SerializeField]
    RectTransform           _panelPrefabForComp4;
    [SerializeField]
    RectTransform           _panelPrefabForComp5;  

    //text numbers
    [SerializeField]
    RectTransform           _textNumber1;
    [SerializeField]
    RectTransform           _textNumber2;
    [SerializeField]
    RectTransform           _textNumber3;
    [SerializeField]
    RectTransform           _textNumber4;
    [SerializeField]
    RectTransform           _textNumber5;
    [SerializeField]
    RectTransform           _textNumber6;
    [SerializeField]
    RectTransform           _textNumber7;
    [SerializeField]
    RectTransform           _textNumber8;
    [SerializeField]
    RectTransform           _textNumber9;
    [SerializeField]
    RectTransform           _textNumber10;


   /***********************************************************\
   |   Start : Fonction d'initialisation                        |
   \***********************************************************/
   void Start () {
       Skill s = new Skill(1, new Vector3(0, 0, 0), Quaternion.identity, 3, 20);
       addSkill(s);
       s = new Skill(2, new Vector3(0, 0, 0), Quaternion.identity, 2, 20);
       addSkill(s);
       s = new Skill(3, new Vector3(0, 0, 0), Quaternion.identity, 3, 20);
       addSkill(s);


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

        if ( (newSkill._castTime + _actualTime) > _maxTime )
        {
            // TODO affichage "l'action que vous tentez d'ajouter ne peut pas entrer dans la timeline"
            Debug.Log((_actualTime + newSkill._castTime) + ">" + (_maxTime));

        }
        else
        {
            //add the skill to action list
            _skillList.Add(newSkill);

            RectTransform _newPortion;
            //create the skill as a image in the bar according to his color type
            switch (newSkill._type)
            {
                case 1:
                    _newPortion = Instantiate(_panelPrefabForComp1) as RectTransform;
                    break;

                case 2:
                    _newPortion = Instantiate(_panelPrefabForComp2) as RectTransform;
                    break;

                case 3:
                    _newPortion = Instantiate(_panelPrefabForComp3) as RectTransform;
                    break;

                case 4:
                    _newPortion = Instantiate(_panelPrefabForComp4) as RectTransform;
                    break;

                case 5:
                    _newPortion = Instantiate(_panelPrefabForComp5) as RectTransform;
                    break;

                default:
                    _newPortion = Instantiate(_panelPrefabForComp4) as RectTransform;
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
            _newPortion.anchorMax = new Vector2(_actualAnchorX + (newSkill._castTime / 10), 1);
            _actualAnchorX = _actualAnchorX + (newSkill._castTime / 10);

            _portionsTimeLine.Add(_newPortion);

            //mise a jour du temps de cast total
            _actualTime += (int)newSkill._castTime;
            stateNumber(_actualTime);

        }
        
    }
    /***********************************************************\
    |   removeLastSkill : supprimer le dernier skill ajouté     |
    \***********************************************************/
    public void removeLastSkill()
    {

        RectTransform   portionToDelete;

        //mise a jour du temps de cast total
        stateNumber(_actualTime);
        _actualTime -= (int)_skillList[_skillList.Count - 1]._castTime;

        //suppression de la liste des skills
        _skillList.RemoveAt(_skillList.Count - 1);

        //supression de l'object de la time line
        portionToDelete = _portionsTimeLine[_portionsTimeLine.Count - 1];
        Destroy(portionToDelete.gameObject);

        //supression de la liste des portions de la timeline
        _portionsTimeLine.RemoveAt(_portionsTimeLine.Count - 1);

    }


    /***********************************************************\
    |   stateNumber : active ou desactive le nombre en parametre|
    \***********************************************************/
    void stateNumber(int number)
    {
        switch (number)
        {
            case 1:
                 _textNumber1.active = (_textNumber1.active == true) ? false : true;
                    break;
            case 2:
                 _textNumber2.active = (_textNumber2.active == true) ? false : true;
                 break;
            case 3:
                 _textNumber3.active = (_textNumber3.active == true) ? false : true;
                 break;
            case 4:
                 _textNumber4.active = (_textNumber4.active == true) ? false : true;
                 break;
            case 5:
                 _textNumber5.active = (_textNumber5.active == true) ? false : true;
                 break;
            case 6:
                 _textNumber6.active = (_textNumber6.active == true) ? false : true;
              break;
            case 7:
                _textNumber7.active = (_textNumber7.active == true) ? false : true;
             break;
            case 8:
                _textNumber8.active = (_textNumber8.active == true) ? false : true;
                 break;
            case 9:
                 _textNumber9.active = (_textNumber9.active == true) ? false : true;
                break;
            case 10:
                _textNumber10.active = (_textNumber10.active == true) ? false : true;
                break;

            default:
                Debug.Log("an error as occured on the textnumber choice");
                break;
        }
    }

    // TODO ajouter le boutton d'annulation

}
