using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.View
{
    [CreateAssetMenu(menuName = "Data/ActorPrefabReference", fileName = "ActorPrefabReference")]
    public class ActorPrefabReference : ScriptableObject
    {
        [SerializeField]
        ActorViewSoldier soldier;
        public ActorViewSoldier Soldier { get { return soldier; } }

        [SerializeField]
        FieldView field;
        public FieldView Field { get { return field; } }
    }
}
