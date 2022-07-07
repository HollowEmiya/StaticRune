using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }
    public void ResetTrigger(string triggerName)
    {
        playerAnimator.ResetTrigger(triggerName);
    }

    public void ActiveCol()
    {
        playerAnimator.SetBool("collider",true);
    }

    public void DeActiveCol()
    {
        playerAnimator.SetBool("collider",false);
    }
}
