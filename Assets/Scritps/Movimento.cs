using UnityEngine;
using UnityEngine.InputSystem;

public class Movimento : MonoBehaviour
{
    
    public float speed = 3.0f;  // Velocidade de movimento do personagem
    private MeuControle controls;
    private Vector2 moveInput;  // Para armazenar a entrada de movimento
    private Animator animator;  // Referência ao componente Animator


    void Awake() {
        controls = new MeuControle();
        animator = GetComponent<Animator>();  // Inicializa o componente Animator

        if (!animator) {
            Debug.LogWarning("Animator component missing on this GameObject");
        }

        // Configura os listeners para a ação de movimento
        controls.Personagem.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Personagem.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnEnable() {
        controls.Personagem.Enable();  // Ativa o mapa de ações Personagem
    }

    void OnDisable() {
        controls.Personagem.Disable();  // Desativa o mapa de ações Personagem
    }

    void Update() {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);


        if (move != Vector3.zero) {
            // Rotaciona suavemente para a direção do movimento
            Vector3 targetDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            transform.forward = Vector3.Slerp(transform.forward, targetDirection, Time.deltaTime * 15);
        }

        // Move o personagem
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        // Atualiza o estado da animação baseado na movimentação
        if (animator) {
            bool isMoving = moveInput != Vector2.zero;
            animator.SetBool("andando", isMoving);
        }
    }
}
