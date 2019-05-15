using UnityEngine;
using Conductor.Game.View;

namespace Conductor.Game.Model
{
    public class FieldModel
    {
        FieldView view;

        public FieldModel(FieldView view)
        {
            this.view = view;

            // FIXME: モデル情報を食わせたりとかやるべき そもそも地形の要件決めてない
            // できれば崖とかつくりたいので、一般的なモデル＆ナビメッシュでやるのがいいような気はする
        }

        // 戦場のサイズ取得
        public Vector2 GetAreaSize()
        {
            return Vector2.one * 100.0f;
        }

        // 高さ取得
        public float GetHeight(Vector2 horizontalPosition)
        {
            return 0.0f;
        }
    }
}
