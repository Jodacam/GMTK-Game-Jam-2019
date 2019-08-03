using Pathfinding;
using UnityEngine;

public class Seaker : EnemyController {
    

    private IAstarAI aiController;

    

    [Tooltip("Distancia maxima a la que tiene que estar el jugador para que el seaker le persiga.")]
    public float maxDistance;

    [Tooltip("Distancia a la que se queda la IA para atacar.")]
    public float minDistance;
    private void Start() {
        aiController = GetComponent<IAstarAI>();
        if (aiController != null) aiController.onSearchPath += Update;
    }


    private void Update() {
        float distance = Vector3.Distance(PlayerController.Player.transform.position,transform.position);
        if(distance <= maxDistance){
            if(aiController!= null)
                aiController.destination = PlayerController.Player.transform.position;
        }

        if(distance <= minDistance) {
            aiController.canMove= false;
        }else{
            aiController.canMove = true;
        }
    }

}