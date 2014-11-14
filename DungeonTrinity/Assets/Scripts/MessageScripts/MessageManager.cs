using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {


    [SerializeField]
    RectTransform _myCanevas;

	void Start () {
        
	}

    public void CreateShortMessage(float time, string message)
    {
        StartCoroutine(TimerMessage(time, message));
    }
    IEnumerator TimerMessage(float seconds, string message)
    {
        RectTransform myMessageInterface = Instantiate(_myCanevas) as RectTransform;

        Text myText = myMessageInterface.GetComponentInChildren<Text>();
        myText.text = message;

        yield return new WaitForSeconds(seconds);

        Destroy(myMessageInterface.gameObject);

    }


}
