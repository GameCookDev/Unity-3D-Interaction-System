using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private bool debugPlayedAnimationName = false;

    private void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        animator = GetComponent<Animator>();
    }

    public void Play(string animationName, bool rootMotion = false, float animationFadeDuration = 0.1f, float newMoveSpeed = 2f, float newRotationSpeed = 2f)
    {
        if (debugPlayedAnimationName) Debug.Log(animationName);

        animator.applyRootMotion = rootMotion;

        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.Play(animationName, i);
        }

        animator.CrossFade(animationName, animationFadeDuration);
    }

    public void SetAnimatorFloat(string variableName, float value, float dampTime) => animator.SetFloat(variableName, value, 0.15f, Time.deltaTime);

    public void SetAnimatorBool(string variableName, bool value) => animator.SetBool(variableName, value);

    public void SetAnimatorTrigger(string variableName) => animator.SetTrigger(variableName);
}
