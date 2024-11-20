using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : Character
{
	public float attackRange = 1.0f;
	public float attackSpeed = 1.0f;
	public Monster target;
	public LayerMask enemyLayer;

	private void Update()
	{
		CheckForEnemies();
	}

	void CheckForEnemies()
	{
		attackSpeed += Time.deltaTime;
		Collider2D[] enemiesRange = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
		if (enemiesRange.Length > 0)
		{
			target = enemiesRange[0].transform.GetComponent<Monster>();
			if (attackSpeed > 1.0f)
			{
				attackSpeed = 0.0f;
				AttackEnemy(target);
			}
		}
		else
		{
			target = null;
		}
	}

	void AttackEnemy(Monster monster)
	{
		AnimatorChange("ATTACK", true);
		monster.GetDamage(10);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
