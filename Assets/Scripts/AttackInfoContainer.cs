using UnityEngine;
using System.Collections;

public class AttackInfoContainer {

	private float damageNumber;
	private IEnumerable damageType;
	private bool didHit;

	public AttackInfoContainer(float damageNum, bool DidHit /*TODO: Add damageType IEnumerable*/)
	{
		damageNumber = damageNum;
		didHit = DidHit;
	}

	public float DamageNum
	{
		get{
			return damageNumber;
		}
	}

	public bool DidHit
	{
		get{
			return didHit;
		}
	}
}

