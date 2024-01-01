using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationAtRandomFrame : MonoBehaviour
{
    [SerializeField] private List<Animator> animators;
    private void Start()
    {
        foreach (var animator in animators)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(state.fullPathHash, 0, Random.Range(0f, 1f));
        }
    }
}
