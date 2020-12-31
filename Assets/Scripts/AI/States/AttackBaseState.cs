using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStates/AttackBase")]
public class AttackBaseState : State
{
    public override void OnUpdate(ref StackFSM stackStates)
    {
        ref AIBase aiBase = ref stackStates.aiBase;
        Transform player = aiBase.Player.transform;

        float distanceToPlayer = Vector3.Distance(aiBase.transform.position, player.position);
        if (distanceToPlayer <= aiBase.AttackDistance)
        {
            if (aiBase.abilityTransform)
            {
                aiBase.abilityTransform.LookAt(player.transform);
            }
            Vector3 lookRotation = Quaternion.LookRotation(player.position - aiBase.transform.position).eulerAngles;
            aiBase.transform.rotation = Quaternion.Euler(Vector3.Scale(lookRotation, Vector3.up));

            if(aiBase.FirstAbility) aiBase.LeftAbility?.Attack();
            else aiBase.RightAbilitiy?.Attack();
        }
        else
        {
            stackStates.PopState();
        }
    }
}
