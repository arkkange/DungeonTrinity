using UnityEngine;
using System.Collections;

public class HitTest : MonoBehaviour {

    [SerializeField]
    Transform _sphere;

    [SerializeField]
    Transform _capsule;

    [SerializeField]
    Transform _attacker;

    Skill _myskill;

    void Start()
    {
        //skill de type circle faisant  100 degats affectant lesm minions
        _myskill = new Skill(1, new Vector3(0, 0, 0), Quaternion.identity, 1, 3, 20, 100, true, 0, false, true, true);
    }


	void Update () {

        // TODO : enlever la serie de tests une fois fonctionel 2

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Vector3 position = _sphere.transform.position;
            _myskill._location = position;
            _attacker.GetComponent<PlayerAttackResolver>().ResolveSkill(_myskill);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Vector3 position = _capsule.transform.position;
            _myskill._location = position;
            _attacker.GetComponent<PlayerAttackResolver>().ResolveSkill(_myskill);
        }

	}

}
