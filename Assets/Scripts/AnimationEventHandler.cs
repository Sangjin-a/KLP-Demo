using System;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public Action onAnimationComplete { get; internal set; }
    public void AnimationComplete()
    {
        onAnimationComplete?.Invoke();
    }
}