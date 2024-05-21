using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTestLeo : MonoBehaviour
{
    [SerializeField] private int damage;
    private bool isAttacking = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAttacking = true;
        }
    }
    
    
    private void OnTriggerStay(Collider other)
    {
        TowerHealthSystem tower = other.GetComponent<TowerHealthSystem>();
        if (tower)
        {
            Debug.Log("Estoy en el rango de una torre");
            if(isAttacking)
            {
                tower.TakeDamage(damage);
                Debug.Log("Enemigo haciendo daño");
                isAttacking = false;
            }
        }
    }

    private SphereCollider sphereCollider;

    private void OnDrawGizmos()
    {
        // Obtener el SphereCollider adjunto
        sphereCollider = GetComponent<SphereCollider>();

        // Configurar el color de los Gizmos
        Gizmos.color = Color.red;

        // Dibujar la esfera en la posición y con el radio del SphereCollider
        Gizmos.DrawWireSphere(transform.position + sphereCollider.center, sphereCollider.radius);
    }
}
