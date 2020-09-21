using System.Collections;
using UnityEngine;

public interface IShield{
	void SetVariables(Health health, GameObject shieldObj);
	IEnumerator Shield(Shield shield, GameObject shieldObj);
}