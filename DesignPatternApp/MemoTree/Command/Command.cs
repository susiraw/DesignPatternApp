namespace MemoTree
{
    /// <summary>
    /// CommandパターンのCommand（命令）
    /// 各インターフェースを定義する。
    /// </summary>
    internal abstract class Command
    {
        // CommandパターンのReceiver（受信者）
        internal Component m_component;

        // Undo用のCommand
        internal Command m_undoCommand;

        // コマンド保持フラグ（true:Undo,Redoが実行可能）
        internal bool m_bStack = true;

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
        /// 各Commandの実行に必要な処理を呼び出す
        /// TemplateMethodパターン
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="component">Component.</param>
        /// <param name="bStack"></param>
        /// <param name="bClearRedoStack"></param>
        internal void CallExecute(Context context, Component component, bool bStack = true, bool bClearRedoStack = true)
        {
            this.BeforeExecute(component, bStack);
            this.Execute(context);
            this.AfterExecute(context, bClearRedoStack);
        }

        /// <summary>
        /// 各Commandの実行前の処理を行う
        /// </summary>
        /// <param name="component"></param>
        /// <param name="bStack"></param>
        internal virtual void BeforeExecute(Component component, bool bStack)
        {
            this.m_bStack = bStack;
            if (component != null)
            {
                this.SetComponent(component);
            }
        }

        /// <summary>
        /// 各Commandの処理を行う
        /// </summary>
        /// <param name="context"></param>
        internal abstract void Execute(Context context);

        /// <summary>
        /// 各Commandの実行後の処理を行う
        /// </summary>
        /// <param name="context">Context.</param>
        internal virtual void AfterExecute(Context context, bool bClearRedoStack = true)
        {
            // コンソールを初期化
            Common.InitConsole();
            // カレントコンポーネント配下の全ファイル名を出力
            var lstStrFileName = DirComponent.GetAllFileName((DirComponent)context.m_currentComponent);
            Common.OutputFileName(lstStrFileName);
            // Redoをクリア
            if (bClearRedoStack)
            {
                context.m_redoStack.Clear();
            }
            // Undoをスタック
            if (this.m_bStack)
            {
                this.SetUndoCommand();
                context.m_undoStack.Push(this);
            }
        }
    }
}
