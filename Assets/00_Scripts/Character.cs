using UnityEngine;

public class Character : MonoBehaviour
{
    protected Animator animator;
	protected SpriteRenderer renderer;

    public virtual void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
		renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }
}
