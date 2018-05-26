namespace MemoTree
{
    /// <summary>
    /// CompositeパターンのComponent（部品）
    /// CommandパターンのReceiver（受信者）
    /// DirComponentとFileComponentを同一に扱うためのインターフェースを定義する。
    /// Commandの処理対象となるインターフェースを定義する。
    /// </summary>
    public abstract class Component
    {
        // TODO プロパティ化の検討
        // 対象コンポーネントのパス
        internal string m_strPath;
        // 対象コンポーネントの名前
        internal string m_strName;

        // TODO 削除フラグを持つか検討

        // TODO 現状は未使用のため使用するよう修正
        /// <summary>
        /// サブコンポーネントの追加を行う
        /// DirComponentでのみ使用するため、仮想メソッドにて定義
        /// </summary>
        protected virtual void Add()
        {
        }

        /// <summary>
        /// サブコンポーネントの削除を行う
        /// DirComponentでのみ使用するため、仮想メソッドにて定義
        /// </summary>
        protected virtual void Remove()
        {
        }
    }
}