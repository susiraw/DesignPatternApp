using System;
using System.Collections.Generic;

namespace MemoTree
{
    /// <summary>
    /// Deleteコマンド処理
    /// ディレクトリ、または、ファイルを削除する。
    /// </summary>
    internal class DeleteCommand : Command
    {
        private List<string> m_listDeleted = new List<string>();

        /// <summary>
        /// DeleteCommandの処理を実行する
        /// 各コンポーネントが持つデリート処理を呼び出す
        /// </summary>
        /// <returns>The execute.</returns>
        /// <param name="context">Context.</param>
        internal override void Execute(Context context)
        {
            this.m_component.Remove(context, ref this.m_listDeleted);
        }

        /// <summary>
        /// 各Commandの実行後の処理を行う
        /// </summary>
        /// <param name="context">Context.</param>
        internal override void AfterExecute(Context context, bool bClearRedoStack = true)
        {
            base.AfterExecute(context, bClearRedoStack);
            foreach (var strDeleted in this.m_listDeleted)
            {
                Console.WriteLine("Deleted:" + strDeleted);
            }
        }

        /// <summary>
        /// DeleteのUndoは行わない
        /// </summary>
        internal override void SetUndoCommand()
        {
            // TODO 中身を保持する必要があり、実装コストが高いため未実装

        }
    }
}
