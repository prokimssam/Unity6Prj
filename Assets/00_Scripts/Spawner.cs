using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour
{
	[SerializeField] private GameObject heroSpawnPrefab;
	[SerializeField] private Monster monsterSpawnPrefab;

	public static List<Vector2> monsterSpawnList = new List<Vector2>();
	List<Vector2> heroSpawnList = new List<Vector2>();
	List<bool> heroSpawnBoolList = new List<bool>();

	void Start()
	{
		GridStart();

		for(int i = 0; i < transform.childCount; i++)
		{
			monsterSpawnList.Add(transform.GetChild(i).position);
		}

		StartCoroutine(SpawnMonsterCoroutine());
	}

	#region 몬스터 소환
	IEnumerator SpawnMonsterCoroutine()
	{
		while (true)
		{
			Monster monster = Instantiate(monsterSpawnPrefab, monsterSpawnList[0], Quaternion.identity);
			yield return new WaitForSeconds(0.5f);

			break;
		}
	}
	#endregion

	#region Make Grid
	private void GridStart()
	{
		SpriteRenderer parentSprite = GetComponent<SpriteRenderer>();
		float parentWidth = parentSprite.bounds.size.x;
		float parentHeight = parentSprite.bounds.size.y;

		float xCount = transform.localScale.x / 6;
		float yCount = transform.localScale.y / 3;
		for (int row = 0; row < 3; row++)
		{
			for (int col = 0; col < 6; col++)
			{
				float xPos = (-parentWidth / 2) + (col * xCount) + (xCount / 2);
				float yPos = (parentHeight / 2) - (row * yCount) + (yCount / 2);

				heroSpawnList.Add(new Vector2(xPos, yPos + transform.localPosition.y - yCount));
				heroSpawnBoolList.Add(false);
			}
		}
	}
	#endregion


	#region 캐릭터 소환
	public void Summon()
	{
		int positionValue = -1;
		GameObject go = Instantiate(heroSpawnPrefab);
		for (int i = 0; i < heroSpawnBoolList.Count; i++)
		{
			if (heroSpawnBoolList[i] == false)
			{
				positionValue = i;
				heroSpawnBoolList[i] = true;
				break;
			}
		}
		go.transform.position = heroSpawnList[positionValue];
	}
	#endregion
}
