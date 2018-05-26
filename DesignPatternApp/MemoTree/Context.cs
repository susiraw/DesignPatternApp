using System.IO;

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
        // TODO プロパティ化の検討
        // StateInvoker（各StateInvokerにて動的に入れ替える）
        internal StateInvoker m_stateInvoker;

        // TODO プロパティ化の検討
        // MemoTree配下のすべてのComponent
        internal DirComponent m_componentMain;

        // TODO プロパティ化の検討
        // TODO パスではなくカレントコンポーネントを検討
        // カレントパス
        internal string m_currentDirectory;

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
            this.m_currentDirectory = strInitPath;
            this.m_componentMain = new DirComponent(strInitPath);
            // 全ディレクトリ、ファイルを再帰的に取得
            DirComponent.SetComponent(this.m_componentMain);

            // 起動時はキー入力モードを設定
            this.m_stateInvoker = KeyInputStateInvoker.GetInstance();
            // 起動時はEnterコマンドを直接実行
            var command = new EnterCommand();
            command.SetComponent(this.m_componentMain);
            command.Execute(this);
        }

        /// <summary>
        /// StateInvokerを設定する
        /// 各StateInvokerにて動的に入れ替える
        /// </summary>
        /// <param name="stateInvoker"></param>
        public void SetState(StateInvoker stateInvoker)
        {
            this.m_stateInvoker = stateInvoker;
        }

        /// <summary>
        /// MemoTreeの処理を実行する
        /// 処理の終了が返却されるまで、各StateInvokerの処理を繰り返す。
        /// </summary>
        public void ExecuteMemoTree()
        {
            while (this.m_stateInvoker.Execute(this)) { }
        }
    }
}