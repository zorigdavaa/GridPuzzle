using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator Animator;
    const string animWalk = "Walk";
    const string animIdle = "Idle";

    public string currentAnimation;
    string beforeAmimation = animIdle;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Idle();
    }

    [SerializeField] bool transitionToIdle = false;
    private void Update()
    {
        // Check if the animation is not transitioning to "Idle" and the current animation is almost finished
        if (transitionToIdle && IsAnimationAlmostFinished() && !Animator.IsInTransition(0))
        {
            // Set a flag to prevent repeated transitions
            transitionToIdle = false;

            Idle();
        }
    }

    public void Walk()
    {
        ChangeAnimation(animWalk);
    }
    public void Idle()
    {
        ChangeAnimation(animIdle);
    }
    //
    public void ChangeAnimation(string animStr)
    {
        // if (!GameController.UseMergeWrapped)
        // {
        //     return;
        // }
        if (currentAnimation != animStr)
        {
            //Animator.Rebind();

            Animator.CrossFade(animStr, 0f, 0);
            beforeAmimation = currentAnimation;
            currentAnimation = animStr;
            transitionToIdle = false;
        }
    }
    // Coroutine OnceCor;
    public void RunAnimationOnce(string animStr)
    {
        ChangeAnimation(animStr);
        transitionToIdle = true;
        // float waitTime = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        // waitTime += 0.2f;
        // print(waitTime);
        // print(Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        // print(Animator.runtimeAnimatorController.animationClips.);
        // OnceCor = StartCoroutine(WaitForAnimation(waitTime, () =>
        // {
        //     Idle();
        // }));
        // IEnumerator WaitForAnimation(float wait, Action afterAction)
        // {
        //     yield return new WaitForSeconds(wait);
        //     OnceCor = null;
        //     // Animation has completed
        //     afterAction();
        // }
    }
    public void StopAllAnimation()
    {
        Animator.speed = 0f;
    }
    public void ResumeAllAnimation()
    {
        Animator.speed = 1f;
    }

    Action WaitAction;
    Coroutine waitCor;
    public void WaitAndCall(Action action, float time = 1f)
    {
        WaitAction = action;
        if (waitCor != null)
        {
            StopCoroutine(waitCor);
        }
        waitCor = StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            yield return new WaitForSeconds(time);
            WaitAction?.Invoke();
            waitCor = null;
        }
    }

    string[] randomAnims = new string[]
    {

    };
    internal void DoRandomAnimation()
    {
        string anim = randomAnims[Random.Range(0, randomAnims.Length)];
        RunAnimationOnce(anim);
    }


    private bool IsAnimationAlmostFinished()
    {
        return Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f;
    }
}
