using System.IO;
using System.Collections.Generic;

namespace MemoTree
{
    /// <summary>
    /// StateパターンのContext（状況判断）
    /// CommandパターンのClient（利用者）
    /// 現在のState（状態）を保持する。
    /// 1.キー入力モード
    /// 2.文字入力モード
    /// </summary>
    public class Context
    {
        // StateInvoker（各StateInvokerにて動的に入れ替える）
        internal StateInvoker m_stateInvoker;

        // MemoTree配下のすべてのComponent
        internal DirComponent m_componentMain;

        // カレントコンポーネント
        internal DirComponent m_currentComponent;

        // 入力されたディレクトリ名、ファイル名
        internal string m_strInputName;

        // Undo用のコマンドを保持
        internal Stack<Command> m_undoStack = new Stack<Command>();
        // Redo用のコマンドを保持
        internal Stack<Command> m_redoStack = new Stack<Command>();

        /// <summary>
        /// コンストラクタ
        /// MemoTreeの起動を行う
        /// </summary>
        public Context()
        {
            // MemoTreeディレクトリの存在チェック
            var strInitPath = Directory.GetCurrentDirectory() + "/MemoTree";
            if (!Directory.Exists(strInitPath))
            {
                Directory.CreateDirectory(strInitPath);
            }
            this.m_componentMain = new DirComponent(strInitPath);
            // 全ディレクトリ、ファイルを再帰的に取得
            DirComponent.SetComponent(this.m_componentMain);
            this.m_currentComponent = this.m_componentMain;

            // 起動時はキー入力モードを設定
            this.m_stateInvoker = KeyInputStateInvoker.GetInstance();
            // 起動時はEnterコマンドを直接実行
            var enterCommand = new EnterCommand();
            enterCommand.CallExecute(this, this.m_componentMain, false);
        }

        /// <summary>
        /// MemoTreeの処理を実行する
        /// 処理の終了が返却されるまで、各StateInvokerの処理を繰り返す。
        /// </summary>
        public void ExecuteMemoTree()
        {
            while (this.m_stateInvoker.Execute(this)) { }
        }

        /// <summary>
        /// StateInvokerを設定する
        /// 各StateInvokerにて動的に入れ替える
        /// </summary>
        /// <param name="stateInvoker"></param>
        internal void SetState(StateInvoker stateInvoker)
        {
            this.m_stateInvoker = stateInvoker;
        }
    }
}