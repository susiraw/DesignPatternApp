using System;

namespace MemoTree
{
    /// <summary>
    /// 文字入力モードのInvoker
    /// 文字入力のみを行い、Commandの実行は行わない。
    /// ※Invokerとしての機能はない
    /// </summary>
    internal class CharInputStateInvoker : StateInvoker
    {
        #region Singleton
        /// <summary>
        /// Singletonパターンにて実装
        /// </summary>
        private static StateInvoker stateInvoker;

        /// <summary>
        /// Singletonパターンにて実装
        /// </summary>
        private CharInputStateInvoker()
        {
        }

        /// <summary>
        /// Singletonパターンにて実装
        /// </summary>
        /// <returns></returns>
        internal static StateInvoker GetInstance()
        {
            if (stateInvoker == null)
            {
                stateInvoker = new CharInputStateInvoker();
            }
            return stateInvoker;
        }
        #endregion

        // Undo用のディレクトリ名、ファイル名
        internal string m_strUndoName = null;

        /// <summary>
        /// 文字入力モードの処理を行う
        /// ※文字入力モードでは常にtrueを返却する
        /// </summary>
		/// <param name="context"></param>
        /// <returns>true:処理の続行、false:処理の終了</returns>
        internal override bool Execute(Context context)
        {
            // 文字入力を受け付ける
            var str = Console.ReadLine();
            context.m_strInputName = str;
            // Commandを実行
            base.m_command.CallExecute(context, null);
            // 実行後、クリア
            context.m_strInputName = null;
            // キー入力モードを設定し、処理の継続を返却
            context.SetState(KeyInputStateInvoker.GetInstance());

            return true;
        }
    }
}