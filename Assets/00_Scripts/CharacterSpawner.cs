using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
	[SerializeField] private GameObject spawnPrefab;

	List<Vector2> spawnList = new List<Vector2>();
	List<bool> spawnListBool = new List<bool>();

    void Start()
	{
		GridStart();

	}

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

				spawnList.Add(new Vector2(xPos, yPos + transform.localPosition.y - yCount));
				spawnListBool.Add(false);
			}
		}
	}
	#endregion

	public void Summon()
	{
		int positionValue = -1;
		GameObject go = Instantiate(spawnPrefab);
        for (int i = 0; i < spawnListBool.Count; i++)
        {
			if (spawnListBool[i] == false)
			{
				positionValue = i;
				spawnListBool[i] = true;
				break;
			}
        }
		go.transform.position = spawnList[positionValue];
    }
}
