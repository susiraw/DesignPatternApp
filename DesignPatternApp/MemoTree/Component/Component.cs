using System;
using System.Collections.Generic;
using System.IO;

namespace MemoTree
{
    /// <summary>
    /// CompositeパターンのComponent（部品）
    /// CommandパターンのReceiver（受信者）
    /// DirComponentとFileComponentを同一に扱うためのインターフェースを定義する。
    /// Commandの処理対象となるインターフェースを定義する。
    /// </summary>
    internal abstract class Component
    {
        // 対象コンポーネントのパス
        internal string m_strPath;
        // 対象コンポーネントの名前
        internal string m_strName;

        /// <summary>
        /// サブコンポーネントの追加を行う
        /// DirComponentでのみ使用するため、仮想メソッドにて定義
        /// </summary>
        internal virtual void Add(Component component)
        {
        }

        /// <summary>
        /// コンポーネントの削除を行う
        /// 対象となるコンポーネントのファイル名を出力し、削除する。
        /// </summary>
        internal virtual void Remove(Context context, ref List<string> listDeleted)
        {
            listDeleted.Add(this.m_strName);
            File.Delete(this.m_strPath);
            context.m_currentComponent.m_listComponent.Remove(this);
        }
    }
}