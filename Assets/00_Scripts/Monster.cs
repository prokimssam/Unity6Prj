using System.Collections;
using UnityEngine;

public class Monster : Character
{
	int targetValue = 0;
	public int HP = 50;

	[SerializeField] private float monsterSpeed = 1;
	bool isDead = false;

	public override void Start()
	{
		base.Start();
	}

	private void Update()
	{
		if (isDead) return;

		Vector2 targetPos = Spawner.monsterSpawnList[targetValue];
		transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * monsterSpeed);
		if (Vector2.Distance(transform.position, targetPos) <= 0.1f)
		{
			targetValue++;
			renderer.flipX = targetValue > 2 ? true : false;

			targetValue %= 4;
		}
	}

	public void GetDamage(int dmg)
	{
		if (isDead) return;

		HP -= dmg;
		if (HP <= 0)
		{
			isDead = true;
			gameObject.layer = LayerMask.NameToLayer("Default");
			StartCoroutine(DeadCoroutine());
			AnimatorChange("DEAD", true);
		}
	}

	IEnumerator DeadCoroutine()
	{
		float alpha = 1.0f;
		while(renderer.color.a > 0.0f)
		{
			alpha -= Time.deltaTime;
			renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);

			yield return null;
		}

		Destroy(gameObject);
	}
}
