using UnityEditor;
using UnityEngine;

public class MyEditorExtensions
{
    [MenuItem("CONTEXT/Animator/Create New Animator Controller", priority = 0)]
#pragma warning disable IDE0051 // Kullanılmayan özel üyeleri kaldır
    private static void CreateNewAnimator(MenuCommand menuCommand)
    {
        // The Animator component can be extracted from the menu command using the context field.
        Animator animator = menuCommand.context as Animator;

        string path = EditorUtility.SaveFilePanelInProject("Create new Animator Controller", animator.gameObject.name + ".controller", "controller", "Please enter a file name to save the Animator Controller to");

        if (path.Length != 0)
        {
            animator.runtimeAnimatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(path);
        }

    }
#pragma warning restore IDE0051 // Kullanılmayan özel üyeleri kaldır

}