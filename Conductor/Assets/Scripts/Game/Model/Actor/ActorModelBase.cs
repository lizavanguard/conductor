using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conductor.Game.View;

namespace Conductor.Game.Model
{
    public abstract class ActorModelBase
    {
        static int nextId = 0;

        int id;

        public int Id { get { return id; } }

        // FIXME: ActorViewBaseができたらその参照を持つようにする
        ActorViewBase viewBase;

        protected ActorViewBase ViewBase { get { return viewBase; } }

        Vector3 horizontalDirection;

        protected Vector3 HorizontalDirection
        {
            set { horizontalDirection = value; }
            get { return horizontalDirection; }
        }

        public ActorModelBase(ActorViewBase viewBase)
        {
            this.viewBase = viewBase;

            // FIXME: 初期化用のinfo構造体から初期化する
            horizontalDirection = Vector3.forward;
            viewBase.transform.localPosition = Vector3.zero;

            // 初期状態をViewに反映
            viewBase.UpdateRotationByDirection(horizontalDirection);

            id = nextId;
            nextId++;
        }

        public abstract void Update();

        public abstract void Walk(bool front);
    }
}
