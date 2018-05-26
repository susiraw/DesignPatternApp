namespace MemoTree
{
    /// <summary>
    /// Deleteコマンド処理
    /// ディレクトリ、または、ファイルを削除する。
    /// </summary>
    public class DeleteCommand : Command
    {
        /// <summary>
        /// DeleteCommandの処理を実行する
        /// </summary>
        /// <returns>The execute.</returns>
        /// <param name="context">Context.</param>
        internal override void Execute(Context context)
        {
            // TODO DeleteCommandの処理を実行する
        }

        /// <summary>
        /// DeleteのUndoは行わない
        /// </summary>
        internal override void SetUndoCommand()
        {
            // TODO 実装コストが高いため要検討
        }
    }
}
