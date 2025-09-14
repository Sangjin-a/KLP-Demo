using UnityEngine;
using UnityEngine.UI;

public class PopUpButton : MonoBehaviour
{
    Button Button;
    Animator animator;
    private void Awake()
    {
        Button = GetComponent<Button>();
        animator = GetComponent<Animator>();
        Button.onClick.AddListener(() =>
        {
            animator.Play("Hide");
            
        });
    }
    void Start()
    {

    }




}
