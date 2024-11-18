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

    protected void AnimatorChange(string animParam, bool trigger)
    {
        //animator.SetBool("IDLE", false);
		//animator.SetBool("MOVE", false);
		if (trigger)
        {
            animator.SetTrigger(animParam);
        }
        else
        {
            animator.SetBool(animParam, true);
        }
    }

    void Update()
    {
        
    }
}
