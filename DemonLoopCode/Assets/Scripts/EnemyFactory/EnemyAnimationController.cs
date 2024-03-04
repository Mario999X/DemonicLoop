using UnityEngine;

// Clase encargada de manejar las animaciones de los enemigos en 3D.
public class EnemyAnimationController : MonoBehaviour
{
    private EnemyMechanics enemyMechanics;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        enemyMechanics = transform.parent.GetComponent<EnemyMechanics>();

        if(enemyMechanics.Patrol)
        {
            WalkingAnimation();
        }
    }

    public void WalkingAnimation()
    {
        animator.SetInteger("State", 1);
    }
}
