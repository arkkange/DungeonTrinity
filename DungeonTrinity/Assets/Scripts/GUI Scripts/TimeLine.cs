﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class TimeLine : MonoBehaviour {


    [SerializeField]
    Transform   MessageManager;

    [SerializeField]
    int                     _maxTime = 10;
    int                     _actualTime = 0;

    List<Skill>             _skillList = new List<Skill>();
    List<RectTransform>     _portionsTimeLine = new List<RectTransform>();
    List<RectTransform>     _textPortionTimeLine = new List<RectTransform>();

    [SerializeField]
    RectTransform           _timeLinePanel;
    float                   _actualAnchorX = 0;

    [SerializeField]
    GameObject              _cancelButton;


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
    RectTransform           _textPrefab;


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
   |   Start : Fonction d'initialisation                       |
   \***********************************************************/
   void Start () {

       //exemple of add a skill
       Skill s = new Skill(1, new Vector3(0, 0, 0), Quaternion.identity, 1, 3, 20, 100, true, 0, false, true, false);
       addSkill(s);

       s = new Skill(2, new Vector3(0, 0, 0), Quaternion.identity, 1, 2, 20, 100, true, 0, false, true, false);
       addSkill(s);

       s = new Skill(3, new Vector3(0, 0, 0), Quaternion.identity, 1, 3, 20, 100, true, 0, false, true, false);
       addSkill(s);

       s = new Skill(2, new Vector3(0, 0, 0), Quaternion.identity, 1, 1, 20, 100, true, 0, false, true, false);
       addSkill(s);

	}

    /***********************************************************\
    |   addSkill : ajoute un skill a la liste                   |
    \***********************************************************/
    void addSkill(Skill newSkill)
    {

        if ( (newSkill._castTime + _actualTime) > _maxTime )
        {
            MessageManager.GetComponent<MessageManager>().CreateShortMessage(2, "Vous ne pouvez pas ajouter cette compétence a votre TimeLine");
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

            _newPortion.localPosition = new Vector3(0, 0, 0);
            _newPortion.sizeDelta = new Vector2(0, 0);          //The normalized position in the parent RectTransform that the lower left corner is anchored to.
            _newPortion.offsetMax = new Vector2(0, 0);          //The offset of the upper right corner of the rectangle relative to the upper right anchor.
            _newPortion.offsetMin = new Vector2(0, 0);          //The size of this RectTransform relative to the distances between the anchors.

            //anchors positions according to the actual X anchor
            _newPortion.anchorMin = new Vector2(_actualAnchorX, 0);
            _newPortion.anchorMax = new Vector2(_actualAnchorX + (newSkill._castTime / 10), 1);
            

            _portionsTimeLine.Add(_newPortion);




            //creation of the text indicator
            RectTransform newText = Instantiate(_textPrefab) as RectTransform;

            //fill the image to the bar
            newText.parent = _timeLinePanel;
            newText.active = true;
            newText.localScale = new Vector3(1, 1, 1);

            newText.anchoredPosition = new Vector2(0.5f, 0.5f);
            newText.localPosition = new Vector3(5, 5, 0);
            newText.sizeDelta = new Vector2(5, 5);
            newText.offsetMax = new Vector2(0, 0);
            newText.offsetMin = new Vector2(0, 0);

            //anchors positions according to the actual X anchor
            newText.anchorMin = new Vector2(_actualAnchorX, 0);
            newText.anchorMax = new Vector2(_actualAnchorX + (newSkill._castTime / 10), 1);
            
            //maj anchors
            _actualAnchorX = _actualAnchorX + (newSkill._castTime / 10);

            //fill the correct text then add to the list
            newText.GetComponent<Text>().text = (string)((int)(_actualAnchorX*10) + "");
            _textPortionTimeLine.Add(newText);

            //mise a jour du temps de cast total
            _actualTime += (int)newSkill._castTime;

            _cancelButton.SetActive(true);
        }
        
    }
    /***********************************************************\
    |   removeLastSkill : supprimer le dernier skill ajouté     |
    \***********************************************************/
    public void removeLastSkill()
    {

        RectTransform   portionToDelete;
        RectTransform   TextToDelete;

        //mise a jour du temps de cast total
        _actualTime -= (int)_skillList[_skillList.Count - 1]._castTime;

        //suppression de la liste des skills
        _skillList.RemoveAt(_skillList.Count - 1);

        //supression de l'object de la timeLine
        portionToDelete = _portionsTimeLine[_portionsTimeLine.Count - 1];
        Destroy(portionToDelete.gameObject);
        _portionsTimeLine.RemoveAt(_portionsTimeLine.Count - 1);

        //supression du chiffre de la portion
        TextToDelete = _textPortionTimeLine[_textPortionTimeLine.Count - 1];
        Destroy(TextToDelete.gameObject);
        _textPortionTimeLine.RemoveAt(_textPortionTimeLine.Count - 1);

        //cas ou la liste est vide
        if (_skillList.Count == 0)
        {
            _cancelButton.SetActive(false);
        }

    }

}
