using System.Collections;
using TMPro;
using UnityEngine;

public class HitText : MonoBehaviour
{
    [SerializeField] private float floatSpeed;  // �ؽ�Ʈ�� �ö󰡴� �ӵ�
    [SerializeField] private float riseDuration = 1.0f; //�ؽ�Ʈ�� �ö󰡴� �� �ɸ��� �ð�
    [SerializeField] private float fadeDuration = 1.0f; //���������� �� �ɸ��� �ð�
    public Vector3 offset = new Vector3(0, 2, 0);  //�ؽ�Ʈ�� �ö󰡴� �Ÿ�

    public TextMeshPro damageText;
    private Color textColor;

	public void Initalize(int dmg)
	{
        damageText.text = dmg.ToString();
		textColor = damageText.color;
        StartCoroutine(MoveAndFade());
	}

    IEnumerator MoveAndFade()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + offset;

        float elapseTime = 0;
        while (elapseTime < riseDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapseTime / riseDuration);
            elapseTime += Time.deltaTime;
            yield return null;
        }

		elapseTime = 0;
		while (elapseTime < fadeDuration)
		{
            textColor.a = Mathf.Lerp(1, 0, elapseTime / fadeDuration);
            damageText.color = textColor;
			elapseTime += Time.deltaTime;
			yield return null;
		}
        Destroy(gameObject);

	}
}
