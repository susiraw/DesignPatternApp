namespace MemoTree
{
    /// <summary>
    /// CommandパターンのCommand（命令）
    /// 各インターフェースを定義する。
    /// </summary>
    public abstract class Command
    {
        // TODO プロパティ化の検討
        // CommandパターンのReceiver（受信者）
        internal Component m_component;

        // TODO プロパティ化の検討
        // Undo用のCommand
        internal Command m_undoCommand;

        /// <summary>
        /// Componentをセットする
        /// </summary>
        /// <param name="component"></param>
        internal void SetComponent(Component component)
        {
            this.m_component = component;
        }

        /// <summary>
        /// Undo用のCommandをセットする
        /// </summary>
        internal abstract void SetUndoCommand();

        /// <summary>
        /// 各Commandの処理を行う
        /// </summary>
        /// <param name="context"></param>
        internal abstract void Execute(Context context);
    }
}
