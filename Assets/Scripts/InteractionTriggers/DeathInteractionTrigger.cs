using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DeathInteractionTrigger : InteractionTrigger
{
    private CharacterBase characterBase;

    protected override void Awake()
    {
        base.Awake();

        characterBase = GetComponent<CharacterBase>();
        characterBase.OnDeath += onDeath;
    }

    void OnDestroy()
    {
        characterBase.OnDeath -= onDeath;
    }

    void onDeath()
    {
        Interact();
    }
}