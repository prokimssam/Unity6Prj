using System.Collections;
using UnityEngine;

public class Monster : Character
{
	int targetValue = 0;

	[SerializeField] private float monsterSpeed = 1;
	public override void Start()
	{
		base.Start();
	}

	private void Update()
	{
		Vector2 targetPos = Spawner.monsterSpawnList[targetValue];
		transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * monsterSpeed);
		if (Vector2.Distance(transform.position, targetPos) <= 0.1f)
		{
			targetValue++;
			renderer.flipX = targetValue > 2 ? true : false;

			targetValue %= 4;
		}
	}
}
