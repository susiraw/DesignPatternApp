using System;
namespace MemoTree
{
    /// <summary>
    /// StateパターンのState（状態）
    /// CommandパターンのInvoker（起動者）
    /// </summary>
    internal abstract class StateInvoker
    {
        // 起動対象となるCommand
        protected Command m_command;

        /// <summary>
        /// 起動対象となるCommandを設定する
        /// </summary>
        /// <param name="command"></param>
        internal void SetCommand(Command command)
        {
            this.m_command = command;
        }

        /// <summary>
        /// 各StateInvokerの処理を行う
        /// </summary>
        /// <param name="context"></param>
        internal abstract bool Execute(Context context);
    }
}
