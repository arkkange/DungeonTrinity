using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameObjectEvents : MonoBehaviour
{

    [SerializeField]
    Image _myImage;

    [SerializeField]
    Sprite _TextureClosed;

    [SerializeField]
    Sprite _TextureOpened;


    /*********************************************************************\
    |   SwitchActive : Switch l'active du gameobject entre true et false  |
    \*********************************************************************/
    public void SwitchActive()
    {
       this.gameObject.active = this.gameObject.active ? false : true;
    }

    /*********************************************************************\
    |   SwitchActive : Switch la texture programmée de mon image          |
    \*********************************************************************/
    public void SwitchImage()
    {
        _myImage = GetComponent<Image>();
        _myImage.sprite = _myImage.sprite.Equals(_TextureClosed) ? _TextureOpened : _TextureClosed;
    }

}
