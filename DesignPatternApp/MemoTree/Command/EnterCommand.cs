using System;
using System.Collections.Generic;

namespace MemoTree
{
    /// <summary>
    /// Enterコマンド処理
    /// 対象がディレクトリの場合、配下のファイル名を表示する。
    /// 対象がファイルの場合、対象ファイルを開く。
    /// </summary>
    internal class EnterCommand : Command
    {
        // Undo用のコマンド実行前コンポーネント
        internal DirComponent m_beforeComponent;

        /// <summary>
        /// EnterCommandの処理を実行する
        /// </summary>
        /// <param name="context"></param>
        internal override void Execute(Context context)
        {
            if (base.m_component.GetType() == typeof(DirComponent))
            {
                // Undo用にカレントコンポーネントを保持
                this.m_beforeComponent = context.m_currentComponent;
                // カレントコンポーネントを設定
                context.m_currentComponent = (DirComponent)base.m_component;
            }
            else
            {
                Common.FileOpen(base.m_component.m_strPath);
                base.m_bStack = false;
            }

        }

        /// <summary>
        /// Undo用のCommandとしてEnterCommandを設定する
        /// </summary>
        internal override void SetUndoCommand()
        {
            var enterCommand = new EnterCommand();
            enterCommand.SetComponent(this.m_beforeComponent);
            base.m_undoCommand = enterCommand;
        }
    }
}
