using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBot : MonoBehaviour
{
    [SerializeField] Animator anim;
    int counter = 0;

    private void OnMouseDown()
    {
        switch (counter)
        {
            case 0:
                anim.SetBool("Packed", true);
                counter++;
                break;
            case 1:
                anim.SetBool("Packed", false);
                anim.SetBool("battle", true);
                counter++;
                break;
            case 2:
                anim.SetBool("battle", false);
                anim.SetTrigger("hitLeft");
                counter++;
                break;
            case 3:
                anim.SetTrigger("hitRight");
                counter++;
                break;
            case 4:
                anim.SetBool("die", true);
                counter++;
                break;
            case 5:
                anim.SetBool("die", false);
                anim.SetTrigger("hitRight");
                counter = 0;
                break;
            default:
                break;
        }
    }



}
