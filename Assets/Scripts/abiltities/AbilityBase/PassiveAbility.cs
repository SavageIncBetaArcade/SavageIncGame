//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public abstract class PassiveAbility : BaseAbility
//{
//    public float ActivePeriod = 10.0f;

//    public override void Use()
//    {
//        //Call the method that would grant the character the passive ability
//        Grant();

//        //After the Active perios has ended call diminish to cancel the passive effect
//        //If the active period is zero the passive ability would never diminish
//        if (ActivePeriod > 0.0f)
//            Invoke("Diminish", ActivePeriod);
//    }


//    public abstract void Grant();
//    public abstract void Diminish();
//}
