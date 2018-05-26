using System;
namespace MemoTree
{
    /// <summary>
    /// StateパターンのState（状態）
    /// CommandパターンのInvoker（起動者）
    /// </summary>
    public abstract class StateInvoker
    {
        // 起動対象となるCommand
        protected Command m_command;

        /// <summary>
        /// 起動対象となるCommandを設定する
        /// </summary>
        /// <param name="command"></param>
        public void SetCommand(Command command)
        {
            this.m_command = command;
        }

        /// <summary>
        /// 各StateInvokerの処理を行う
        /// </summary>
        /// <param name="context"></param>
        public abstract bool Execute(Context context);
    }
}
